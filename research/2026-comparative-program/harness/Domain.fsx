// OD2 domain: case and truth model, structural validation, deterministic scoring.
// Pure functions only. File IO lives in the command scripts.

open System
open System.IO
open System.Text.Json

// ---------- JSON helpers (total) ----------
let prop (name: string) (e: JsonElement) =
    match e.ValueKind with
    | JsonValueKind.Object ->
        match e.TryGetProperty name with
        | true, v when v.ValueKind <> JsonValueKind.Null -> Some v
        | _ -> None
    | _ -> None

let str name e = prop name e |> Option.bind (fun v -> if v.ValueKind = JsonValueKind.String then Some (v.GetString()) else None)
let strOr d name e = str name e |> Option.defaultValue d
let arr name e =
    prop name e
    |> Option.map (fun v -> if v.ValueKind = JsonValueKind.Array then v.EnumerateArray() |> List.ofSeq else [])
    |> Option.defaultValue []
let strList name e = arr name e |> List.choose (fun v -> if v.ValueKind = JsonValueKind.String then Some (v.GetString()) else None)
let num name e =
    prop name e
    |> Option.bind (fun v ->
        match v.ValueKind with
        | JsonValueKind.Number -> Some (v.GetDouble())
        | JsonValueKind.String -> (match Double.TryParse(v.GetString()) with | true, x -> Some x | _ -> None)
        | _ -> None)

let readJson (path: string) = JsonDocument.Parse(File.ReadAllText path).RootElement.Clone()

// ---------- Domain ----------
type Evidence = { Id: string; Stage: string; Title: string; Source: string; SourceType: string; Text: string }
type Item = { ItemId: string; RefId: string; Kind: string; AsOfStage: string option; Allowed: string list }
type Case =
    { Id: string; Split: string; Class: string; Domain: string; Title: string; Setting: string; Request: string
      Initial: Evidence list; Catalogue: Evidence list
      Hypotheses: (string * string) list; Edges: (string * string * string * string) list; Actions: (string * string) list
      Items: Item list }
type Key = { ItemId: string; Expected: string; Weight: float; Required: Set<string>; AllowedEv: Set<string>; Tags: Set<string> }
type Truth =
    { CaseId: string; Narrative: string; Keys: Key list; Discriminating: Set<string>; LowValue: Set<string>; OptimalPath: string list }

let allowedFor =
    function
    | "hypothesis" -> [ "supported"; "contradicted"; "unknown" ]
    | "edge" -> [ "causal"; "association-only"; "contradicted"; "unknown" ]
    | "action" -> [ "preferred"; "acceptable"; "premature"; "harmful" ]
    | "sufficiency" -> [ "sufficient"; "insufficient" ]
    | _ -> []

let private parseEvidence (e: JsonElement) : Evidence =
    { Id = strOr "" "id" e; Stage = strOr "S1" "stage" e; Title = strOr "" "title" e
      Source = strOr "" "source" e; SourceType = strOr "" "sourceType" e; Text = strOr "" "text" e }

let parseCase (e: JsonElement) : Case =
    let cands = prop "candidates" e
    let candList name f = cands |> Option.map (arr name >> List.map f) |> Option.defaultValue []
    { Id = strOr "" "id" e; Split = strOr "" "split" e; Class = strOr "" "benchmarkClass" e; Domain = strOr "" "domain" e
      Title = strOr "" "title" e; Setting = strOr "" "setting" e; Request = strOr "" "presentingRequest" e
      Initial = arr "initialEvidence" e |> List.map parseEvidence
      Catalogue = arr "catalogue" e |> List.map parseEvidence
      Hypotheses = candList "hypotheses" (fun h -> strOr "" "id" h, strOr "" "text" h)
      Edges = candList "edges" (fun r -> strOr "" "id" r, strOr "" "from" r, strOr "" "to" r, strOr "causes" "relation" r)
      Actions = candList "actions" (fun a -> strOr "" "id" a, strOr "" "text" a)
      Items =
        arr "scoredItems" e
        |> List.map (fun i ->
            { ItemId = strOr "" "itemId" i; RefId = strOr "" "refId" i; Kind = strOr "" "type" i
              AsOfStage = str "asOfStage" i; Allowed = strList "allowedAnswers" i }) }

let parseTruth (e: JsonElement) : Truth =
    { CaseId = strOr "" "caseId" e
      Narrative = strOr "" "groundTruthNarrative" e
      Keys =
        arr "answers" e
        |> List.map (fun a ->
            { ItemId = strOr "" "itemId" a; Expected = (strOr "" "expected" a).Trim().ToLowerInvariant()
              Weight = num "weight" a |> Option.defaultValue 1.0
              Required = strList "requiredEvidence" a |> Set.ofList; AllowedEv = strList "allowedEvidence" a |> Set.ofList
              Tags = strList "tags" a |> Set.ofList })
      Discriminating = strList "discriminatingReads" e |> Set.ofList
      LowValue = strList "lowValueReads" e |> Set.ofList
      OptimalPath = strList "optimalReadPath" e }

// ---------- Class definitions (shared by authoring and revision) ----------
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

// ---------- Structural validation (pure) ----------
let bannedPhrases =
    // "Clarity" and plain "is not" are ordinary English, so only the framework-specific forms are banned.
    [ "edf"; "echelon"; "kepner"; "tregoe"; "root cause"; "root causes"; "origin network"; "control point"; "control points"
      "propagation"; "is / is not"; "is/is not"; "five whys"; "5 whys"; "fishbone"; "ishikawa"; "nadler"; "tushman" ]

let caseText (c: Case) =
    [ yield c.Title; yield c.Setting; yield c.Request
      for e in c.Initial @ c.Catalogue do yield e.Title; yield e.Text; yield e.Source
      for (_, t) in c.Hypotheses do yield t
      for (_, f, t, _) in c.Edges do yield f; yield t
      for (_, t) in c.Actions do yield t ]
    |> String.concat "\n"

let validate (c: Case) (t: Truth) : string list =
    let itemIds = c.Items |> List.map (fun i -> i.ItemId)
    let keyIds = t.Keys |> List.map (fun k -> k.ItemId)
    let itemById = c.Items |> List.map (fun i -> i.ItemId, i) |> Map.ofList
    let evIds = (c.Initial @ c.Catalogue) |> List.map (fun e -> e.Id) |> Set.ofList
    let catIds = c.Catalogue |> List.map (fun e -> e.Id) |> Set.ofList
    let refIds =
        (c.Hypotheses |> List.map fst) @ (c.Edges |> List.map (fun (i, _, _, _) -> i)) @ (c.Actions |> List.map fst) @ [ "SUFF" ]
        |> Set.ofList
    let lower = (caseText c).ToLowerInvariant()
    let between lo hi n = n >= lo && n <= hi
    [ if c.Id <> t.CaseId then yield sprintf "caseId mismatch %s vs %s" c.Id t.CaseId
      if List.length (List.distinct itemIds) <> List.length itemIds then yield "duplicate itemIds"
      if Set.ofList itemIds <> Set.ofList keyIds then yield sprintf "items and keys differ: %A" (Set.difference (Set.union (Set.ofList itemIds) (Set.ofList keyIds)) (Set.intersect (Set.ofList itemIds) (Set.ofList keyIds)))
      if not (between 4 7 c.Initial.Length) then yield sprintf "initial evidence count %d" c.Initial.Length
      if not (between 10 14 c.Catalogue.Length) then yield sprintf "catalogue count %d" c.Catalogue.Length
      if not (between 4 6 c.Hypotheses.Length) then yield sprintf "hypothesis count %d" c.Hypotheses.Length
      if not (between 3 5 c.Edges.Length) then yield sprintf "edge count %d" c.Edges.Length
      if not (between 4 5 c.Actions.Length) then yield sprintf "action count %d" c.Actions.Length
      if (c.Items |> List.filter (fun i -> i.Kind = "sufficiency") |> List.length) <> 1 then yield "need exactly one sufficiency item"
      let covered = c.Items |> List.map (fun i -> i.RefId) |> Set.ofList
      if not (Set.isSubset refIds covered) then yield sprintf "unscored candidates: %A" (Set.difference refIds covered)
      for i in c.Items do
          if not (refIds.Contains i.RefId) then yield sprintf "%s: unknown refId %s" i.ItemId i.RefId
          if Set.ofList i.Allowed <> Set.ofList (allowedFor i.Kind) then yield sprintf "%s: allowedAnswers wrong for %s" i.ItemId i.Kind
      for k in t.Keys do
          match Map.tryFind k.ItemId itemById with
          | Some i when not (List.contains k.Expected (allowedFor i.Kind)) -> yield sprintf "%s: expected '%s' not allowed" k.ItemId k.Expected
          | _ -> ()
          let bad = Set.union k.Required k.AllowedEv - evIds
          if not bad.IsEmpty then yield sprintf "%s: unknown evidence %A" k.ItemId bad
          if not (Set.isSubset k.Required k.AllowedEv) then yield sprintf "%s: required not subset of allowed" k.ItemId
          if k.Weight <= 0.0 then yield sprintf "%s: non-positive weight" k.ItemId
      if not (Set.isSubset (Set.unionMany [ t.Discriminating; t.LowValue; Set.ofList t.OptimalPath ]) catIds) then yield "read lists name non-catalogue ids"
      if t.OptimalPath.Length > 3 then yield "optimal path longer than 3"
      if not (Set.intersect t.Discriminating t.LowValue).IsEmpty then yield "an item is both discriminating and low-value"
      if c.Class = "STOP" && not t.Discriminating.IsEmpty then yield "STOP case with discriminating reads"
      if c.Class <> "STOP" && c.Class <> "INCOMPLETE" && c.Class <> "NOCONC" && t.Discriminating.IsEmpty then yield "non-STOP case without discriminating reads"
      if not (t.Keys |> List.exists (fun k -> k.Expected = "unknown" || k.Expected = "association-only")) then yield "no unknown/association-only item"
      for p in bannedPhrases do
          // whole-word match to avoid false hits such as "credfile"
          if Text.RegularExpressions.Regex.IsMatch(lower, @"\b" + Text.RegularExpressions.Regex.Escape p + @"\b") then
              yield sprintf "banned phrase '%s'" p ]

// ---------- Scoring (pure) ----------
type Answer = { ItemId: string; Answer: string; Confidence: float; EvidenceIds: Set<string> }

type Run =
    { RunId: string; Condition: string; CaseId: string; Trial: int; Model: string; Effort: string
      Answers: Answer list option
      Reads: string list          // catalogue ids actually read, in order (mapped from run labels)
      InvalidReads: int           // Read calls outside the run's evidence files
      Brief: string
      InputTokens: int64; OutputTokens: int64; ThinkingTokens: int64; CacheReadTokens: int64; CacheCreationTokens: int64
      CostUsd: float; DurationMs: int64 }

let budget = 5

let parseAnswers (o: JsonElement) : Answer list =
    arr "answers" o
    |> List.map (fun a ->
        { ItemId = strOr "" "itemId" a
          Answer = (strOr "" "answer" a).Trim().ToLowerInvariant()
          Confidence = num "confidence" a |> Option.defaultValue 50.0 |> fun c -> Math.Clamp(c, 0.0, 100.0) / 100.0
          EvidenceIds = strList "evidenceIds" a |> Set.ofList })

type Ratio = { Num: float; Den: float }
let ratio n d = { Num = n; Den = d }

type Scored =
    { Run: Run; Case: Case; Valid: bool; Score: float; Acc: float; EvF1: float; Cal: float; Acq: float
      HarmCount: int; Breach: bool; Metrics: Map<string, Ratio>
      ItemRows: (string * string * string * bool * float) list }

let f1 (cited: Set<string>) (k: Key) =
    let p = if cited.IsEmpty then 0.0 else float (Set.intersect cited k.AllowedEv).Count / float cited.Count
    let r = if k.Required.IsEmpty then 1.0 else float (Set.intersect cited k.Required).Count / float k.Required.Count
    if p + r = 0.0 then 0.0 else 2.0 * p * r / (p + r)

let acquisition (t: Truth) (reads: string list) =
    let distinct = reads |> List.distinct
    if distinct.Length > budget then 0.0
    else
        let hits = distinct |> List.filter t.Discriminating.Contains |> List.length |> float
        let waste = distinct |> List.filter (t.Discriminating.Contains >> not) |> List.length |> float
        let base' = if t.Discriminating.IsEmpty then 1.0 else hits / float t.Discriminating.Count
        Math.Clamp(base' - 0.1 * waste, 0.0, 1.0)

let scoreRun (c: Case) (t: Truth) (run: Run) : Scored =
    let kindOf = c.Items |> List.map (fun i -> i.ItemId, i.Kind) |> Map.ofList
    let keys = t.Keys
    let ansMap = run.Answers |> Option.defaultValue [] |> List.map (fun a -> a.ItemId, a) |> Map.ofList
    let lookup (k: Key) = Map.tryFind k.ItemId ansMap
    let given (k: Key) = lookup k |> Option.map (fun a -> a.Answer) |> Option.defaultValue "<missing>"
    let isCorrect (k: Key) = given k = k.Expected
    let wtot = keys |> List.sumBy (fun k -> k.Weight)
    let wsum f = keys |> List.sumBy (fun k -> k.Weight * f k)
    let valid = run.Answers.IsSome
    let acc = if wtot = 0.0 then 0.0 else wsum (fun k -> if isCorrect k then 1.0 else 0.0) / wtot
    let evf1 = if wtot = 0.0 then 0.0 else wsum (fun k -> match lookup k with Some a when a.Answer = k.Expected -> f1 a.EvidenceIds k | _ -> 0.0) / wtot
    let cal =
        if keys.IsEmpty then 0.0
        else keys |> List.averageBy (fun k -> match lookup k with
                                              | Some a -> let y = if a.Answer = k.Expected then 1.0 else 0.0 in 1.0 - (a.Confidence - y) ** 2.0
                                              | None -> 0.0)
    let breach = (run.Reads |> List.distinct |> List.length) > budget
    let acq = acquisition t run.Reads
    let kind (k: Key) = Map.tryFind k.ItemId kindOf |> Option.defaultValue ""
    let of' pred hit = keys |> List.filter pred |> fun ks -> ratio (ks |> List.filter hit |> List.length |> float) (float ks.Length)
    let harmful = keys |> List.filter (fun k -> kind k = "action" && k.Expected = "harmful" && List.contains (given k) [ "preferred"; "acceptable" ])
    let reads = run.Reads |> List.distinct
    let metrics =
        [ "hypothesisAccuracy", of' (fun k -> kind k = "hypothesis") isCorrect
          "falseDiagnosis", of' (fun k -> kind k = "hypothesis" && k.Expected <> "supported") (fun k -> given k = "supported")
          "missedProblem", of' (fun k -> kind k = "hypothesis" && k.Expected = "supported") (fun k -> given k <> "supported")
          "unsupportedCertainty", of' (fun k -> k.Expected = "unknown") (fun k -> given k <> "unknown")
          "overAbstention", of' (fun k -> k.Expected <> "unknown" && (kind k = "hypothesis" || kind k = "edge")) (fun k -> given k = "unknown")
          "causalAccuracy", of' (fun k -> kind k = "edge") isCorrect
          "causalOverclaim", of' (fun k -> kind k = "edge" && k.Expected <> "causal") (fun k -> given k = "causal")
          "abstentionAccuracy", of' (fun k -> kind k = "sufficiency") isCorrect
          "missedAbstention", of' (fun k -> kind k = "sufficiency" && k.Expected = "insufficient") (fun k -> given k = "sufficient")
          "falseAbstention", of' (fun k -> kind k = "sufficiency" && k.Expected = "sufficient") (fun k -> given k = "insufficient")
          "actionAccuracy", of' (fun k -> kind k = "action") isCorrect
          "prematurePromotion", of' (fun k -> kind k = "action" && k.Expected = "premature") (fun k -> given k = "preferred")
          "harmfulPromotion", of' (fun k -> kind k = "action" && k.Expected = "harmful") (fun k -> List.contains (given k) [ "preferred"; "acceptable" ])
          "highConfidenceError", of' (fun k -> lookup k |> Option.exists (fun a -> a.Confidence >= 0.8)) (isCorrect >> not)
          "discriminatingRecall", ratio (reads |> List.filter t.Discriminating.Contains |> List.length |> float) (float t.Discriminating.Count)
          "lowValueReadShare", ratio (reads |> List.filter t.LowValue.Contains |> List.length |> float) (float reads.Length)
          "budgetBreach", ratio (if breach then 1.0 else 0.0) 1.0 ]
        |> Map.ofList
    let score =
        if not valid then 0.0
        else max 0.0 (60.0 * acc + 10.0 * evf1 + 10.0 * cal + 20.0 * acq - 15.0 * float harmful.Length)
    { Run = run; Case = c; Valid = valid; Score = score; Acc = acc; EvF1 = evf1; Cal = cal; Acq = acq
      HarmCount = harmful.Length; Breach = breach; Metrics = metrics
      ItemRows = keys |> List.map (fun k -> k.ItemId, given k, k.Expected, isCorrect k, lookup k |> Option.map (fun a -> a.Confidence) |> Option.defaultValue nan) }

// ---------- Statistics (pure given a seed) ----------
let mean (xs: float list) = if xs.IsEmpty then nan else List.average xs

let bootstrapCI (seed: int) (n: int) (xs: float list) =
    let a = Array.ofList xs
    if a.Length = 0 then nan, nan
    else
        let rng = Random(seed)
        let means = Array.init n (fun _ -> Array.init a.Length (fun _ -> a.[rng.Next a.Length]) |> Array.average) |> Array.sort
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
