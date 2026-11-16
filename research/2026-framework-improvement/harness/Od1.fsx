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
      Requests =
        let raw = output |> Option.map (strList "evidenceRequests") |> Option.defaultValue []
        match prop "requestMap" e with
        | Some m when m.ValueKind = JsonValueKind.Object ->
            raw |> List.map (fun r -> match m.TryGetProperty(r.Trim()) with | true, v -> v.GetString() | _ -> r)
        | _ -> raw }

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
    // Spec §5: calibration is a plain (unweighted) mean over scored items (replication finding R-1, fixed 2026-11-16).
    let cal =
        if keys.IsEmpty then 0.0
        else keys |> List.averageBy (fun k -> match lookup k with
                                              | Some a -> let y = if a.Answer = k.Expected then 1.0 else 0.0 in 1.0 - (a.Confidence - y) ** 2.0
                                              | None -> 0.0)
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

// ---------- Request-menu relabeling (fixes the menu-order leak found by replication) ----------
// Shuffles the evidence-request menu with a seeded RNG and renames ids X1..Xn (X avoids clashing with edge ids R#); returns the new case JSON and a map X->Q.
let relabelRequests (seed: int) (caseText: string) : string * (string * string) list =
    let node = Nodes.JsonNode.Parse(caseText).AsObject()
    let cands = node.["candidates"].AsObject()
    let reqs = cands.["evidenceRequests"].AsArray() |> Seq.map (fun n -> n.ToJsonString()) |> List.ofSeq
    let rng = Random(seed)
    let shuffled = reqs |> List.map (fun r -> rng.Next(), r) |> List.sortBy fst |> List.map snd
    let renamed =
        shuffled |> List.mapi (fun i r ->
            let o = Nodes.JsonNode.Parse(r).AsObject()
            let oldId = o.["id"].GetValue<string>()
            let newId = sprintf "X%d" (i + 1)
            o.["id"] <- Nodes.JsonValue.Create(newId)
            o.ToJsonString(), (newId, oldId))
    cands.["evidenceRequests"] <- Nodes.JsonNode.Parse("[" + String.Join(",", renamed |> List.map fst) + "]")
    node.ToJsonString(JsonSerializerOptions(WriteIndented = true)), renamed |> List.map snd

let seedOf (s: string) = s |> Seq.fold (fun h ch -> (h * 31 + int ch) &&& 0x7fffffff) 17

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
                let caseId = readJson cf |> strOr "" "id"
                let caseJson, rmap = relabelRequests (seedOf (orderSeed + "|" + cid + "|" + caseId)) (File.ReadAllText cf)
                File.WriteAllText(Path.Combine(outDir, sprintf "%s__%s__%s.map.json" phase cid caseId),
                    "{" + String.Join(",", rmap |> List.map (fun (r, q) -> sprintf "\"%s\":\"%s\"" r q)) + "}")
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
    // Keys hash (prompt, options), so identical bundles in different journals share a key:
    // labels are therefore resolved within each journal, never across journals.
    let perJournal (j: string) =
        let events = File.ReadAllLines j |> List.ofArray |> List.map (fun l -> JsonDocument.Parse(l).RootElement.Clone())
        let labels =
            events |> List.filter (fun e -> str "type" e = Some "started")
            |> List.choose (fun e -> match str "key" e, str "label" e with | Some k, Some l -> Some (k, l) | _ -> None)
        let dupKeys = labels |> List.countBy fst |> List.filter (fun (_, n) -> n > 1) |> List.map fst |> Set.ofList
        if not dupKeys.IsEmpty then eprintfn "WARNING %s: %d keys shared by several labels in one journal" j dupKeys.Count
        let labelMap = labels |> Map.ofList
        events |> List.filter (fun e -> str "type" e = Some "result")
        |> List.choose (fun e -> match str "key" e with Some k -> Some (j + "#" + k, prop "result" e) | None -> None),
        labelMap |> Map.toList |> List.map (fun (k, l) -> j + "#" + k, l)
    let parts = journals.Split(',') |> List.ofArray |> List.map perJournal
    let results = parts |> List.collect fst
    let labels = parts |> List.collect snd |> Map.ofList
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
                    let mapPath = sprintf "/home/user/od1-bundles/%s__%s__%s.map.json" phase cid kid
                    let body = if File.Exists mapPath then body.Substring(0, body.Length - 1) + ",\"requestMap\":" + File.ReadAllText mapPath + "}" else body
                    File.WriteAllText(path, body)
                    label, "written"))
    printfn "results=%d written=%d existing=%d unlabeled=%d" results.Length
        (written |> List.filter (snd >> (=) "written") |> List.length) (written |> List.filter (snd >> (=) "exists") |> List.length) (results.Length - written.Length)
| [ "ogbundle"; casesDir; promptsDir; outDir; phase; conds; split ] ->
    // Open-generation bundles: candidates and scored items removed; only organization, request and evidence remain.
    let shared = File.ReadAllText(Path.Combine(promptsDir, "SHARED_OG.txt"))
    let stripped (p: string) =
        let e = readJson p
        let o = Nodes.JsonObject()
        [ "id"; "title"; "organization"; "presentingRequest"; "evidence" ]
        |> List.iter (fun k -> prop k e |> Option.iter (fun v -> o.[k] <- Nodes.JsonNode.Parse(v.GetRawText())))
        o.ToJsonString(JsonSerializerOptions(WriteIndented = true))
    let caseFiles =
        Directory.GetFiles(casesDir, "*.case.json") |> Array.sort |> List.ofArray
        |> List.filter (fun p -> split = "all" || (readJson p |> strOr "" "split") = split)
    let made =
        [ for cid in conds.Split(',') do
            let modul = File.ReadAllText(Path.Combine(promptsDir, cid + ".module.txt"))
            for cf in caseFiles do
                let kid = readJson cf |> strOr "" "id"
                let text = sprintf "%s\n\n%s\n\nCASE FILE (JSON):\n%s\n\nReturn your answer through the structured output tool only." shared modul (stripped cf)
                File.WriteAllText(Path.Combine(outDir, sprintf "%s__%s__%s.txt" phase cid kid), text)
                yield sprintf "%s|%s|%s|1" phase cid kid ]
    printfn "%s" (JsonSerializer.Serialize made)
| [ "gradebundle"; casesDir; rawDir; outDir ] ->
    // Grader bundles: the analyst's open output plus the frozen candidate items. No condition name, no expected answers.
    Directory.CreateDirectory outDir |> ignore
    let finalStage (c: JsonElement) = arr "evidence" c |> List.map (strOr "S1" "stage") |> List.max
    let made =
        Directory.GetFiles(rawDir, "*.json") |> Array.sort |> List.ofArray
        |> List.choose (fun p ->
            let r = readJson p
            prop "output" r |> Option.map (fun out ->
                let kid = strOr "" "caseId" r
                let c = readJson (Path.Combine(casesDir, kid + ".case.json"))
                let fs = finalStage c
                let cands = prop "candidates" c |> Option.get
                let textOf (refId: string) =
                    [ "hypotheses"; "edges"; "actions" ]
                    |> List.tryPick (fun k ->
                        arr k cands |> List.tryFind (fun x -> strOr "" "id" x = refId)
                        |> Option.map (fun x ->
                            match str "text" x with
                            | Some t -> t
                            | None -> sprintf "%s -> %s (%s)" (strOr "" "from" x) (strOr "" "to" x) (strOr "" "relation" x)))
                    |> Option.defaultValue (if refId = "SUFF" then "Is current evidence sufficient to commit to a substantive intervention?" else refId)
                let items =
                    arr "scoredItems" c
                    |> List.filter (fun i -> match str "asOfStage" i with Some s -> s = fs | None -> true)
                    |> List.map (fun i -> sprintf "- %s [%s] %s" (strOr "" "itemId" i) (strOr "" "type" i) (textOf (strOr "" "refId" i)))
                let reqs = arr "evidenceRequests" cands |> List.map (fun q -> sprintf "- %s %s" (strOr "" "id" q) (strOr "" "text" q))
                let blindId = Convert.ToHexString(Security.Cryptography.SHA256.HashData(Text.Encoding.UTF8.GetBytes(strOr "" "runId" r))).Substring(0, 12)
                let text =
                    String.concat "\n"
                        [ "You are a careful grader. Read an analyst's diagnosis of an organization and record, for each listed item, what the ANALYST asserted. Do not judge whether the analyst is right; record only what they said or clearly implied. Use only this message."
                          ""
                          "Reading rules:"
                          "- hypothesis items: \"supported\" if the analyst treats it as established; \"contradicted\" if the analyst rejects it; \"unknown\" if the analyst treats it as possible but unestablished; \"not-addressed\" if the analyst does not discuss it or anything equivalent."
                          "- edge items: \"causal\", \"association-only\", \"contradicted\" (analyst rejects the link), \"unknown\", or \"not-addressed\"."
                          "- action items: \"preferred\" if the analyst recommends it (or an equivalent) as a do-now/best action; \"acceptable\" if recommended as reasonable but not best; \"premature\" if the analyst says to wait for evidence before doing it; \"harmful\" if the analyst advises against it; \"not-addressed\" otherwise."
                          "- sufficiency item: the analyst's sufficiency judgement (\"sufficient\" or \"insufficient\")."
                          "- Match on meaning, not wording. A statement equivalent to, or clearly entailing, an item counts as addressing it."
                          "- requestIds: IDs of listed evidence requests that match the analyst's nextEvidence (up to three; empty if none match)."
                          ""
                          sprintf "Grading id: %s (case %s)" blindId kid
                          ""
                          "ITEMS:"
                          String.concat "\n" items
                          ""
                          "EVIDENCE REQUESTS:"
                          String.concat "\n" reqs
                          ""
                          "ANALYST OUTPUT (JSON):"
                          out.GetRawText() ]
                File.WriteAllText(Path.Combine(outDir, sprintf "grade__%s__%s.txt" blindId kid), text)
                sprintf "%s|%s|%s" blindId kid (strOr "" "runId" r)))
    File.WriteAllLines(Path.Combine(outDir, "grade-map.txt"), made)
    printfn "%s" (JsonSerializer.Serialize(made |> List.map (fun m -> let a = m.Split('|') in a.[0] + "|" + a.[1])))
| [ "ogscore"; casesDir; gradeMap; gradeJournals; outDir ] ->
    // Grader readings -> OG score. not-addressed counts wrong for hypotheses, edges, SUFF and preferred actions,
    // and as not-promoted (correct) for actions expected harmful/premature/acceptable.
    let cases = loadCases casesDir |> List.map (fun (c, t) -> c.Id, (c, t)) |> Map.ofList
    let mapping =
        File.ReadAllLines gradeMap
        |> Array.map (fun l -> let a = l.Split('|') in a.[0], (a.[1], String.Join("|", a.[2..]))) |> Map.ofArray
    let events =
        gradeJournals.Split(',') |> List.ofArray
        |> List.collect (fun j -> File.ReadAllLines j |> List.ofArray |> List.map (fun l -> JsonDocument.Parse(l).RootElement.Clone()))
    let labels =
        events |> List.filter (fun e -> str "type" e = Some "started")
        |> List.choose (fun e -> match str "key" e, str "label" e with | Some k, Some l -> Some (k, l) | _ -> None) |> Map.ofList
    let graded =
        events |> List.filter (fun e -> str "type" e = Some "result")
        |> List.choose (fun e ->
            match str "key" e |> Option.bind (fun k -> Map.tryFind k labels), prop "result" e with
            | Some label, Some res -> Some (label, res)
            | _ -> None)
    let scoreOne (grader: string) (runId: string) (c: Case) (t: Truth) (res: JsonElement) =
        let kindOf = c.Items |> List.map (fun i -> i.ItemId, i.Kind) |> Map.ofList
        let kind (k: Key) = Map.tryFind k.ItemId kindOf |> Option.defaultValue ""
        let readings =
            arr "mappings" res
            |> List.map (fun m -> strOr "" "itemId" m, (strOr "not-addressed" "reading" m).Trim().ToLowerInvariant()) |> Map.ofList
        let scorable = t.Keys |> List.filter (fun k -> k.Weight > 0.0 && readings.ContainsKey k.ItemId)
        let rd (k: Key) = readings.[k.ItemId]
        let ok (k: Key) = if kind k = "action" && rd k = "not-addressed" then k.Expected <> "preferred" else rd k = k.Expected
        let wtot = scorable |> List.sumBy (fun k -> k.Weight)
        let acc = if wtot = 0.0 then 0.0 else (scorable |> List.sumBy (fun k -> if ok k then k.Weight else 0.0)) / wtot
        let harm = scorable |> List.filter (fun k -> kind k = "action" && k.Expected = "harmful" && List.contains (rd k) [ "preferred"; "acceptable" ]) |> List.length
        let req = strList "requestIds" res |> List.truncate 3 |> Set.ofList
        let reqScore = if t.Discriminating.IsEmpty then 0.0 else float (Set.intersect req t.Discriminating).Count / float (min 3 t.Discriminating.Count)
        let rate (pred: Key -> bool) (hit: Key -> bool) = scorable |> List.filter pred |> fun ks -> sprintf "%d,%d" (ks |> List.filter hit |> List.length) ks.Length
        let score = max 0.0 (90.0 * acc + 10.0 * reqScore - 15.0 * float harm)
        let row =
            String.concat "," [ runId; grader; runId.Split('|').[1]; c.Id; fmt score; fmt acc; fmt reqScore; string harm
                                rate (fun k -> kind k = "hypothesis" && k.Expected = "supported") (fun k -> rd k <> "supported")
                                rate (fun k -> kind k = "hypothesis" && k.Expected <> "supported") (fun k -> rd k = "supported")
                                rate (fun k -> kind k = "hypothesis") (fun k -> rd k = "not-addressed")
                                rate (fun k -> k.Tags.Contains "leadership-narrative" && k.Expected <> "supported") (fun k -> rd k = "supported")
                                rate (fun k -> kind k = "sufficiency") ok ]
        row, (readings |> Map.toList |> List.map (fun (i, v) -> String.concat "," [ runId; grader; i; v ]))
    let rows =
        graded |> List.choose (fun (label, res) ->
            let parts = label.Split('|')   // grade|<grader>|<blindId>
            Map.tryFind parts.[2] mapping
            |> Option.bind (fun (kid, runId) -> Map.tryFind kid cases |> Option.map (fun (c, t) -> scoreOne parts.[1] runId c t res)))
    Directory.CreateDirectory outDir |> ignore
    File.WriteAllLines(Path.Combine(outDir, "og-runs.csv"),
        "runId,grader,condition,caseId,score,acc,req,harm,missed_n,missed_d,falseDx_n,falseDx_d,notAddressed_n,notAddressed_d,narrative_n,narrative_d,suff_n,suff_d"
        :: (rows |> List.map fst))
    File.WriteAllLines(Path.Combine(outDir, "og-readings.csv"), "runId,grader,itemId,reading" :: (rows |> List.collect snd))
    printfn "graded=%d scored=%d" graded.Length rows.Length
| [ "adapt1"; casesDir; promptsDir; outDir; conds; k; split ] ->
    // Stage 1 of the adaptive-evidence experiment: catalogue only (id, stage, source, sourceType), choose k items.
    let caseFiles =
        Directory.GetFiles(casesDir, "*.case.json") |> Array.sort |> List.ofArray
        |> List.filter (fun p -> split = "all" || (readJson p |> strOr "" "split") = split)
    let made =
        [ for cid in conds.Split(',') do
            let modul = File.ReadAllText(Path.Combine(promptsDir, cid + ".module.txt"))
            for cf in caseFiles do
                let e = readJson cf
                let kid = strOr "" "id" e
                let catalogue =
                    arr "evidence" e
                    |> List.map (fun x -> sprintf "- %s [%s] source: %s (%s)" (strOr "" "id" x) (strOr "" "stage" x) (strOr "" "source" x) (strOr "" "sourceType" x))
                let candidates = prop "candidates" e |> Option.map (fun c -> c.GetRawText()) |> Option.defaultValue "{}"
                let text =
                    String.concat "\n"
                        [ sprintf "You are planning an organizational diagnosis under an evidence budget. You may collect only %s of the evidence items listed below. You can see each item's source but not its content. Use no tools other than returning your answer." k
                          "Choose the items that will most improve your ability to judge the candidate hypotheses, causal links and actions correctly. Prefer items that discriminate between live explanations over items that would only confirm the client's framing."
                          ""
                          "Method guidance (apply as relevant to choosing evidence):"
                          modul
                          ""
                          sprintf "ORGANIZATION: %s" (prop "organization" e |> Option.map (fun o -> o.GetRawText()) |> Option.defaultValue "")
                          sprintf "PRESENTING REQUEST: %s" (strOr "" "presentingRequest" e)
                          ""
                          "EVIDENCE CATALOGUE (content hidden):"
                          String.concat "\n" catalogue
                          ""
                          "CANDIDATES THAT WILL BE JUDGED:"
                          candidates
                          ""
                          sprintf "Return exactly %s distinct evidence IDs in 'selected', most valuable first." k ]
                File.WriteAllText(Path.Combine(outDir, sprintf "adapt1__%s__%s.txt" cid kid), text)
                yield sprintf "adapt1|%s|%s|1" cid kid ]
    printfn "%s" (JsonSerializer.Serialize made)
| [ "adapt2"; casesDir; promptsDir; outDir; selJournals; k; seed ] ->
    // Stage 2: recognition bundles (D_EDFX module) containing only the selected evidence.
    // Selectors: SEL_A_GENERAL / SEL_D_EDFX (from stage-1 journals), FIXED (first k by file order), RANDOM (seeded).
    let kk = int k
    let events =
        selJournals.Split(',') |> List.ofArray
        |> List.collect (fun j ->
            let ev = File.ReadAllLines j |> List.ofArray |> List.map (fun l -> JsonDocument.Parse(l).RootElement.Clone())
            let labels = ev |> List.filter (fun e -> str "type" e = Some "started") |> List.choose (fun e -> match str "key" e, str "label" e with | Some a, Some b -> Some (a, b) | _ -> None) |> Map.ofList
            ev |> List.filter (fun e -> str "type" e = Some "result")
            |> List.choose (fun e -> match str "key" e |> Option.bind (fun x -> Map.tryFind x labels), prop "result" e with
                                     | Some l, Some r -> Some (l, strList "selected" r) | _ -> None))
    let selections = events |> List.map (fun (l, s) -> let p = l.Split('|') in (p.[1], p.[2]), s) |> Map.ofList
    let shared = File.ReadAllText(Path.Combine(promptsDir, "SHARED.txt"))
    let modul = File.ReadAllText(Path.Combine(promptsDir, "D_EDFX.module.txt"))
    let seedInt = seed |> Seq.fold (fun h ch -> (h * 31 + int ch) &&& 0x7fffffff) 17
    let made =
        [ for (kid, cf) in (Directory.GetFiles(casesDir, "*.case.json") |> Array.sort |> Array.map (fun p -> (readJson p |> strOr "" "id"), p) |> List.ofArray) do
            let e = readJson cf
            let ids = arr "evidence" e |> List.map (strOr "" "id")
            let rnd = Random(seedInt + (kid |> Seq.sumBy int))
            let strategies =
                [ "FIXED", ids |> List.truncate kk
                  "RANDOM", ids |> List.map (fun i -> rnd.Next(), i) |> List.sortBy fst |> List.map snd |> List.truncate kk ]
                @ ([ "A_GENERAL"; "D_EDFX" ] |> List.choose (fun s -> Map.tryFind (s, kid) selections |> Option.map (fun sel -> "SEL_" + s, sel |> List.filter (fun i -> List.contains i ids) |> List.distinct |> List.truncate kk)))
            for (sname, chosen) in strategies do
                let o = Nodes.JsonNode.Parse(e.GetRawText()).AsObject()
                let kept = arr "evidence" e |> List.filter (fun x -> List.contains (strOr "" "id" x) chosen)
                o.["evidence"] <- Nodes.JsonNode.Parse("[" + String.Join(",", kept |> List.map (fun x -> x.GetRawText())) + "]")
                let note = sprintf "Evidence budget: only %d evidence items were collected for this case; other evidence exists but was not collected. Items whose evidence is not in the file must be judged on what was collected." chosen.Length
                let text = sprintf "%s\n\n%s\n\n%s\n\nCASE FILE (JSON):\n%s\n\nReturn your answer through the structured output tool only. Answer every scored item exactly once. Use itemIds exactly as given." shared modul note (o.ToJsonString(JsonSerializerOptions(WriteIndented = true)))
                File.WriteAllText(Path.Combine(outDir, sprintf "adapt2__%s__%s.txt" sname kid), text)
                yield sprintf "adapt2|%s|%s|1" sname kid, String.Join(" ", chosen) ]
    File.WriteAllLines(Path.Combine(outDir, "adapt2-selections.txt"), made |> List.map (fun (a, b) -> a + "|" + b))
    printfn "%s" (JsonSerializer.Serialize(made |> List.map fst))
| [ "seqbundle"; casesDir; promptsDir; bundleDir; evidenceRoot; conds; split; seed ] ->
    // Sequential-acquisition bundles. conds like "A_GENERAL:FREE,D_EDFX:K6". One evidence file per item under
    // evidenceRoot/<case>/<id>.txt; the task file holds request, candidates (relabeled menu), scored items and a catalogue.
    let shared = File.ReadAllText(Path.Combine(promptsDir, "SHARED.txt"))
    let caseFiles =
        Directory.GetFiles(casesDir, "*.case.json") |> Array.sort |> List.ofArray
        |> List.filter (fun p -> split = "all" || (readJson p |> strOr "" "split") = split)
    let made =
        [ for spec in conds.Split(',') do
            let cid, mode = let a = spec.Split(':') in a.[0], a.[1]
            let modul = File.ReadAllText(Path.Combine(promptsDir, cid + ".module.txt"))
            let seqText = File.ReadAllText(Path.Combine(promptsDir, (if mode = "K6" then "SEQ_K6.txt" else "SEQ_FREE.txt")))
            let condId = if mode = "K6" then cid + "_K6" else cid
            for cf in caseFiles do
                let e = readJson cf
                let kid = strOr "" "id" e
                let evDir = Path.Combine(evidenceRoot, kid)
                Directory.CreateDirectory evDir |> ignore
                arr "evidence" e |> List.iter (fun x -> File.WriteAllText(Path.Combine(evDir, strOr "" "id" x + ".txt"), x.GetRawText()))
                let relabeled, rmap = relabelRequests (seedOf (seed + "|" + condId + "|" + kid)) (File.ReadAllText cf)
                File.WriteAllText(Path.Combine(bundleDir, sprintf "seq__%s__%s.map.json" condId kid),
                    "{" + String.Join(",", rmap |> List.map (fun (r, q) -> sprintf "\"%s\":\"%s\"" r q)) + "}")
                let header = Nodes.JsonNode.Parse(relabeled).AsObject()
                header.Remove("evidence") |> ignore
                let catalogue =
                    arr "evidence" e
                    |> List.map (fun x -> sprintf "- %s [%s] source: %s (%s) -> file %s" (strOr "" "id" x) (strOr "" "stage" x) (strOr "" "source" x) (strOr "" "sourceType" x) (Path.Combine(evDir, strOr "" "id" x + ".txt")))
                let text =
                    sprintf "%s\n\n%s\n\n%s\n\nCASE FILE WITHOUT EVIDENCE CONTENT (JSON):\n%s\n\nEVIDENCE CATALOGUE:\n%s\n\nWhen you have finished collecting, return your answer through the structured output tool only. Answer every scored item exactly once. Use itemIds exactly as given; cite only evidence IDs you collected."
                        shared modul seqText (header.ToJsonString(JsonSerializerOptions(WriteIndented = true))) (String.concat "\n" catalogue)
                File.WriteAllText(Path.Combine(bundleDir, sprintf "seq__%s__%s.txt" condId kid), text)
                yield sprintf "seq|%s|%s|1" condId kid ]
    printfn "%s" (JsonSerializer.Serialize made)
| [ "seqreads"; journals; evidenceRoot; outCsv ] ->
    // Counts evidence files read per run from workflow agent transcripts (agentId from journal 'started' lines).
    let rows =
        journals.Split(',') |> List.ofArray
        |> List.collect (fun j ->
            let dir = Path.GetDirectoryName j
            File.ReadAllLines j |> List.ofArray |> List.map (fun l -> JsonDocument.Parse(l).RootElement.Clone())
            |> List.filter (fun e -> str "type" e = Some "started")
            |> List.choose (fun e ->
                match str "label" e, str "agentId" e with
                | Some label, Some aid ->
                    let tp = Path.Combine(dir, sprintf "agent-%s.jsonl" aid)
                    if not (File.Exists tp) then None
                    else
                        let txt = File.ReadAllText tp
                        let reads =
                            Text.RegularExpressions.Regex.Matches(txt, "\"name\":\\s*\"Read\",\\s*\"input\":\\s*\\{\"file_path\":\\s*\"([^\"]+)\"")
                            |> Seq.map (fun m -> m.Groups.[1].Value) |> List.ofSeq
                        let kid = label.Split('|').[2]
                        let evPrefix = Path.Combine(evidenceRoot, kid) + "/"
                        let evReads = reads |> List.filter (fun r -> r.StartsWith evPrefix) |> List.map (fun r -> Path.GetFileNameWithoutExtension r)
                        let others = reads |> List.filter (fun r -> not (r.StartsWith evPrefix) && not (r.Contains "/od1-bundles/seq__"))
                        Some (sprintf "%s,%d,%d,%s,%s" label evReads.Length (evReads |> List.distinct |> List.length) (String.Join(" ", evReads)) (String.Join(" ", others)))
                | _ -> None))
    File.WriteAllLines(outCsv, "runId,reads,distinctReads,order,violations" :: rows)
    printfn "runs=%d" rows.Length
| _ -> eprintfn "usage: validate <casesDir> | score <casesDir> <rawDir> <outDir> | stats <outDir> <split|all> <seed> <a,b;c,d>"; exit 2
