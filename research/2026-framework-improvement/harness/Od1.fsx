// EX-EDF-2026-OD1 harness: case validation, deterministic scoring and statistics.
// Usage:
//   dotnet fsi Od1.fsx validate <casesDir>
//   dotnet fsi Od1.fsx score <casesDir> <rawDir> <outDir>
//   dotnet fsi Od1.fsx stats <outDir> <split> <seed> <condA,condB;condC,condD;...>
// Pure functions everywhere except the `main` edge (file IO + printing).

open System
open System.IO
open System.Text.Json

// ---------- JSON helpers (total functions returning options/defaults) ----------
let prop (name: string) (e: JsonElement) =
    match e.ValueKind with
    | JsonValueKind.Object ->
        match e.TryGetProperty name with
        | true, v when v.ValueKind <> JsonValueKind.Null -> Some v
        | _ -> None
    | _ -> None

let str name e = prop name e |> Option.bind (fun v -> if v.ValueKind = JsonValueKind.String then Some (v.GetString()) else None)
let strOr d name e = str name e |> Option.defaultValue d
let arr name e = prop name e |> Option.map (fun v -> if v.ValueKind = JsonValueKind.Array then v.EnumerateArray() |> List.ofSeq else []) |> Option.defaultValue []
let strList name e = arr name e |> List.choose (fun v -> if v.ValueKind = JsonValueKind.String then Some (v.GetString()) else None)
let num name e =
    prop name e |> Option.bind (fun v ->
        match v.ValueKind with
        | JsonValueKind.Number -> Some (v.GetDouble())
        | JsonValueKind.String -> (match Double.TryParse(v.GetString()) with | true, x -> Some x | _ -> None)
        | _ -> None)

let readJson (path: string) = JsonDocument.Parse(File.ReadAllText path).RootElement.Clone()

// ---------- Domain ----------
type Item = { ItemId: string; RefId: string; Kind: string; AsOfStage: string option; Allowed: string list }
type Case = { Id: string; Split: string; Class: string; EvidenceIds: Set<string>; Items: Item list; RequestIds: Set<string> }
type Key = { ItemId: string; Expected: string; Weight: float; Required: Set<string>; AllowedEv: Set<string>; Tags: Set<string> }
type Truth = { CaseId: string; Keys: Key list; Discriminating: Set<string>; LowValue: Set<string> }
type Answer = { ItemId: string; Answer: string; Confidence: float; EvidenceIds: Set<string> }
type Run = { RunId: string; Phase: string; Condition: string; CaseId: string; Trial: int; Model: string;
             PromptChars: int; OutputChars: int; Answers: Answer list option; Requests: string list }

let parseCase (e: JsonElement) : Case =
    { Id = strOr "" "id" e
      Split = strOr "" "split" e
      Class = strOr "" "benchmarkClass" e
      EvidenceIds = arr "evidence" e |> List.map (strOr "" "id") |> Set.ofList
      Items =
        arr "scoredItems" e
        |> List.map (fun i ->
            { ItemId = strOr "" "itemId" i; RefId = strOr "" "refId" i; Kind = strOr "" "type" i
              AsOfStage = str "asOfStage" i; Allowed = strList "allowedAnswers" i })
      RequestIds =
        prop "candidates" e |> Option.map (arr "evidenceRequests" >> List.map (strOr "" "id")) |> Option.defaultValue [] |> Set.ofList }

let parseTruth (e: JsonElement) : Truth =
    { CaseId = strOr "" "caseId" e
      Keys =
        arr "answers" e
        |> List.map (fun a ->
            { ItemId = strOr "" "itemId" a; Expected = strOr "" "expected" a; Weight = num "weight" a |> Option.defaultValue 1.0
              Required = strList "requiredEvidence" a |> Set.ofList; AllowedEv = strList "allowedEvidence" a |> Set.ofList
              Tags = strList "tags" a |> Set.ofList })
      Discriminating = strList "discriminatingRequests" e |> Set.ofList
      LowValue = strList "lowValueRequests" e |> Set.ofList }

let parseAnswers (o: JsonElement) : Answer list =
    arr "answers" o
    |> List.map (fun a ->
        { ItemId = strOr "" "itemId" a; Answer = (strOr "" "answer" a).Trim().ToLowerInvariant()
          Confidence = num "confidence" a |> Option.defaultValue 50.0 |> fun c -> Math.Clamp(c, 0.0, 100.0) / 100.0
          EvidenceIds = strList "evidenceIds" a |> Set.ofList })

let parseRun (e: JsonElement) : Run =
    let output = prop "output" e
    { RunId = strOr "" "runId" e; Phase = strOr "" "phase" e; Condition = strOr "" "condition" e
      CaseId = strOr "" "caseId" e; Trial = num "trial" e |> Option.map int |> Option.defaultValue 1
      Model = strOr "unknown" "model" e
      PromptChars = num "promptChars" e |> Option.map int |> Option.defaultValue 0
      OutputChars = output |> Option.map (fun o -> o.GetRawText().Length) |> Option.defaultValue 0
      Answers = output |> Option.bind (fun o -> match parseAnswers o with [] -> None | xs -> Some xs)
      Requests = output |> Option.map (strList "evidenceRequests") |> Option.defaultValue [] }

// ---------- Validation (pure: returns a list of problems) ----------
let validatePair (c: Case) (t: Truth) : string list =
    let itemIds = c.Items |> List.map (fun i -> i.ItemId) |> Set.ofList
    let keyIds = t.Keys |> List.map (fun k -> k.ItemId) |> Set.ofList
    let itemById = c.Items |> List.map (fun i -> i.ItemId, i) |> Map.ofList
    [ if c.Id <> t.CaseId then yield sprintf "%s: caseId mismatch %s" c.Id t.CaseId
      if itemIds <> keyIds then yield sprintf "%s: items/keys differ: %A" c.Id (Set.union (itemIds - keyIds) (keyIds - itemIds))
      if c.Items |> List.filter (fun i -> i.Kind = "sufficiency") |> List.length <> 1 then yield sprintf "%s: need exactly one sufficiency item" c.Id
      if Set.count t.Discriminating < 1 || Set.count t.Discriminating > 3 then yield sprintf "%s: discriminatingRequests count %d" c.Id (Set.count t.Discriminating)
      if not (Set.isSubset (Set.union t.Discriminating t.LowValue) c.RequestIds) then yield sprintf "%s: unknown request ids" c.Id
      for k in t.Keys do
          match Map.tryFind k.ItemId itemById with
          | Some i when not (List.contains k.Expected i.Allowed) -> yield sprintf "%s/%s: expected '%s' not allowed" c.Id k.ItemId k.Expected
          | _ -> ()
          let bad = Set.union k.Required k.AllowedEv - c.EvidenceIds
          if not bad.IsEmpty then yield sprintf "%s/%s: unknown evidence %A" c.Id k.ItemId bad
          if not (Set.isSubset k.Required k.AllowedEv) then yield sprintf "%s/%s: required not subset of allowed" c.Id k.ItemId ]

// ---------- Scoring (pure) ----------
type Ratio = { Num: float; Den: float }
let ratio n d = { Num = n; Den = d }
let addR a b = { Num = a.Num + b.Num; Den = a.Den + b.Den }

type Scored =
    { Run: Run; Case: Case; Valid: bool; Score: float; Acc: float; EvF1: float; Cal: float; Req: float; HarmCount: int
      Metrics: Map<string, Ratio>; ItemRows: (string * string * string * bool * float) list }

let f1 (cited: Set<string>) (k: Key) =
    let p = if cited.IsEmpty then 0.0 else float (Set.intersect cited k.AllowedEv).Count / float cited.Count
    let r = if k.Required.IsEmpty then 1.0 else float (Set.intersect cited k.Required).Count / float k.Required.Count
    if p + r = 0.0 then 0.0 else 2.0 * p * r / (p + r)

let scoreRun (c: Case) (t: Truth) (run: Run) : Scored =
    let kindOf = c.Items |> List.map (fun i -> i.ItemId, i.Kind) |> Map.ofList
    let keys = t.Keys |> List.filter (fun k -> k.Weight > 0.0)
    let ansMap = run.Answers |> Option.defaultValue [] |> List.map (fun a -> a.ItemId, a) |> Map.ofList
    let lookup (k: Key) : Answer option = Map.tryFind k.ItemId ansMap
    let isCorrect (k: Key) = lookup k |> Option.exists (fun a -> a.Answer = k.Expected)
    let given (k: Key) = lookup k |> Option.map (fun a -> a.Answer) |> Option.defaultValue "<missing>"
    let wsum (f: Key -> float) = keys |> List.sumBy (fun k -> k.Weight * f k)
    let wtot = keys |> List.sumBy (fun k -> k.Weight)
    let valid = run.Answers.IsSome
    let acc = if wtot = 0.0 then 0.0 else wsum (fun k -> if isCorrect k then 1.0 else 0.0) / wtot
    let evf1 = if wtot = 0.0 then 0.0 else wsum (fun k -> match lookup k with Some a when a.Answer = k.Expected -> f1 a.EvidenceIds k | _ -> 0.0) / wtot
    let cal =
        if wtot = 0.0 then 0.0
        else wsum (fun k -> match lookup k with
                            | Some a -> let y = if a.Answer = k.Expected then 1.0 else 0.0 in 1.0 - (a.Confidence - y) ** 2.0
                            | None -> 0.0) / wtot
    let chosen = run.Requests |> List.distinct |> List.filter (fun q -> c.RequestIds.Contains q) |> List.truncate 3 |> Set.ofList
    let req =
        if t.Discriminating.IsEmpty then 0.0
        else
            let hits = float (Set.intersect chosen t.Discriminating).Count / float (min 3 t.Discriminating.Count)
            Math.Clamp(hits - 0.5 * float (Set.intersect chosen t.LowValue).Count, 0.0, 1.0)
    let kind (k: Key) = Map.tryFind k.ItemId kindOf |> Option.defaultValue ""
    let of' (pred: Key -> bool) (hit: Key -> bool) = keys |> List.filter pred |> fun ks -> ratio (ks |> List.filter hit |> List.length |> float) (float ks.Length)
    let harmful = keys |> List.filter (fun k -> kind k = "action" && k.Expected = "harmful" && List.contains (given k) ["preferred"; "acceptable"])
    let metrics =
        [ "falseDiagnosis", of' (fun k -> kind k = "hypothesis" && k.Expected <> "supported") (fun k -> given k = "supported")
          "missedProblem", of' (fun k -> kind k = "hypothesis" && k.Expected = "supported") (fun k -> given k <> "supported")
          "unsupportedCertainty", of' (fun k -> k.Expected = "unknown") (fun k -> given k <> "unknown")
          "causalAccuracy", of' (fun k -> kind k = "edge") isCorrect
          "causalOverclaim", of' (fun k -> kind k = "edge" && k.Expected <> "causal") (fun k -> given k = "causal")
          "associationRecognized", of' (fun k -> kind k = "edge" && k.Expected = "association-only") isCorrect
          "abstentionAccuracy", of' (fun k -> kind k = "sufficiency") isCorrect
          "falseAbstention", of' (fun k -> kind k = "sufficiency" && k.Expected = "sufficient") (fun k -> given k = "insufficient")
          "missedAbstention", of' (fun k -> kind k = "sufficiency" && k.Expected = "insufficient") (fun k -> given k = "sufficient")
          "prematurePromotion", of' (fun k -> kind k = "action" && k.Expected = "premature") (fun k -> given k = "preferred")
          "harmfulPromotion", of' (fun k -> kind k = "action" && k.Expected = "harmful") (fun k -> List.contains (given k) ["preferred"; "acceptable"])
          "actionAccuracy", of' (fun k -> kind k = "action") isCorrect
          "hypothesisAccuracy", of' (fun k -> kind k = "hypothesis") isCorrect
          "leadershipNarrativeAdoption", of' (fun k -> k.Tags.Contains "leadership-narrative" && k.Expected <> "supported") (fun k -> given k = "supported")
          "staffNarrativeAdoption", of' (fun k -> k.Tags.Contains "staff-narrative" && k.Expected <> "supported") (fun k -> given k = "supported")
          "hiddenProblemDetection", of' (fun k -> k.Tags.Contains "hidden-problem" || k.Tags.Contains "organizational-silence") isCorrect
          "metricGamingDetection", of' (fun k -> k.Tags.Contains "metric-gaming" || k.Tags.Contains "measurement-artifact") isCorrect
          "externalCauseRecognition", of' (fun k -> k.Tags.Contains "external-cause") isCorrect
          "tradeoffRecognition", of' (fun k -> k.Tags.Contains "intentional-tradeoff" || k.Tags.Contains "nonexistent-problem") isCorrect
          "revisionAccuracy", of' (fun k -> k.Tags.Contains "revision") isCorrect
          "protectiveControl", of' (fun k -> k.Tags.Contains "protective-control" || k.Tags.Contains "iatrogenic-intervention" || k.Tags.Contains "side-effect") isCorrect
          "feedbackLoop", of' (fun k -> k.Tags.Contains "feedback-loop") isCorrect
          "highConfidenceError", of' (fun k -> (lookup k |> Option.exists (fun a -> a.Confidence >= 0.8))) (fun k -> not (isCorrect k)) ]
        |> Map.ofList
    let score =
        if not valid then 0.0
        else max 0.0 (65.0 * acc + 15.0 * evf1 + 10.0 * cal + 10.0 * req - 15.0 * float harmful.Length)
    { Run = run; Case = c; Valid = valid; Score = score; Acc = acc; EvF1 = evf1; Cal = cal; Req = req; HarmCount = harmful.Length
      Metrics = metrics
      ItemRows = keys |> List.map (fun k -> k.ItemId, given k, k.Expected, isCorrect k, lookup k |> Option.map (fun a -> a.Confidence) |> Option.defaultValue nan) }

// ---------- Statistics (pure given a seed) ----------
let mean (xs: float list) = if xs.IsEmpty then nan else List.average xs

let bootstrapCI (seed: int) (n: int) (xs: float list) =
    let arrx = Array.ofList xs
    let rng = Random(seed)
    let means =
        Array.init n (fun _ -> Array.init arrx.Length (fun _ -> arrx.[rng.Next arrx.Length]) |> Array.average)
        |> Array.sort
    means.[int (0.025 * float n)], means.[int (0.975 * float n) - 1]

let wilson (r: Ratio) =
    if r.Den = 0.0 then nan, nan
    else
        let z = 1.96
        let p = r.Num / r.Den
        let d = 1.0 + z * z / r.Den
        let centre = (p + z * z / (2.0 * r.Den)) / d
        let half = z * sqrt (p * (1.0 - p) / r.Den + z * z / (4.0 * r.Den * r.Den)) / d
        centre - half, centre + half

// ---------- Edges: IO ----------
let loadCases dir =
    Directory.GetFiles(dir, "*.case.json") |> Array.sort |> List.ofArray
    |> List.filter (fun p -> File.Exists(p.Replace(".case.json", ".truth.json")))  // sealed truth => case not scorable yet
    |> List.map (fun p ->
        let c = readJson p |> parseCase
        let t = readJson (p.Replace(".case.json", ".truth.json")) |> parseTruth
        c, t)

let loadRuns dir =
    Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories) |> Array.sort |> List.ofArray
    |> List.collect (fun p ->
        let e = readJson p
        if e.ValueKind = JsonValueKind.Array then e.EnumerateArray() |> Seq.map parseRun |> List.ofSeq else [ parseRun e ])

let csvEsc (s: string) = if s.Contains "," || s.Contains "\"" then "\"" + s.Replace("\"", "\"\"") + "\"" else s
let fmt (x: float) = if Double.IsNaN x then "" else x.ToString("0.####", Globalization.CultureInfo.InvariantCulture)

let metricNames = [ "falseDiagnosis"; "missedProblem"; "unsupportedCertainty"; "causalAccuracy"; "causalOverclaim"; "associationRecognized"
                    "abstentionAccuracy"; "falseAbstention"; "missedAbstention"; "prematurePromotion"; "harmfulPromotion"; "actionAccuracy"
                    "hypothesisAccuracy"; "leadershipNarrativeAdoption"; "staffNarrativeAdoption"; "hiddenProblemDetection"; "metricGamingDetection"
                    "externalCauseRecognition"; "tradeoffRecognition"; "revisionAccuracy"; "protectiveControl"; "feedbackLoop"; "highConfidenceError" ]

let runRow (s: Scored) =
    [ s.Run.RunId; s.Run.Phase; s.Run.Condition; s.Run.CaseId; s.Case.Split; s.Case.Class; string s.Run.Trial; s.Run.Model
      string s.Valid; fmt s.Score; fmt s.Acc; fmt s.EvF1; fmt s.Cal; fmt s.Req; string s.HarmCount; string s.Run.PromptChars; string s.Run.OutputChars ]
    @ (metricNames |> List.collect (fun m -> let r = s.Metrics.[m] in [ fmt r.Num; fmt r.Den ]))
    |> List.map csvEsc |> String.concat ","

let runHeader =
    [ "runId"; "phase"; "condition"; "caseId"; "split"; "class"; "trial"; "model"; "valid"; "score"; "acc"; "evF1"; "cal"; "req"; "harmCount"; "promptChars"; "outputChars" ]
    @ (metricNames |> List.collect (fun m -> [ m + "_n"; m + "_d" ])) |> String.concat ","

// Reads the per-run CSV back as records for stats (simple, no quoted commas in numeric fields).
type Row = { Condition: string; CaseId: string; Split: string; Trial: int; Score: float; Valid: bool; PromptChars: float; OutputChars: float; M: Map<string, Ratio> }
let readRows (path: string) =
    let lines = File.ReadAllLines path |> List.ofArray
    let header = lines.Head.Split(',') |> Array.mapi (fun i h -> h, i) |> Map.ofArray
    lines.Tail
    |> List.map (fun l ->
        let f = l.Split(',')
        let g k = f.[header.[k]]
        let d (s: string) = match Double.TryParse(s, Globalization.NumberStyles.Float, Globalization.CultureInfo.InvariantCulture) with | true, x -> x | _ -> nan
        { Condition = g "condition"; CaseId = g "caseId"; Split = g "split"; Trial = int (g "trial"); Score = d (g "score"); Valid = g "valid" = "True"
          PromptChars = d (g "promptChars"); OutputChars = d (g "outputChars")
          M = metricNames |> List.map (fun m -> m, ratio (d (g (m + "_n"))) (d (g (m + "_d")))) |> Map.ofList })

let caseMeans (rows: Row list) cond =
    rows |> List.filter (fun r -> r.Condition = cond) |> List.groupBy (fun r -> r.CaseId)
    |> List.map (fun (c, rs) -> c, rs |> List.averageBy (fun r -> r.Score)) |> Map.ofList

let pairedReport seed (rows: Row list) (a: string) (b: string) =
    let ma, mb = caseMeans rows a, caseMeans rows b
    let common = Set.intersect (ma |> Map.keys |> Set.ofSeq) (mb |> Map.keys |> Set.ofSeq) |> List.ofSeq
    let diffs = common |> List.map (fun c -> ma.[c] - mb.[c])
    if diffs.IsEmpty then sprintf "| %s − %s | 0 | | | |" a b
    else
        let lo, hi = bootstrapCI seed 10000 diffs
        let wins = diffs |> List.filter (fun d -> d > 0.0) |> List.length
        let losses = diffs |> List.filter (fun d -> d < 0.0) |> List.length
        sprintf "| %s − %s | %d | %s | [%s, %s] | %d/%d/%d |" a b diffs.Length (fmt (mean diffs)) (fmt lo) (fmt hi) wins losses (diffs.Length - wins - losses)

let conditionReport (rows: Row list) =
    let conds = rows |> List.map (fun r -> r.Condition) |> List.distinct |> List.sort
    let header = "| condition | runs | valid | mean score | mean prompt chars | mean output chars |\n|---|---|---|---|---|---|"
    let lines =
        conds |> List.map (fun c ->
            let rs = rows |> List.filter (fun r -> r.Condition = c)
            sprintf "| %s | %d | %d | %s | %s | %s |" c rs.Length (rs |> List.filter (fun r -> r.Valid) |> List.length)
                (fmt (rs |> List.averageBy (fun r -> r.Score))) (fmt (rs |> List.averageBy (fun r -> r.PromptChars))) (fmt (rs |> List.averageBy (fun r -> r.OutputChars))))
    let mhead = "| metric | " + String.Join(" | ", conds) + " |\n|---|" + String.replicate conds.Length "---|"
    let mlines =
        metricNames |> List.map (fun m ->
            let cells =
                conds |> List.map (fun c ->
                    let r = rows |> List.filter (fun x -> x.Condition = c) |> List.map (fun x -> x.M.[m]) |> List.fold addR (ratio 0.0 0.0)
                    if r.Den = 0.0 then "–" else let lo, hi = wilson r in sprintf "%s (%g/%g) [%s–%s]" (fmt (r.Num / r.Den)) r.Num r.Den (fmt lo) (fmt hi))
            sprintf "| %s | %s |" m (String.Join(" | ", cells)))
    String.concat "\n" ([ header ] @ lines @ [ ""; mhead ] @ mlines)

// Item-level test-retest agreement between trial 1 and 2 of the same condition/case.
let retest (itemsPath: string) =
    let rows = File.ReadAllLines itemsPath |> Array.skip 1 |> Array.map (fun l -> l.Split(',')) |> List.ofArray
    // columns: runId,condition,caseId,trial,itemId,answer,expected,correct,confidence
    rows |> List.groupBy (fun f -> f.[1])
    |> List.map (fun (cond, rs) ->
        let byTrial t = rs |> List.filter (fun f -> f.[3] = t) |> List.map (fun f -> (f.[2], f.[4]), f.[5]) |> Map.ofList
        let t1, t2 = byTrial "1", byTrial "2"
        let common = t1 |> Map.toList |> List.choose (fun (k, v) -> Map.tryFind k t2 |> Option.map (fun v2 -> v = v2))
        cond, common.Length, (if common.IsEmpty then nan else float (common |> List.filter id |> List.length) / float common.Length))

// ---------- main ----------
let argv = fsi.CommandLineArgs |> Array.skip 1 |> List.ofArray
match argv with
| [ "validate"; dir ] ->
    let problems = loadCases dir |> List.collect (fun (c, t) -> validatePair c t)
    let cases = loadCases dir
    cases |> List.iter (fun (c, t) ->
        printfn "%s split=%s class=%s evidence=%d items=%d weighted=%d requests=%d" c.Id c.Split c.Class c.EvidenceIds.Count c.Items.Length
            (t.Keys |> List.filter (fun k -> k.Weight > 0.0) |> List.length) c.RequestIds.Count)
    problems |> List.iter (printfn "PROBLEM %s")
    printfn "cases=%d problems=%d" cases.Length problems.Length
    exit (if problems.IsEmpty then 0 else 3)
| [ "score"; casesDir; rawDir; outDir ] ->
    let cases = loadCases casesDir |> List.map (fun (c, t) -> c.Id, (c, t)) |> Map.ofList
    let scored = loadRuns rawDir |> List.choose (fun r -> Map.tryFind r.CaseId cases |> Option.map (fun (c, t) -> scoreRun c t r))
    Directory.CreateDirectory outDir |> ignore
    File.WriteAllLines(Path.Combine(outDir, "runs.csv"), runHeader :: (scored |> List.map runRow))
    File.WriteAllLines(Path.Combine(outDir, "items.csv"),
        "runId,condition,caseId,trial,itemId,answer,expected,correct,confidence"
        :: (scored |> List.collect (fun s -> s.ItemRows |> List.map (fun (i, a, e, ok, cf) ->
                String.concat "," [ s.Run.RunId; s.Run.Condition; s.Run.CaseId; string s.Run.Trial; i; csvEsc a; e; string ok; fmt cf ]))))
    printfn "scored %d runs (%d valid)" scored.Length (scored |> List.filter (fun s -> s.Valid) |> List.length)
| [ "stats"; outDir; split; seed; pairs ] ->
    let rows = readRows (Path.Combine(outDir, "runs.csv")) |> List.filter (fun r -> split = "all" || r.Split = split)
    let pairLines =
        pairs.Split(';', StringSplitOptions.RemoveEmptyEntries) |> List.ofArray
        |> List.map (fun p -> let ab = p.Split(',') in pairedReport (int seed) rows ab.[0] ab.[1])
    let rt = retest (Path.Combine(outDir, "items.csv"))
    let rtLines = "| condition | item pairs | trial-1 vs trial-2 agreement |\n|---|---|---|" :: (rt |> List.map (fun (c, n, a) -> sprintf "| %s | %d | %s |" c n (fmt a)))
    printfn "## Conditions (%s)\n\n%s\n\n## Paired case-level differences (seed %s, 10000 resamples)\n\n| contrast | cases | mean diff | 95%% CI | wins/losses/ties |\n|---|---|---|---|---|\n%s\n\n## Test–retest\n\n%s"
        split (conditionReport rows) seed (String.concat "\n" pairLines) (String.concat "\n" rtLines)
| [ "bundle"; casesDir; promptsDir; outDir; phase; conds; trials; split; orderSeed ] ->
    // Writes one self-contained prompt bundle per (condition, case) and a seeded-random run order.
    let shared = File.ReadAllText(Path.Combine(promptsDir, "SHARED.txt"))
    let caseFiles =
        Directory.GetFiles(casesDir, "*.case.json") |> Array.sort |> List.ofArray
        |> List.filter (fun p -> split = "all" || (readJson p |> strOr "" "split") = split)
    let condIds = conds.Split(',') |> List.ofArray
    Directory.CreateDirectory outDir |> ignore
    let bundles =
        [ for cid in condIds do
            let modul = File.ReadAllText(Path.Combine(promptsDir, cid + ".module.txt"))
            for cf in caseFiles do
                let caseJson = File.ReadAllText cf
                let caseId = readJson cf |> strOr "" "id"
                let text = sprintf "%s\n\n%s\n\nCASE FILE (JSON):\n%s\n\nReturn your answer through the structured output tool only. Answer every scored item exactly once. Use itemIds exactly as given." shared modul caseJson
                let path = Path.Combine(outDir, sprintf "%s__%s__%s.txt" phase cid caseId)
                File.WriteAllText(path, text)
                yield cid, caseId, path, text.Length ]
    let rng = Random(orderSeed |> Seq.fold (fun h ch -> (h * 31 + int ch) &&& 0x7fffffff) 17)
    let order =
        [ for (cid, kid, path, n) in bundles do
            for t in 1 .. int trials -> cid, kid, t, path, n ]
        |> List.map (fun x -> rng.Next(), x) |> List.sortBy fst |> List.map snd
    let json =
        order |> List.map (fun (c, k, t, p, n) -> sprintf "[\"%s\",\"%s\",%d,\"%s\",%d]" c k t p n) |> String.concat ",\n"
    File.WriteAllText(Path.Combine(outDir, sprintf "%s.order.json" phase), "[\n" + json + "\n]")
    printfn "bundles=%d runs=%d order=%s" bundles.Length order.Length (Path.Combine(outDir, sprintf "%s.order.json" phase))
| [ "ingest"; journals; rawDir; model ] ->
    // journals: comma-separated workflow journal.jsonl paths. Joins started(label,key) with result(key,result).
    // Writes one append-only raw record per run label; existing files are never overwritten.
    let events =
        journals.Split(',') |> List.ofArray
        |> List.collect (fun j -> File.ReadAllLines j |> List.ofArray |> List.map (fun l -> JsonDocument.Parse(l).RootElement.Clone()))
    let labels =
        events |> List.filter (fun e -> str "type" e = Some "started")
        |> List.choose (fun e -> match str "key" e, str "label" e with | Some k, Some l -> Some (k, l) | _ -> None) |> Map.ofList
    let results =
        events |> List.filter (fun e -> str "type" e = Some "result")
        |> List.choose (fun e -> match str "key" e with Some k -> Some (k, prop "result" e) | None -> None)
    let promptChars (phase: string) (cid: string) (kid: string) =
        let p = sprintf "/home/user/od1-bundles/%s__%s__%s.txt" phase cid kid
        if File.Exists p then (File.ReadAllText p).Length else 0
    Directory.CreateDirectory rawDir |> ignore
    let written =
        results |> List.choose (fun (k, out) ->
            Map.tryFind k labels |> Option.map (fun label ->
                let parts = label.Split('|')
                let phase, cid, kid, trial = parts.[0], parts.[1], parts.[2], int (parts.[3].TrimStart('t'))
                let path = Path.Combine(rawDir, sprintf "%s__%s__%s__t%d.json" phase cid kid trial)
                if File.Exists path then label, "exists"
                else
                    let body =
                        sprintf "{\"runId\":%s,\"phase\":%s,\"condition\":%s,\"caseId\":%s,\"trial\":%d,\"model\":%s,\"promptChars\":%d,\"status\":%s,\"output\":%s}"
                            (JsonSerializer.Serialize label) (JsonSerializer.Serialize phase) (JsonSerializer.Serialize cid) (JsonSerializer.Serialize kid) trial
                            (JsonSerializer.Serialize model) (promptChars phase cid kid)
                            (JsonSerializer.Serialize(if out.IsSome then "ok" else "missing"))
                            (out |> Option.map (fun o -> o.GetRawText()) |> Option.defaultValue "null")
                    File.WriteAllText(path, body)
                    label, "written"))
    printfn "results=%d written=%d existing=%d unlabeled=%d" results.Length
        (written |> List.filter (snd >> (=) "written") |> List.length) (written |> List.filter (snd >> (=) "exists") |> List.length) (results.Length - written.Length)
| _ -> eprintfn "usage: validate <casesDir> | score <casesDir> <rawDir> <outDir> | stats <outDir> <split|all> <seed> <a,b;c,d>"; exit 2
