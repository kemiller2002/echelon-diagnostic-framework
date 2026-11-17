// OD2 Track B: blind rating of briefs for persuasiveness, perceived correctness and
// actionability. The rater sees the case setting, request and initial evidence and one
// brief with method vocabulary scrubbed. It never sees truth, condition, scores or
// catalogue contents. Primary scoring never reads these ratings.
// Usage (from research/2026-comparative-program):
//   dotnet fsi harness/Rate.fsx rate <scoredDir> <workRoot> <ratingsDir> [conditionFilter,...]
// Reads <scoredDir>/briefs.csv; writes one record per brief to <ratingsDir>.

#load "Claude.fsx"
#load "Domain.fsx"

open System
open System.IO
open System.Text
open System.Text.Json
open System.Text.RegularExpressions
open Claude
open Domain

/// Fixed, preregistered substitutions that remove method vocabulary (case-insensitive, whole phrase).
let scrubRules =
    [ @"\borigin networks?\b", "set of causes"
      @"\bcontrol points?\b", "intervention point"
      @"\bpropagation\b", "spread"
      @"\bsystem context\b", "scope"
      @"\bdiagnostic sufficiency\b", "whether the evidence is enough"
      @"\bmanifestations?\b", "symptom"
      @"\bnext responsible action\b", "next step"
      @"\bis\s*/\s*is not\b", "comparison"
      @"\bkepner[- ]tregoe\b", "the analysis"
      @"\bcompeting hypotheses\b", "alternative explanations"
      @"\bEDF\b", "the analysis" ]

let scrub (text: string) =
    scrubRules |> List.fold (fun (t: string) (pat, rep) -> Regex.Replace(t, pat, rep, RegexOptions.IgnoreCase)) text

let rateSchema =
    """{"type":"object","properties":{"persuasiveness":{"type":"integer","minimum":1,"maximum":7},"perceivedCorrectness":{"type":"integer","minimum":0,"maximum":100},"actionability":{"type":"integer","minimum":1,"maximum":7},"reason":{"type":"string"}},"required":["persuasiveness","perceivedCorrectness","actionability","reason"]}"""

let task (c: Case) (brief: string) =
    let sb = StringBuilder()
    let line (s: string) = sb.AppendLine s |> ignore
    line "You are the person who asked for this diagnosis. An analyst has sent you the brief below. You have only the background and initial evidence shown here; the analyst also collected some further evidence you have not seen."
    line "Rate the brief:"
    line "- persuasiveness (1-7): how convinced are you by it?"
    line "- perceivedCorrectness (0-100): how likely do you think its diagnosis is correct?"
    line "- actionability (1-7): how clearly does it tell you what to do next?"
    line "Give a one-sentence reason. Judge the brief as a decision-maker would; you do not know the true answer."
    line ""
    line ("Background: " + c.Setting)
    line ("Your request: " + c.Request)
    line "Initial evidence:"
    for e in c.Initial do line (sprintf "- %s (%s): %s" e.Source e.SourceType e.Text)
    line ""
    line "BRIEF:"
    line (scrub brief)
    sb.ToString()

// minimal CSV reader (quoted fields)
let parseLine (line: string) =
    let rec go i inQ (cur: StringBuilder) acc =
        if i >= String.length line then List.rev (cur.ToString() :: acc)
        else
            let ch = line.[i]
            match inQ, ch with
            | true, '"' when i + 1 < line.Length && line.[i + 1] = '"' -> go (i + 2) true (cur.Append '"') acc
            | true, '"' -> go (i + 1) false cur acc
            | true, _ -> go (i + 1) true (cur.Append ch) acc
            | false, '"' -> go (i + 1) true cur acc
            | false, ',' -> go (i + 1) false (StringBuilder()) (cur.ToString() :: acc)
            | false, _ -> go (i + 1) false (cur.Append ch) acc
    go 0 false (StringBuilder()) []

match fsi.CommandLineArgs |> Array.toList |> List.tail with
| "rate" :: scoredDir :: workRoot :: ratingsDir :: rest ->
    let filter = match rest with f :: _ -> f.Split(',') |> Set.ofArray | [] -> Set.empty
    let rows =
        (readJson (Path.Combine(scoredDir, "briefs.json"))).EnumerateArray()
        |> Seq.map (fun e -> [ "runId"; "condition"; "caseId"; "brief" ] |> List.map (fun k -> k, strOr "" k e) |> Map.ofList)
        |> List.ofSeq
    let cases =
        [ "dev"; "dev2"; "heldout" ] |> List.filter (fun s -> Directory.Exists(sprintf "benchmark/cases/%s" s))
        |> List.collect (fun s -> Directory.GetFiles(sprintf "benchmark/cases/%s" s, "*.case.json") |> List.ofArray)
        |> List.map (readJson >> parseCase) |> List.map (fun c -> c.Id, c) |> Map.ofList
    Directory.CreateDirectory ratingsDir |> ignore
    let pending =
        rows
        |> List.filter (fun r -> filter.IsEmpty || filter.Contains(Map.find "condition" r))
        |> List.filter (fun r -> (Map.find "brief" r).Trim() <> "")
        |> List.filter (fun r -> not (File.Exists(Path.Combine(ratingsDir, Map.find "runId" r + ".rating.json"))))
    printfn "rating %d briefs" pending.Length
    let specs =
        pending |> List.map (fun r ->
            let id = Map.find "runId" r
            let dir = Path.Combine(workRoot, "rate", id)
            if Directory.Exists dir then Directory.Delete(dir, true)
            Directory.CreateDirectory dir |> ignore
            File.WriteAllText(Path.Combine(dir, "task.md"), task (Map.find (Map.find "caseId" r) cases) (Map.find "brief" r))
            { Label = id; WorkDir = dir
              Prompt = "Read the file task.md in the current directory and follow it. Return your rating through the structured output tool. Do not read any other file."
              SystemPrompt = "You are a busy decision-maker reading an analyst's brief."
              Model = "claude-opus-5-5"; Effort = "low"; Schema = rateSchema
              TranscriptPath = Path.GetFullPath(Path.Combine(ratingsDir, "transcripts", id + ".jsonl")); TimeoutMinutes = 15 })
    specs
    |> runAll 8 (fun res -> File.WriteAllText(Path.Combine(ratingsDir, res.Label + ".rating.json"), resultToJson res))
    |> fun rs -> printfn "rated %d ok, %d failed, %.2f USD" (rs |> List.filter (fun r -> r.Ok) |> List.length) (rs |> List.filter (fun r -> not r.Ok) |> List.length) (rs |> List.sumBy (fun r -> r.CostUsd))
| _ ->
    eprintfn "usage: rate <scoredDir> <workRoot> <ratingsDir> [conditions]"
    exit 2
