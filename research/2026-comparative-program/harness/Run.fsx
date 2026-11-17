// OD2 analyzer execution.
// Usage (from research/2026-comparative-program):
//   dotnet fsi harness/Run.fsx run <plan.json> <workRoot> <rawDir> [concurrency]
// A plan is {"phase", "seed", "runs": [{"condition","caseId","trial","model","effort"}]}.
// Each run gets an isolated directory holding only task.md and evidence/X*.txt,
// with catalogue labels shuffled per run. Runs execute in a seeded random order.
// Already-recorded runs are skipped, so an interrupted phase can be resumed.

#load "Claude.fsx"
#load "Domain.fsx"

open System
open System.IO
open System.Text
open System.Text.Json
open Claude
open Domain

let shared = File.ReadAllText "prompts/SHARED.txt"
let moduleText (cond: string) = File.ReadAllText(sprintf "prompts/%s.module.txt" cond)

let outputSchema =
    """{"type":"object","properties":{"caseId":{"type":"string"},"answers":{"type":"array","items":{"type":"object","properties":{"itemId":{"type":"string"},"answer":{"type":"string"},"confidence":{"type":"integer","minimum":0,"maximum":100},"evidenceIds":{"type":"array","items":{"type":"string"}}},"required":["itemId","answer","confidence","evidenceIds"]}},"brief":{"type":"string"},"methodChoice":{"type":"string"}},"required":["caseId","answers","brief"]}"""

let systemPrompt = "You are a careful, independent diagnostic analyst. You follow the task instructions exactly."

let instruction (budget: int) =
    "This is an isolated, controlled diagnostic task. Use the Read tool on task.md in the current directory: it contains your complete task. "
    + sprintf "After that you may read only the evidence files that task.md lists, at most %d of them, and nothing else. " budget
    + "Follow the task exactly and return your answer through the structured output tool."

/// Stable 32-bit hash for seeding (string.GetHashCode is randomized per process).
let stableHash (s: string) =
    s |> Seq.fold (fun (h: uint32) ch -> (h ^^^ uint32 ch) * 16777619u) 2166136261u |> int

/// Seeded Fisher-Yates shuffle (pure given the seed).
let shuffle (seed: int) (xs: 'a list) =
    let a = Array.ofList xs
    let rng = Random(seed)
    for i in a.Length - 1 .. -1 .. 1 do
        let j = rng.Next(i + 1)
        let t = a.[i]
        a.[i] <- a.[j]
        a.[j] <- t
    List.ofArray a

/// label (X1..Xn) -> catalogue id, shuffled per run.
let labelMap (runId: string) (c: Case) =
    c.Catalogue |> List.map (fun e -> e.Id) |> shuffle (stableHash runId) |> List.mapi (fun i id -> sprintf "X%d" (i + 1), id)

let renderCase (c: Case) (labels: (string * string) list) =
    let byId = c.Catalogue |> List.map (fun e -> e.Id, e) |> Map.ofList
    let sb = StringBuilder()
    let line (s: string) = sb.AppendLine s |> ignore
    line (sprintf "CASE %s: %s" c.Id c.Title)
    line ""
    line ("Setting: " + c.Setting)
    line ""
    line ("Presenting request: " + c.Request)
    line ""
    line "INITIAL EVIDENCE"
    for e in c.Initial do
        line (sprintf "%s [stage %s] %s (%s): %s" e.Id e.Stage e.Source e.SourceType e.Text)
    line ""
    line (sprintf "EVIDENCE CATALOGUE (collect at most %d; read the file path to collect)" c.ReadBudget)
    for (label, id) in labels do
        let e = Map.find id byId
        line (sprintf "%s [stage %s] %s - source: %s (%s) - file: evidence/%s.txt" label e.Stage e.Title e.Source e.SourceType label)
    line ""
    line "CANDIDATE HYPOTHESES"
    for (id, t) in c.Hypotheses do line (sprintf "%s: %s" id t)
    line ""
    line "CANDIDATE CAUSAL LINKS"
    for (id, f, t, rel) in c.Edges do line (sprintf "%s: %s -> %s (%s)" id f t rel)
    line ""
    line "CANDIDATE ACTIONS"
    for (id, t) in c.Actions do line (sprintf "%s: %s" id t)
    line ""
    line "SCORED ITEMS (answer every one)"
    for i in c.Items do
        let stage = match i.AsOfStage with Some s -> sprintf " asOfStage=%s" s | None -> ""
        line (sprintf "%s: %s %s%s allowed=[%s]" i.ItemId i.Kind i.RefId stage (String.Join(", ", i.Allowed)))
    sb.ToString()

let renderEvidence (label: string) (e: Evidence) =
    sprintf "%s [stage %s]\nTitle: %s\nSource: %s (%s)\n\n%s\n" label e.Stage e.Title e.Source e.SourceType e.Text

type Planned = { Condition: string; CaseId: string; Trial: int; Model: string; Effort: string }

let tier (model: string) =
    if model.Contains "haiku" then "haiku" elif model.Contains "opus" then "opus" else "sonnet"

let runId (phase: string) (p: Planned) =
    sprintf "%s__%s__%s__%s-%s__t%d" phase p.Condition p.CaseId (tier p.Model) p.Effort p.Trial

let prepare (workRoot: string) (rawDir: string) (phase: string) (cases: Map<string, Case>) (p: Planned) : RunSpec * (string * string) list =
    let id = runId phase p
    let c = Map.find p.CaseId cases
    let labels = labelMap id c
    let dir = Path.Combine(workRoot, "runs", id)
    if Directory.Exists dir then Directory.Delete(dir, true)
    Directory.CreateDirectory(Path.Combine(dir, "evidence")) |> ignore
    let task = shared.Replace("{{BUDGET}}", string c.ReadBudget) + "\n\n" + moduleText p.Condition + "\n\n" + renderCase c labels
    File.WriteAllText(Path.Combine(dir, "task.md"), task)
    let byId = c.Catalogue |> List.map (fun e -> e.Id, e) |> Map.ofList
    for (label, cid) in labels do
        File.WriteAllText(Path.Combine(dir, "evidence", label + ".txt"), renderEvidence label (Map.find cid byId))
    { Label = id; WorkDir = dir; Prompt = instruction c.ReadBudget; SystemPrompt = systemPrompt; Model = p.Model; Effort = p.Effort
      Schema = outputSchema; TranscriptPath = Path.GetFullPath(Path.Combine(rawDir, "transcripts", id + ".jsonl")); TimeoutMinutes = 30 },
    labels

/// Classify Read calls: evidence label reads, the task read, and anything else (a protocol violation).
let classifyReads (workDir: string) (labels: (string * string) list) (calls: ToolCall list) =
    let labelSet = labels |> List.map fst |> Set.ofList
    let path (input: string) =
        try JsonDocument.Parse(input).RootElement |> str "file_path" |> Option.defaultValue "" with _ -> ""
    let reads = calls |> List.filter (fun t -> t.Name = "Read") |> List.map (fun t -> path t.Input)
    let classify (p: string) =
        let name = Path.GetFileName p
        let inDir = (try Path.GetFullPath(p, workDir).StartsWith(workDir) with _ -> false)
        if inDir && name = "task.md" then Choice1Of3 ()
        elif inDir && name.EndsWith ".txt" && labelSet.Contains(name.Replace(".txt", "")) then Choice2Of3 (name.Replace(".txt", ""))
        else Choice3Of3 p
    let classified = reads |> List.map classify
    let evidenceLabels = classified |> List.choose (function Choice2Of3 l -> Some l | _ -> None)
    let invalid = classified |> List.choose (function Choice3Of3 p -> Some p | _ -> None)
    let others = calls |> List.filter (fun t -> t.Name <> "Read" && t.Name <> "StructuredOutput") |> List.map (fun t -> t.Name)
    evidenceLabels, invalid @ others

let record (rawDir: string) (phase: string) (p: Planned) (labels: (string * string) list) (r: RunResult) (workDir: string) =
    let readLabels, violations = classifyReads workDir labels r.ToolCalls
    let toId = labels |> Map.ofList
    let o = Collections.Generic.Dictionary<string, obj>()
    o.["runId"] <- r.Label
    o.["phase"] <- phase
    o.["condition"] <- p.Condition
    o.["caseId"] <- p.CaseId
    o.["trial"] <- p.Trial
    o.["model"] <- p.Model
    o.["effort"] <- p.Effort
    o.["labelMap"] <- (labels |> List.map (fun (l, i) -> l, box i) |> dict)
    o.["readLabels"] <- readLabels
    o.["reads"] <- (readLabels |> List.map (fun l -> Map.find l toId))
    o.["violations"] <- violations
    o.["run"] <- JsonDocument.Parse(resultToJson r).RootElement
    File.WriteAllText(Path.Combine(rawDir, r.Label + ".json"), JsonSerializer.Serialize(o, JsonSerializerOptions(WriteIndented = true)))

let loadCases (dirs: string list) =
    dirs
    |> List.collect (fun d -> Directory.GetFiles(d, "*.case.json") |> List.ofArray)
    |> List.map (readJson >> parseCase)
    |> List.map (fun c -> c.Id, c)
    |> Map.ofList

match fsi.CommandLineArgs |> Array.toList |> List.tail with
| "run" :: planPath :: workRoot :: rawDir :: rest ->
    let concurrency = match rest with n :: _ -> int n | [] -> 8
    let plan = readJson planPath
    let phase = strOr "" "phase" plan
    let seed = num "seed" plan |> Option.map int |> Option.defaultValue 20261117
    let planned =
        arr "runs" plan
        |> List.map (fun r ->
            { Condition = strOr "" "condition" r; CaseId = strOr "" "caseId" r
              Trial = num "trial" r |> Option.map int |> Option.defaultValue 1
              Model = strOr "claude-sonnet-5-5" "model" r; Effort = strOr "medium" "effort" r })
    let cases = loadCases ([ "benchmark/cases/dev"; "benchmark/cases/dev2"; "benchmark/cases/heldout" ] |> List.filter Directory.Exists)
    Directory.CreateDirectory(Path.Combine(rawDir, "transcripts")) |> ignore
    let pending =
        planned
        |> shuffle seed
        |> List.filter (fun p -> not (File.Exists(Path.Combine(rawDir, runId phase p + ".json"))))
    printfn "phase %s: %d planned, %d pending" phase planned.Length pending.Length
    let prepared = pending |> List.map (fun p -> p, prepare (Path.GetFullPath workRoot) rawDir phase cases p)
    let lookup = prepared |> List.map (fun (p, (s, l)) -> s.Label, (p, s, l)) |> Map.ofList
    let counter = ref 0
    let onDone (r: RunResult) =
        let (p, s, l) = Map.find r.Label lookup
        record rawDir phase p l r s.WorkDir
        lock counter (fun () ->
            counter.Value <- counter.Value + 1
            printfn "[%d/%d] %s ok=%b reads=%d cost=%.3f %s" counter.Value pending.Length r.Label r.Ok
                (r.ToolCalls |> List.filter (fun t -> t.Name = "Read") |> List.length) r.CostUsd (r.Error |> Option.defaultValue ""))
    let results = prepared |> List.map (fun (_, (s, _)) -> s) |> runAll concurrency onDone
    printfn "phase %s finished: %d ok, %d failed, %.2f USD" phase (results |> List.filter (fun r -> r.Ok) |> List.length)
        (results |> List.filter (fun r -> not r.Ok) |> List.length) (results |> List.sumBy (fun r -> r.CostUsd))
| _ ->
    eprintfn "usage: run <plan.json> <workRoot> <rawDir> [concurrency]"
    exit 2
