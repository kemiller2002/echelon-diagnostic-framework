// OD2 deterministic scoring and statistics.
// Usage (from research/2026-comparative-program):
//   dotnet fsi harness/Score.fsx score <rawDir> <outDir> [voteSpec ...]
//   dotnet fsi harness/Score.fsx stats <outDir> <seed> <contrasts>
// voteSpec "NEW=SOURCE:k" adds a self-consistency condition NEW formed by majority vote
// over the first k trials of SOURCE (same case, model and effort); its cost is the sum.
// contrasts: "X,Y;Z,W" gives paired case-level differences X - Y (same model tier and effort
// unless a side is written COND@tier-effort).
// Truth is read from benchmark/truth/<split>/. Pure scoring; IO only at the edges.

#load "Domain.fsx"

open System
open System.IO
open System.Text.Json
open Domain

let loadMap dir pattern parse key =
    if Directory.Exists dir then Directory.GetFiles(dir, pattern) |> Array.map (readJson >> parse) |> Array.map (fun x -> key x, x) |> Map.ofArray
    else Map.empty

let cases =
    [ "dev"; "dev2"; "heldout" ] |> List.map (fun s -> loadMap (sprintf "benchmark/cases/%s" s) "*.case.json" parseCase (fun (c: Case) -> c.Id))
    |> List.fold (fun acc m -> Map.fold (fun a k v -> Map.add k v a) acc m) Map.empty
let truths =
    [ "dev"; "dev2"; "heldout" ] |> List.map (fun s -> loadMap (sprintf "benchmark/truth/%s" s) "*.truth.json" parseTruth (fun (t: Truth) -> t.CaseId))
    |> List.fold (fun acc m -> Map.fold (fun a k v -> Map.add k v a) acc m) Map.empty

let tierOf (m: string) = if m.Contains "haiku" then "haiku" elif m.Contains "opus" then "opus" else "sonnet"

let parseRecord (e: JsonElement) : Run * string list =
    let run = prop "run" e
    let rnum name = run |> Option.bind (num name) |> Option.defaultValue 0.0
    let output = run |> Option.bind (prop "output")
    let labelMap = prop "labelMap" e |> Option.map (fun m -> m.EnumerateObject() |> Seq.map (fun p -> p.Name, p.Value.GetString()) |> Map.ofSeq) |> Option.defaultValue Map.empty
    let mapId (id: string) = Map.tryFind (id.Trim()) labelMap |> Option.defaultValue (id.Trim())
    let ok = run |> Option.bind (prop "ok") |> Option.exists (fun v -> v.ValueKind = JsonValueKind.True)
    let answers =
        if not ok then None
        else
            output
            |> Option.map (parseAnswers >> List.map (fun a -> { a with EvidenceIds = a.EvidenceIds |> Set.map mapId }))
            |> Option.bind (fun xs -> if xs.IsEmpty then None else Some xs)
    { RunId = strOr "" "runId" e; Condition = strOr "" "condition" e; CaseId = strOr "" "caseId" e
      Trial = num "trial" e |> Option.map int |> Option.defaultValue 1; Model = strOr "" "model" e; Effort = strOr "" "effort" e
      Answers = answers; Reads = strList "reads" e; InvalidReads = (strList "violations" e).Length
      Brief = output |> Option.map (strOr "" "brief") |> Option.defaultValue ""
      InputTokens = int64 (rnum "inputTokens"); OutputTokens = int64 (rnum "outputTokens"); ThinkingTokens = int64 (rnum "thinkingTokens")
      CacheReadTokens = int64 (rnum "cacheReadTokens"); CacheCreationTokens = int64 (rnum "cacheCreationTokens")
      CostUsd = rnum "costUsd"; DurationMs = int64 (rnum "durationMs") },
    strList "violations" e

/// Self-consistency: majority answer per item over k runs; confidence = vote share;
/// evidence = union of evidence cited by the majority; reads = union; cost and tokens summed.
let vote (name: string) (runs: Run list) : Run =
    let valid = runs |> List.choose (fun r -> r.Answers)
    let first = List.head runs
    let byItem = valid |> List.concat |> List.groupBy (fun a -> a.ItemId)
    let answers =
        byItem
        |> List.map (fun (itemId, xs) ->
            let ranked = xs |> List.countBy (fun a -> a.Answer) |> List.sortByDescending (fun (ans, n) -> n, -(xs |> List.findIndex (fun a -> a.Answer = ans)))
            let (winner, n) = List.head ranked
            { ItemId = itemId; Answer = winner; Confidence = float n / float runs.Length
              EvidenceIds = xs |> List.filter (fun a -> a.Answer = winner) |> List.map (fun a -> a.EvidenceIds) |> Set.unionMany })
    { first with
        RunId = first.RunId.Replace("__" + first.Condition + "__", "__" + name + "__")
        Condition = name; Trial = 1
        Answers = (if answers.IsEmpty then None else Some answers)
        Reads = runs |> List.collect (fun r -> r.Reads) |> List.distinct
        InputTokens = runs |> List.sumBy (fun r -> r.InputTokens); OutputTokens = runs |> List.sumBy (fun r -> r.OutputTokens)
        ThinkingTokens = runs |> List.sumBy (fun r -> r.ThinkingTokens); CacheReadTokens = runs |> List.sumBy (fun r -> r.CacheReadTokens)
        CacheCreationTokens = runs |> List.sumBy (fun r -> r.CacheCreationTokens)
        CostUsd = runs |> List.sumBy (fun r -> r.CostUsd); DurationMs = runs |> List.sumBy (fun r -> r.DurationMs) }

let csvEscape (s: string) = "\"" + s.Replace("\"", "\"\"") + "\""

let metricNames =
    [ "hypothesisAccuracy"; "falseDiagnosis"; "missedProblem"; "unsupportedCertainty"; "overAbstention"; "causalAccuracy"
      "causalOverclaim"; "abstentionAccuracy"; "missedAbstention"; "falseAbstention"; "actionAccuracy"; "prematurePromotion"
      "harmfulPromotion"; "highConfidenceError"; "discriminatingRecall"; "lowValueReadShare"; "budgetBreach" ]

let scoreAll (rawDir: string) (votes: string list) =
    let records = Directory.GetFiles(rawDir, "*.json") |> Array.sort |> Array.map (readJson >> parseRecord) |> List.ofArray
    let runs = records |> List.map fst
    let voted =
        votes
        |> List.collect (fun spec ->
            let name, rest = let i = spec.IndexOf '=' in spec.Substring(0, i), spec.Substring(i + 1)
            let source, k = let i = rest.IndexOf ':' in rest.Substring(0, i), int (rest.Substring(i + 1))
            runs
            |> List.filter (fun r -> r.Condition = source)
            |> List.groupBy (fun r -> r.CaseId, r.Model, r.Effort)
            |> List.choose (fun (_, rs) ->
                let chosen = rs |> List.sortBy (fun r -> r.Trial) |> List.truncate k
                if chosen.Length = k then Some (vote name chosen) else None))
    (runs @ voted)
    |> List.choose (fun r ->
        match Map.tryFind r.CaseId cases, Map.tryFind r.CaseId truths with
        | Some c, Some t -> Some (scoreRun c t r)
        | _ -> None)

let writeOutputs (outDir: string) (scored: Scored list) =
    Directory.CreateDirectory outDir |> ignore
    let header =
        [ "runId"; "condition"; "caseId"; "split"; "class"; "domain"; "trial"; "tier"; "effort"; "valid"; "score"; "acc"; "evf1"; "cal"; "acq"
          "harm"; "breach"; "reads"; "invalidReads"; "inputTokens"; "cacheReadTokens"; "cacheCreationTokens"; "outputTokens"; "thinkingTokens"; "costUsd"; "durationMs" ]
        @ (metricNames |> List.collect (fun m -> [ m + "_num"; m + "_den" ]))
    let row (s: Scored) =
        let r = s.Run
        [ r.RunId; r.Condition; r.CaseId; s.Case.Split; s.Case.Class; s.Case.Domain; string r.Trial; tierOf r.Model; r.Effort
          string s.Valid; sprintf "%.4f" s.Score; sprintf "%.4f" s.Acc; sprintf "%.4f" s.EvF1; sprintf "%.4f" s.Cal; sprintf "%.4f" s.Acq
          string s.HarmCount; string s.Breach; string (List.distinct r.Reads).Length; string r.InvalidReads
          string r.InputTokens; string r.CacheReadTokens; string r.CacheCreationTokens; string r.OutputTokens; string r.ThinkingTokens
          sprintf "%.6f" r.CostUsd; string r.DurationMs ]
        @ (metricNames |> List.collect (fun m -> let v = Map.find m s.Metrics in [ string v.Num; string v.Den ]))
    File.WriteAllLines(Path.Combine(outDir, "runs.csv"), (String.Join(",", header)) :: (scored |> List.map (row >> List.map csvEscape >> fun xs -> String.Join(",", xs))))
    let items =
        scored
        |> List.collect (fun s ->
            s.ItemRows |> List.map (fun (item, given, expected, ok, conf) ->
                [ s.Run.RunId; s.Run.Condition; s.Run.CaseId; tierOf s.Run.Model; s.Run.Effort; string s.Run.Trial; item; given; expected; string ok
                  (if Double.IsNaN conf then "" else sprintf "%.2f" conf) ] |> List.map csvEscape |> fun xs -> String.Join(",", xs)))
    File.WriteAllLines(Path.Combine(outDir, "items.csv"), "runId,condition,caseId,tier,effort,trial,itemId,given,expected,correct,confidence" :: items)
    let briefs =
        scored |> List.map (fun s -> [ s.Run.RunId; s.Run.Condition; s.Run.CaseId; tierOf s.Run.Model; s.Run.Effort; string s.Run.Trial; sprintf "%.4f" s.Acc; s.Run.Brief ] |> List.map csvEscape |> fun xs -> String.Join(",", xs))
    File.WriteAllLines(Path.Combine(outDir, "briefs.csv"), "runId,condition,caseId,tier,effort,trial,acc,brief" :: briefs)

// ---------- statistics over runs.csv ----------
let readCsv (path: string) =
    let parseLine (line: string) =
        let rec go (i: int) (inQ: bool) (cur: Text.StringBuilder) (acc: string list) =
            if i >= line.Length then List.rev (cur.ToString() :: acc)
            else
                let ch = line.[i]
                match inQ, ch with
                | true, '"' when i + 1 < line.Length && line.[i + 1] = '"' -> go (i + 2) true (cur.Append '"') acc
                | true, '"' -> go (i + 1) false cur acc
                | true, _ -> go (i + 1) true (cur.Append ch) acc
                | false, '"' -> go (i + 1) true cur acc
                | false, ',' -> go (i + 1) false (Text.StringBuilder()) (cur.ToString() :: acc)
                | false, _ -> go (i + 1) false (cur.Append ch) acc
        go 0 false (Text.StringBuilder()) []
    let lines = File.ReadAllLines path |> List.ofArray
    let header = parseLine (List.head lines)
    lines |> List.tail |> List.map (fun l -> List.zip header (parseLine l) |> Map.ofList)

type Row = Map<string, string>
let f (k: string) (r: Row) = Double.Parse(Map.find k r, Globalization.CultureInfo.InvariantCulture)
let cell (r: Row) = sprintf "%s@%s-%s" (Map.find "condition" r) (Map.find "tier" r) (Map.find "effort" r)

/// Case-level mean of a measure for one cell (condition@tier-effort), averaging trials.
let caseMeans (rows: Row list) (cellName: string) (measure: Row -> float) =
    rows |> List.filter (fun r -> cell r = cellName) |> List.groupBy (Map.find "caseId") |> List.map (fun (c, rs) -> c, rs |> List.averageBy measure) |> Map.ofList

let resolveCell (defaultTierEffort: string) (s: string) = if s.Contains "@" then s else s + "@" + defaultTierEffort

let stats (outDir: string) (seed: int) (contrasts: string) =
    let rows = readCsv (Path.Combine(outDir, "runs.csv"))
    let cells = rows |> List.map cell |> List.distinct |> List.sort
    let sb = Text.StringBuilder()
    let line (s: string) = sb.AppendLine s |> ignore
    line "# OD2 statistics"
    line ""
    line (sprintf "Runs: %d. Bootstrap: seed %d, 10,000 resamples over cases (trials averaged within case)." rows.Length seed)
    line ""
    line "## Condition summary"
    line ""
    line "| cell | runs | valid | cases | score | acc | acq | cal | reads | breaches | invalid reads | harm | thinking tok | output tok | cost USD | time s |"
    line "|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|"
    for c in cells do
        let rs = rows |> List.filter (fun r -> cell r = c)
        let m k = rs |> List.averageBy (f k)
        let cm = caseMeans rows c (f "score") |> Map.toList |> List.map snd
        let lo, hi = bootstrapCI seed 10000 cm
        line (sprintf "| %s | %d | %d | %d | %.1f [%.1f, %.1f] | %.3f | %.3f | %.3f | %.2f | %d | %d | %d | %.0f | %.0f | %.4f | %.0f |"
                c rs.Length (rs |> List.filter (fun r -> Map.find "valid" r = "True") |> List.length) cm.Length (List.average cm) lo hi
                (m "acc") (m "acq") (m "cal") (m "reads")
                (rs |> List.filter (fun r -> Map.find "breach" r = "True") |> List.length)
                (rs |> List.sumBy (fun r -> int (f "invalidReads" r)))
                (rs |> List.sumBy (fun r -> int (f "harm" r))) (m "thinkingTokens") (m "outputTokens") (m "costUsd") (m "durationMs" / 1000.0))
    line ""
    line "## Secondary rates (pooled ratio; 95% Wilson interval)"
    line ""
    line ("| metric | " + String.Join(" | ", cells) + " |")
    line ("|---|" + String.replicate cells.Length "---|")
    for m in metricNames do
        let cellsTxt =
            cells |> List.map (fun c ->
                let rs = rows |> List.filter (fun r -> cell r = c)
                let r = { Num = rs |> List.sumBy (f (m + "_num")); Den = rs |> List.sumBy (f (m + "_den")) }
                if r.Den = 0.0 then "-" else let lo, hi = wilson r in sprintf "%.3f (%g/%g) [%.3f-%.3f]" (r.Num / r.Den) r.Num r.Den lo hi)
        line (sprintf "| %s | %s |" m (String.Join(" | ", cellsTxt)))
    line ""
    line "## Paired case-level contrasts"
    line ""
    line "| contrast | measure | cases | mean diff | 95% CI | wins/losses/ties |"
    line "|---|---|---|---|---|---|"
    let defaultTE = "sonnet-medium"
    for pair in contrasts.Split(';', StringSplitOptions.RemoveEmptyEntries) do
        let parts = pair.Split(',')
        let a, b = resolveCell defaultTE parts.[0], resolveCell defaultTE parts.[1]
        for (mname, measure) in [ "score", f "score"; "acc", f "acc"; "acq", f "acq"; "reads", f "reads"; "costUsd", f "costUsd" ] do
            let ma, mb = caseMeans rows a measure, caseMeans rows b measure
            let common = Set.intersect (ma |> Map.keys |> Set.ofSeq) (mb |> Map.keys |> Set.ofSeq) |> Set.toList
            if not common.IsEmpty then
                let d = common |> List.map (fun k -> ma.[k] - mb.[k])
                let lo, hi = bootstrapCI seed 10000 d
                let w = d |> List.filter (fun x -> x > 1e-9) |> List.length
                let l = d |> List.filter (fun x -> x < -1e-9) |> List.length
                line (sprintf "| %s − %s | %s | %d | %.3f | [%.3f, %.3f] | %d/%d/%d |" a b mname common.Length (List.average d) lo hi w l (d.Length - w - l))
    line ""
    line "## Test–retest (item agreement between trials 1 and 2)"
    line ""
    let items = readCsv (Path.Combine(outDir, "items.csv"))
    line "| cell | item pairs | agreement |"
    line "|---|---|---|"
    for c in cells do
        let cond, te = let i = c.IndexOf '@' in c.Substring(0, i), c.Substring(i + 1)
        let mine = items |> List.filter (fun r -> Map.find "condition" r = cond && (Map.find "tier" r + "-" + Map.find "effort" r) = te)
        let byKey t = mine |> List.filter (fun r -> Map.find "trial" r = t) |> List.map (fun r -> (Map.find "caseId" r, Map.find "itemId" r), Map.find "given" r) |> Map.ofList
        let t1, t2 = byKey "1", byKey "2"
        let common = Set.intersect (t1 |> Map.keys |> Set.ofSeq) (t2 |> Map.keys |> Set.ofSeq)
        if not common.IsEmpty then
            let agree = common |> Set.filter (fun k -> t1.[k] = t2.[k]) |> Set.count
            line (sprintf "| %s | %d | %.3f |" c common.Count (float agree / float common.Count))
    File.WriteAllText(Path.Combine(outDir, "stats.md"), sb.ToString())
    printfn "%s" (sb.ToString())

match fsi.CommandLineArgs |> Array.toList |> List.tail with
| "score" :: rawDir :: outDir :: votes ->
    let scored = scoreAll rawDir votes
    writeOutputs outDir scored
    printfn "scored %d runs (%d valid)" scored.Length (scored |> List.filter (fun s -> s.Valid) |> List.length)
| [ "stats"; outDir; seed; contrasts ] -> stats outDir (int seed) contrasts
| _ ->
    eprintfn "usage: score <rawDir> <outDir> [NEW=SRC:k ...] | stats <outDir> <seed> <contrasts>"
    exit 2
