// OD2 case authoring and structural validation.
// Usage (from research/2026-comparative-program):
//   dotnet fsi harness/Author.fsx author <dev|heldout> <workRoot> [caseId ...]
//   dotnet fsi harness/Author.fsx validate <dev|heldout>
// Cases go to benchmark/cases/<split>/, truth to benchmark/truth/<split>/,
// transcripts and run records to benchmark/authoring/raw/.

#load "Claude.fsx"
#load "Domain.fsx"

open System
open System.IO
open System.Text.Json
open Claude
open Domain

let contractTemplate = File.ReadAllText "benchmark/authoring/authoring-contract.md"

let fill (a: JsonElement) (split: string) =
    let cls = strOr "" "class" a
    contractTemplate
        .Replace("{{ID}}", strOr "" "id" a)
        .Replace("{{SPLIT}}", split)
        .Replace("{{CLASS}}", cls)
        .Replace("{{CLASS_DEFINITION}}", Map.find cls classDefinitions)
        .Replace("{{DOMAIN}}", strOr "" "domain" a)
        .Replace("{{SETTING}}", strOr "" "setting" a)

let authorSchema =
    """{"type":"object","properties":{"case":{"type":"object"},"truth":{"type":"object"}},"required":["case","truth"]}"""

let assignments (split: string) =
    [ "benchmark/authoring/assignments.json"; "benchmark/authoring/assignments-amendment-01.json" ]
    |> List.filter File.Exists
    |> List.collect (fun p -> readJson p |> arr split)

let authorSpec (workRoot: string) (split: string) (a: JsonElement) : RunSpec =
    let id = strOr "" "id" a
    let dir = Path.Combine(workRoot, "author", id)
    Directory.CreateDirectory dir |> ignore
    File.WriteAllText(Path.Combine(dir, "task.md"), fill a split)
    { Label = id
      WorkDir = dir
      Prompt = "Read the file task.md in the current directory. It contains your complete task. Follow it exactly and return the result through the structured output tool. Do not read any other file."
      SystemPrompt = "You are an expert author of fair, difficult diagnostic benchmark cases. You write precise, realistic evidence and defensible answer keys."
      Model = "claude-opus-5-5"
      Effort = "high"
      Schema = authorSchema
      TranscriptPath = Path.GetFullPath(Path.Combine("benchmark/authoring/raw", id + ".jsonl"))
      TimeoutMinutes = 40 }

let pretty (e: JsonElement) = JsonSerializer.Serialize(e, JsonSerializerOptions(WriteIndented = true))

let save (split: string) (r: RunResult) =
    File.WriteAllText(Path.Combine("benchmark/authoring/raw", r.Label + ".run.json"), resultToJson r)
    match r.Output with
    | Some o ->
        let c = prop "case" o
        let t = prop "truth" o
        match c, t with
        | Some c, Some t ->
            Directory.CreateDirectory(sprintf "benchmark/cases/%s" split) |> ignore
            Directory.CreateDirectory(sprintf "benchmark/truth/%s" split) |> ignore
            File.WriteAllText(sprintf "benchmark/cases/%s/%s.case.json" split r.Label, pretty c)
            File.WriteAllText(sprintf "benchmark/truth/%s/%s.truth.json" split r.Label, pretty t)
            printfn "authored %s (%.2f USD, %d s)" r.Label r.CostUsd (r.DurationMs / 1000L)
        | _ -> printfn "FAILED %s: output lacks case or truth" r.Label
    | None -> printfn "FAILED %s: %s" r.Label (r.Error |> Option.defaultValue "no output")

let validateSplit (split: string) =
    let dir = sprintf "benchmark/cases/%s" split
    let results =
        Directory.GetFiles(dir, "*.case.json")
        |> Array.sort
        |> Array.map (fun p ->
            let c = parseCase (readJson p)
            let t = parseTruth (readJson (sprintf "benchmark/truth/%s/%s.truth.json" split c.Id))
            c, t, validate c t)
    results
    |> Array.iter (fun (c, t, problems) ->
        printfn "%s class=%s domain=%s initial=%d catalogue=%d items=%d disc=%d problems=%d"
            c.Id c.Class c.Domain c.Initial.Length c.Catalogue.Length c.Items.Length t.Discriminating.Count problems.Length
        problems |> List.iter (printfn "   - %s"))
    let total = results |> Array.sumBy (fun (_, _, p) -> p.Length)
    printfn "cases=%d problems=%d" results.Length total
    if total > 0 then exit 3

match fsi.CommandLineArgs |> Array.toList |> List.tail with
| "author" :: split :: workRoot :: ids ->
    Directory.CreateDirectory "benchmark/authoring/raw" |> ignore
    let chosen =
        assignments split
        |> List.filter (fun a -> ids.IsEmpty || List.contains (strOr "" "id" a) ids)
    chosen
    |> List.map (authorSpec workRoot split)
    |> runAll 6 (save split)
    |> fun rs -> printfn "done: %d ok, %d failed, %.2f USD" (rs |> List.filter (fun r -> r.Ok) |> List.length) (rs |> List.filter (fun r -> not r.Ok) |> List.length) (rs |> List.sumBy (fun r -> r.CostUsd))
| [ "validate"; split ] -> validateSplit split
| _ ->
    eprintfn "usage: author <split> <workRoot> [ids] | validate <split>"
    exit 2
