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

let classDefinitions =
    Map.ofList
        [ "KNOWN", "single root cause that is findable: decisive evidence sits in one or two catalogue items among plausible distractors."
          "INTERACT", "interacting causes: neither cause alone is sufficient, and each alone looks contradicted by a contrast case; single-cause fixes are premature."
          "MISLEAD", "misleading evidence: the initial file contains a salient but misleading signal that only a catalogue item counters."
          "INCOMPLETE", "decisive evidence absent: no catalogue item discriminates the two leading hypotheses, so both stay unknown and SUFF is insufficient; an evidence-gathering action is preferred."
          "CONTRA", "contradictory sources: accounts disagree, and independence and provenance resolve which to trust; an interested party's account is contradicted by an independent record."
          "NOCONC", "no reliable conclusion: noise, base rates or normal variation explain the signal as well as any cause; most hypotheses unknown, edges association-only or unknown, and either SUFF insufficient or 'no intervention warranted' preferred."
          "SEQ", "sequential investigation: the discriminating second catalogue item can only be identified after reading the first; the optimal path is a specific two-step sequence."
          "OBVWRONG", "the obvious diagnosis is wrong: the obvious cause fits the initial file perfectly but a contrast case refutes it, and a less salient cause is supported."
          "CHANGE", "changing conditions: stage-S2 evidence reverses a stage-S1 conclusion; include asOfStage S1 items whose answers differ from the final answers."
          "STOP", "stop early: the initial file already suffices for every item; catalogue items add nothing; SUFF is sufficient and reading zero or one item is optimal."
          "CROSS", "cross-domain: the mechanism spans two domains, and the cause lies outside the domain that shows the symptom."
          "ADVERS", "strategic or deceptive actor: a record or account is gamed or fabricated, and a provenance clue exposes it."
          "CONFOUND", "hidden confounder: a strong correlation is driven by a third factor that the catalogue reveals; the tempting edge is association-only and the confounder hypothesis is supported." ]

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
    (readJson "benchmark/authoring/assignments.json") |> arr split

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
