/// EDF site browser conformance: geometry, targets, navigation, keyboard focus,
/// axe WCAG 2.2 AA, long-content stress and 200% text, at phone to desktop widths.
/// Usage: dotnet run -- --base-url http://127.0.0.1:8765 --out artifacts/browser
module EdfSite.Program

open System
open System.IO
open System.Text.Json
open System.Threading.Tasks
open Microsoft.Playwright
open Deque.AxeCore.Commons
open Deque.AxeCore.Playwright

type Finding =
    { Page: string
      Width: int
      Mode: string
      Kind: string
      Target: string
      Detail: string }

type Options = { BaseUrl: string; Out: string }

let pages = [ "index.html"; "learn.html"; "examples.html"; "evidence.html"; "start.html" ]

/// 320/375/430 phones, 768 tablet, 1280 desktop.
let widths = [ 320; 375; 430; 768; 1280 ]

let stressWidths = set [ 320; 375; 1280 ]

let wcagTags = [ "wcag2a"; "wcag2aa"; "wcag21a"; "wcag21aa"; "wcag22aa" ]

let rec parseArgs (options: Options) args =
    match args with
    | "--base-url" :: value :: rest -> parseArgs { options with BaseUrl = value.TrimEnd '/' } rest
    | "--out" :: value :: rest -> parseArgs { options with Out = value } rest
    | [] -> Ok options
    | unknown :: _ -> Error $"unknown argument: {unknown}"

let finding page width mode (element: JsonElement) =
    let text (name: string) =
        match element.TryGetProperty name with
        | true, value -> value.ToString()
        | _ -> ""

    { Page = page
      Width = width
      Mode = mode
      Kind = text "kind"
      Target = text "target"
      Detail = text "detail" }

let findingsFrom page width mode (result: JsonElement) =
    result.EnumerateArray() |> Seq.map (finding page width mode) |> List.ofSeq

let evaluate (page: IPage) (script: string) (arg: obj) =
    task {
        let! result = page.EvaluateAsync<JsonElement>(script, arg)
        return result
    }

let probe (page: IPage) name width mode script (arg: obj) =
    task {
        let! result = evaluate page script arg
        return findingsFrom name width mode result
    }

let geometryOf page name width mode =
    probe page name width mode Probes.geometry (box Probes.ContainerSelector)

/// Tabs through the page: the skip link must come first, and every stop must paint
/// a visible indicator inside the viewport.
let focusWalk (page: IPage) name width =
    let check index (state: JsonElement) =
        let flag (p: string) = state.GetProperty(p).GetBoolean()
        let target = state.GetProperty("target").GetString() |> Option.ofObj |> Option.defaultValue ""
        let fail kind detail = [ { Page = name; Width = width; Mode = "keyboard"; Kind = kind; Target = target; Detail = detail } ]
        [ if index = 0 && not (flag "isSkipLink") then yield! fail "focus-order" "first Tab stop is not the skip link"
          if not (flag "visibleIndicator") then yield! fail "focus-invisible" $"Tab stop {index + 1} paints no focus indicator"
          if not (flag "inViewport") then yield! fail "focus-offscreen" $"Tab stop {index + 1} is outside the viewport" ]

    let step (acc: Task<bool * Finding list>) index =
        task {
            let! stopped, found = acc
            if stopped then
                return stopped, found
            else
                do! page.Keyboard.PressAsync "Tab"
                let! state = evaluate page Probes.focused null
                match state.ValueKind with
                | JsonValueKind.Null -> return true, found
                | _ -> return false, found @ check index state
        }

    task {
        let! count = page.EvaluateAsync<int>("() => document.querySelectorAll('a[href], button, input, select, textarea, [tabindex]:not([tabindex=\"-1\"])').length")
        let! _, findings = List.fold step (Task.FromResult(false, [])) [ 0 .. count - 1 ]
        return findings
    }

let axe (page: IPage) name width =
    task {
        let options = AxeRunOptions(RunOnly = RunOnlyOptions(Type = "tag", Values = ResizeArray wcagTags))
        let! result = page.RunAxe(options)
        return
            result.Violations
            |> Seq.map (fun v ->
                { Page = name
                  Width = width
                  Mode = "axe"
                  Kind = $"axe:{v.Id}"
                  Target = v.Nodes |> Seq.map (fun n -> string n.Target) |> String.concat ", "
                  Detail = $"{v.Impact}: {v.Help}" })
            |> List.ofSeq
    }

let openPage (browser: IBrowser) (options: Options) name width =
    task {
        let! context = browser.NewContextAsync(BrowserNewContextOptions(ViewportSize = ViewportSize(Width = width, Height = 900), ReducedMotion = ReducedMotion.Reduce))
        let! page = context.NewPageAsync()
        let! _ = page.GotoAsync($"{options.BaseUrl}/{name}", PageGotoOptions(WaitUntil = WaitUntilState.NetworkIdle))
        let! _ = page.EvaluateAsync("() => document.fonts.ready.then(() => true)")
        return context, page
    }

let screenshotPath (options: Options) (name: string) width suffix =
    Path.Combine(options.Out, "screenshots", $"{Path.GetFileNameWithoutExtension name}-{width}{suffix}.png")

let examine (browser: IBrowser) (options: Options) name width =
    task {
        let! context, page = openPage browser options name width
        let! _ = page.ScreenshotAsync(PageScreenshotOptions(Path = screenshotPath options name width "", FullPage = true))
        let! geometry = geometryOf page name width "default"
        let! targets = probe page name width "default" Probes.targets null
        let! navigation = probe page name width "default" Probes.navigation null
        let! accessibility = axe page name width
        let! keyboard = focusWalk page name width
        do! context.CloseAsync()
        return geometry @ targets @ navigation @ accessibility @ keyboard
    }

/// Long unbroken URLs, identifiers and labels injected into the real content.
let stressed (browser: IBrowser) (options: Options) name width =
    task {
        let! context, page = openPage browser options name width
        let! _ = page.EvaluateAsync Probes.stress
        let! _ = page.ScreenshotAsync(PageScreenshotOptions(Path = screenshotPath options name width "-stress", FullPage = true))
        let! findings = geometryOf page name width "long-content"
        do! context.CloseAsync()
        return findings
    }

/// WCAG 1.4.4: text resized to 200% at the narrowest phone width.
let enlarged (browser: IBrowser) (options: Options) name width =
    task {
        let! context, page = openPage browser options name width
        let! _ = page.EvaluateAsync "() => { document.documentElement.style.fontSize = '200%'; }"
        let! _ = page.ScreenshotAsync(PageScreenshotOptions(Path = screenshotPath options name width "-text200", FullPage = true))
        let! findings = geometryOf page name width "text-200"
        do! context.CloseAsync()
        return findings
    }

let sequentially (work: (unit -> Task<Finding list>) list) =
    let step (acc: Task<Finding list>) (next: unit -> Task<Finding list>) =
        task {
            let! before = acc
            let! found = next ()
            return before @ found
        }
    List.fold step (Task.FromResult []) work

let plan browser options =
    [ for width in widths do
          for name in pages do
              yield fun () -> examine browser options name width
              if stressWidths.Contains width then yield fun () -> stressed browser options name width
              if width = 320 then yield fun () -> enlarged browser options name width ]

let launchOptions () =
    match Environment.GetEnvironmentVariable "EDF_CHROMIUM_EXECUTABLE" with
    | null
    | "" -> BrowserTypeLaunchOptions()
    | path -> BrowserTypeLaunchOptions(ExecutablePath = path)

let report (options: Options) (findings: Finding list) =
    let json = JsonSerializer.Serialize(findings, JsonSerializerOptions(WriteIndented = true))
    File.WriteAllText(Path.Combine(options.Out, "report.json"), json)
    let checkedRuns = widths.Length * pages.Length
    printfn "EDF browser conformance: %d page/width runs, widths %A" checkedRuns widths
    findings
    |> List.groupBy (fun f -> f.Kind)
    |> List.iter (fun (kind, items) ->
        printfn "  %s: %d" kind items.Length
        items |> List.truncate 8 |> List.iter (fun f -> printfn "    %s @%d [%s] %s — %s" f.Page f.Width f.Mode f.Target f.Detail))
    printfn "%s" (if List.isEmpty findings then "PASS: 0 findings" else $"FAIL: {findings.Length} finding(s)")

[<EntryPoint>]
let main argv =
    match parseArgs { BaseUrl = "http://127.0.0.1:8765"; Out = "artifacts/browser" } (List.ofArray argv) with
    | Error message ->
        eprintfn "%s" message
        2
    | Ok options ->
        Directory.CreateDirectory(Path.Combine(options.Out, "screenshots")) |> ignore
        let run =
            task {
                use! playwright = Playwright.CreateAsync()
                let! browser = playwright.Chromium.LaunchAsync(launchOptions ())
                let! findings = sequentially (plan browser options)
                do! browser.CloseAsync()
                return findings
            }
        let findings = run.GetAwaiter().GetResult()
        report options findings
        if List.isEmpty findings then 0 else 1
