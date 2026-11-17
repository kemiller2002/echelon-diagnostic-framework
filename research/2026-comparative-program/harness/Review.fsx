// OD2 adversarial case review.
// Stage 1 (blind solve): an isolated session reads the whole case, including every
// catalogue text, and answers every item without seeing the key.
// Stage 2 (adjudication): for each item where the blind answer differs from the key,
// a second isolated session sees the case, the key and the blind answer, and rules
// whether the key is uniquely defensible.
// Stage 3 (revision): the original author contract plus the review findings go to a new
// authoring session, which returns a corrected case and truth.
// Usage (from research/2026-comparative-program):
//   dotnet fsi harness/Review.fsx review <split> <workRoot> [caseId ...]
//   dotnet fsi harness/Review.fsx revise <split> <workRoot> [caseId ...]

#load "Claude.fsx"
#load "Domain.fsx"

open System
open System.IO
open System.Text
open System.Text.Json
open Claude
open Domain

let pretty (e: JsonElement) = JsonSerializer.Serialize(e, JsonSerializerOptions(WriteIndented = true))

let fullCaseText (c: Case) =
    let sb = StringBuilder()
    let line (s: string) = sb.AppendLine s |> ignore
    line (sprintf "CASE %s (%s, %s): %s" c.Id c.Class c.Domain c.Title)
    line ("Setting: " + c.Setting)
    line ("Presenting request: " + c.Request)
    line "INITIAL EVIDENCE"
    for e in c.Initial do line (sprintf "%s [stage %s] %s (%s): %s" e.Id e.Stage e.Source e.SourceType e.Text)
    line "CATALOGUE (full contents)"
    for e in c.Catalogue do line (sprintf "%s [stage %s] %s - %s (%s): %s" e.Id e.Stage e.Title e.Source e.SourceType e.Text)
    line "CANDIDATES"
    for (id, t) in c.Hypotheses do line (sprintf "%s: %s" id t)
    for (id, f, t, r) in c.Edges do line (sprintf "%s: %s -> %s (%s)" id f t r)
    for (id, t) in c.Actions do line (sprintf "%s: %s" id t)
    line "SCORED ITEMS"
    for i in c.Items do
        let st = match i.AsOfStage with Some s -> " asOfStage=" + s | None -> ""
        line (sprintf "%s: %s %s%s allowed=[%s]" i.ItemId i.Kind i.RefId st (String.Join(", ", i.Allowed)))
    sb.ToString()

let semantics =
    let shared = File.ReadAllText "prompts/SHARED.txt"
    let s = shared.IndexOf "Answer semantics:"
    let e = shared.IndexOf "Answer using the initial evidence"
    shared.Substring(s, e - s).Trim()

let solveTask (c: Case) =
    String.concat "\n\n"
        [ "You are reviewing a diagnostic benchmark case before it is used. Solve it as a careful expert would, using the whole case including every catalogue item (no collection limit applies to you). For items with asOfStage set, use only evidence at or before that stage."
          "For CLASS INCOMPLETE and NOCONC cases, answer what the whole catalogue supports. For other classes, answer what the evidence supports after reading the most relevant catalogue items."
          semantics
          "For every scored item give one allowed answer, a confidence 0-100, and a one-sentence justification citing evidence ids. Also list any item you think has two defensible answers, and any evidence that is unrealistic, inconsistent or leaks the answer through a catalogue title."
          fullCaseText c ]

let solveSchema =
    """{"type":"object","properties":{"answers":{"type":"array","items":{"type":"object","properties":{"itemId":{"type":"string"},"answer":{"type":"string"},"confidence":{"type":"integer"},"justification":{"type":"string"}},"required":["itemId","answer","confidence","justification"]}},"ambiguousItems":{"type":"array","items":{"type":"string"}},"caseProblems":{"type":"array","items":{"type":"string"}}},"required":["answers","ambiguousItems","caseProblems"]}"""

let adjudicateSchema =
    """{"type":"object","properties":{"rulings":{"type":"array","items":{"type":"object","properties":{"itemId":{"type":"string"},"ruling":{"type":"string","enum":["key-uniquely-defensible","ambiguous","key-wrong"]},"explanation":{"type":"string"},"fix":{"type":"string"}},"required":["itemId","ruling","explanation","fix"]}}},"required":["rulings"]}"""

let spec (workRoot: string) (label: string) (task: string) (schema: string) (sys: string) =
    let dir = Path.Combine(workRoot, "review", label)
    if Directory.Exists dir then Directory.Delete(dir, true)
    Directory.CreateDirectory dir |> ignore
    File.WriteAllText(Path.Combine(dir, "task.md"), task)
    { Label = label; WorkDir = dir
      Prompt = "Read the file task.md in the current directory. It contains your complete task. Follow it exactly and return the result through the structured output tool. Do not read any other file."
      SystemPrompt = sys; Model = "claude-opus-5-5"; Effort = "high"; Schema = schema
      TranscriptPath = Path.GetFullPath(Path.Combine("benchmark/reviews/raw", label + ".jsonl")); TimeoutMinutes = 40 }

let loadPair (split: string) (id: string) =
    parseCase (readJson (sprintf "benchmark/cases/%s/%s.case.json" split id)),
    parseTruth (readJson (sprintf "benchmark/truth/%s/%s.truth.json" split id)),
    readJson (sprintf "benchmark/truth/%s/%s.truth.json" split id)

let caseIds (split: string) (ids: string list) =
    Directory.GetFiles(sprintf "benchmark/cases/%s" split, "*.case.json")
    |> Array.map (fun p -> Path.GetFileName(p).Replace(".case.json", ""))
    |> Array.sort |> List.ofArray
    |> List.filter (fun id -> ids.IsEmpty || List.contains id ids)

let review (split: string) (workRoot: string) (ids: string list) =
    Directory.CreateDirectory "benchmark/reviews/raw" |> ignore
    let chosen = caseIds split ids
    // stage 1
    let solved =
        chosen
        |> List.map (fun id -> let c, _, _ = loadPair split id in spec workRoot (id + ".solve") (solveTask c) solveSchema "You are a meticulous expert reviewer of diagnostic benchmark cases.")
        |> runAll 6 (fun r -> printfn "solved %s ok=%b %.2f USD" r.Label r.Ok r.CostUsd)
    // stage 2
    let disagreements =
        solved
        |> List.choose (fun r ->
            let id = r.Label.Replace(".solve", "")
            let c, t, _ = loadPair split id
            r.Output |> Option.map (fun o ->
                let blind = arr "answers" o |> List.map (fun a -> strOr "" "itemId" a, ((strOr "" "answer" a).Trim().ToLowerInvariant(), strOr "" "justification" a)) |> Map.ofList
                let diffs = t.Keys |> List.filter (fun k -> match Map.tryFind k.ItemId blind with Some (a, _) -> a <> k.Expected | None -> true)
                id, c, t, o, blind, diffs))
    let adjudications =
        disagreements
        |> List.filter (fun (_, _, _, o, _, diffs) -> not diffs.IsEmpty || not (strList "ambiguousItems" o).IsEmpty)
        |> List.map (fun (id, c, t, o, blind, diffs) ->
            let truthJson = (let _, _, tj = loadPair split id in pretty tj)
            let flagged = (diffs |> List.map (fun k -> k.ItemId)) @ strList "ambiguousItems" o |> List.distinct
            let task =
                String.concat "\n\n"
                    [ "You are adjudicating disagreements between a benchmark case's answer key and an independent expert's blind solution. For each flagged item, rule whether the key's answer is uniquely defensible from the evidence (key-uniquely-defensible), whether two answers are defensible (ambiguous), or whether the key is wrong (key-wrong). If not uniquely defensible, propose the smallest change to the case evidence or the key that would make exactly one answer defensible."
                      semantics
                      "Truth convention: expected answers are what a careful analyst concludes after reading the initial file plus the optimal read path (at most three catalogue items); for INCOMPLETE and NOCONC cases, what the whole catalogue supports."
                      "FLAGGED ITEMS: " + String.Join(", ", flagged)
                      "BLIND SOLUTION (answer and justification) FOR FLAGGED ITEMS:\n" + String.Join("\n", flagged |> List.map (fun i -> match Map.tryFind i blind with Some (a, j) -> sprintf "%s: %s - %s" i a j | None -> sprintf "%s: (no answer)" i))
                      "ANSWER KEY (truth file):\n" + truthJson
                      fullCaseText c ]
            spec workRoot (id + ".adjudicate") task adjudicateSchema "You are an impartial, rigorous adjudicator of benchmark answer keys.")
        |> runAll 6 (fun r -> printfn "adjudicated %s ok=%b %.2f USD" r.Label r.Ok r.CostUsd)
        |> List.map (fun r -> r.Label.Replace(".adjudicate", ""), r)
        |> Map.ofList
    // write review records
    for (id, c, t, o, blind, diffs) in disagreements do
        let rec' = Collections.Generic.Dictionary<string, obj>()
        rec'.["caseId"] <- id
        rec'.["blindAgreement"] <- sprintf "%d/%d" (t.Keys.Length - diffs.Length) t.Keys.Length
        rec'.["weightedBlindAgreement"] <- (let w = t.Keys |> List.sumBy (fun k -> k.Weight) in (t.Keys |> List.filter (fun k -> not (List.contains k diffs)) |> List.sumBy (fun k -> k.Weight)) / w)
        rec'.["disagreements"] <- (diffs |> List.map (fun k -> dict [ "itemId", box k.ItemId; "key", box k.Expected; "blind", box (Map.tryFind k.ItemId blind |> Option.map fst |> Option.defaultValue "") ]))
        rec'.["blindSolution"] <- o
        rec'.["adjudication"] <- (Map.tryFind id adjudications |> Option.bind (fun r -> r.Output) |> Option.map box |> Option.toObj)
        let rulings = Map.tryFind id adjudications |> Option.bind (fun r -> r.Output) |> Option.map (arr "rulings") |> Option.defaultValue []
        let bad = rulings |> List.filter (fun r -> strOr "" "ruling" r <> "key-uniquely-defensible")
        let structural = validate c t
        rec'.["structuralProblems"] <- structural
        rec'.["needsRevision"] <- not bad.IsEmpty || not structural.IsEmpty
        Directory.CreateDirectory "benchmark/reviews" |> ignore
        File.WriteAllText(sprintf "benchmark/reviews/%s.review.json" id, JsonSerializer.Serialize(rec', JsonSerializerOptions(WriteIndented = true)))
        printfn "%s blind agreement %d/%d; rulings needing revision: %d" id (t.Keys.Length - diffs.Length) t.Keys.Length bad.Length
        bad |> List.iter (fun r -> printfn "   %s %s: %s" (strOr "" "itemId" r) (strOr "" "ruling" r) (let s = strOr "" "explanation" r in if s.Length > 160 then s.Substring(0, 160) else s))

let revise (split: string) (workRoot: string) (ids: string list) =
    let assignments = [ "benchmark/authoring/assignments.json"; "benchmark/authoring/assignments-amendment-01.json" ] |> List.filter File.Exists |> List.collect (fun p -> readJson p |> arr split) |> List.map (fun a -> strOr "" "id" a, a) |> Map.ofList
    let contract = File.ReadAllText "benchmark/authoring/authoring-contract.md"
    let chosen =
        caseIds split ids
        |> List.filter (fun id ->
            let p = sprintf "benchmark/reviews/%s.review.json" id
            File.Exists p && (readJson p |> prop "needsRevision" |> Option.exists (fun v -> v.ValueKind = JsonValueKind.True)))
    chosen
    |> List.map (fun id ->
        let a = Map.find id assignments
        let reviewJson = File.ReadAllText(sprintf "benchmark/reviews/%s.review.json" id)
        let caseJson = File.ReadAllText(sprintf "benchmark/cases/%s/%s.case.json" split id)
        let truthJson = File.ReadAllText(sprintf "benchmark/truth/%s/%s.truth.json" split id)
        let task =
            String.concat "\n\n"
                [ "You are revising a benchmark case after independent review. Keep everything that works. Change the smallest amount of evidence or key needed so that every flagged item has exactly one defensible answer. Keep the case's class, difficulty and trap. Return the full corrected case and truth through the structured output tool, in the same format."
                  "AUTHORING CONTRACT (unchanged):\n" + contract.Replace("{{ID}}", id).Replace("{{SPLIT}}", split).Replace("{{CLASS}}", strOr "" "class" a).Replace("{{CLASS_DEFINITION}}", Map.find (strOr "" "class" a) classDefinitions).Replace("{{DOMAIN}}", strOr "" "domain" a).Replace("{{SETTING}}", strOr "" "setting" a)
                  "CURRENT CASE:\n" + caseJson
                  "CURRENT TRUTH:\n" + truthJson
                  "REVIEW (blind solution and adjudication):\n" + reviewJson ]
        spec workRoot (id + ".revise") task """{"type":"object","properties":{"case":{"type":"object"},"truth":{"type":"object"}},"required":["case","truth"]}""" "You are an expert author of fair, difficult diagnostic benchmark cases.")
    |> runAll 6 (fun r ->
        let id = r.Label.Replace(".revise", "")
        match r.Output |> Option.bind (prop "case"), r.Output |> Option.bind (prop "truth") with
        | Some c, Some t ->
            File.Copy(sprintf "benchmark/cases/%s/%s.case.json" split id, sprintf "benchmark/reviews/raw/%s.case.pre-revision.json" id, true)
            File.Copy(sprintf "benchmark/truth/%s/%s.truth.json" split id, sprintf "benchmark/reviews/raw/%s.truth.pre-revision.json" id, true)
            File.WriteAllText(sprintf "benchmark/cases/%s/%s.case.json" split id, pretty c)
            File.WriteAllText(sprintf "benchmark/truth/%s/%s.truth.json" split id, pretty t)
            printfn "revised %s (%.2f USD)" id r.CostUsd
        | _ -> printfn "REVISION FAILED %s: %s" id (r.Error |> Option.defaultValue "missing case/truth"))
    |> ignore

match fsi.CommandLineArgs |> Array.toList |> List.tail with
| "review" :: split :: workRoot :: ids -> review split workRoot ids
| "revise" :: split :: workRoot :: ids -> revise split workRoot ids
| _ ->
    eprintfn "usage: review|revise <split> <workRoot> [caseIds]"
    exit 2
