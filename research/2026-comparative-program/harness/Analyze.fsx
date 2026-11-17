// OD2 track analyses (A: compute, B: persuasiveness, C: method selection).
// Usage (from research/2026-comparative-program):
//   dotnet fsi harness/Analyze.fsx tracks <scoredDir> <rawDir> <ratingsDir> <seed> <out.md>
// Reads runs.csv (from Score.fsx), raw run records and rating records.
// Pure computation; writes one markdown report.

#load "Domain.fsx"

open System
open System.IO
open System.Text
open System.Text.Json
open Domain

let parseLine (line: string) =
    let rec go i inQ (cur: StringBuilder) acc =
        if i >= line.Length then List.rev (cur.ToString() :: acc)
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

let readCsv (path: string) =
    let lines = File.ReadAllLines path |> List.ofArray
    let header = parseLine (List.head lines)
    lines |> List.tail |> List.map (fun l -> List.zip header (parseLine l) |> Map.ofList)

let fnum (k: string) (r: Map<string, string>) = Double.Parse(Map.find k r, Globalization.CultureInfo.InvariantCulture)
let cellOf (r: Map<string, string>) = sprintf "%s@%s-%s" r.["condition"] r.["tier"] r.["effort"]

/// Paired case-level contrast of a per-run measure between two cells.
let paired seed (rows: Map<string, string> list) (measure: Map<string, string> -> float option) (a: string) (b: string) =
    let means cell =
        rows |> List.filter (fun r -> cellOf r = cell)
        |> List.choose (fun r -> measure r |> Option.map (fun v -> r.["caseId"], v))
        |> List.groupBy fst |> List.map (fun (k, vs) -> k, vs |> List.averageBy snd) |> Map.ofList
    let ma, mb = means a, means b
    let common = Set.intersect (ma |> Map.keys |> Set.ofSeq) (mb |> Map.keys |> Set.ofSeq) |> Set.toList
    if common.IsEmpty then None
    else
        let d = common |> List.map (fun k -> ma.[k] - mb.[k])
        let lo, hi = bootstrapCI seed 10000 d
        Some (common.Length, List.average d, lo, hi)

let rank (xs: float list) =
    let indexed = xs |> List.mapi (fun i x -> i, x) |> List.sortBy snd
    let ranks = Array.zeroCreate xs.Length
    let rec assign (group: (int * float) list) (rest: (int * float) list) pos =
        match rest with
        | [] ->
            let avg = (float pos + float (pos + group.Length - 1)) / 2.0
            group |> List.iter (fun (i, _) -> ranks.[i] <- avg)
        | (i, x) :: tl when not group.IsEmpty && snd (List.head group) = x -> assign (group @ [ (i, x) ]) tl pos
        | (i, x) :: tl ->
            if not group.IsEmpty then
                let avg = (float pos + float (pos + group.Length - 1)) / 2.0
                group |> List.iter (fun (j, _) -> ranks.[j] <- avg)
            assign [ (i, x) ] tl (pos + group.Length)
    assign [] indexed 1
    List.ofArray ranks

let pearson (xs: float list) (ys: float list) =
    let mx, my = List.average xs, List.average ys
    let cov = List.map2 (fun x y -> (x - mx) * (y - my)) xs ys |> List.sum
    let sx = xs |> List.sumBy (fun x -> (x - mx) ** 2.0) |> sqrt
    let sy = ys |> List.sumBy (fun y -> (y - my) ** 2.0) |> sqrt
    if sx = 0.0 || sy = 0.0 then nan else cov / (sx * sy)

let spearman xs ys = pearson (rank xs) (rank ys)

let spearmanCI seed (pairs: (float * float) list) =
    let a = Array.ofList pairs
    let rng = Random(seed)
    let rs =
        Array.init 2000 (fun _ -> Array.init a.Length (fun _ -> a.[rng.Next a.Length]) |> List.ofArray)
        |> Array.map (fun s -> spearman (s |> List.map fst) (s |> List.map snd))
        |> Array.filter (Double.IsNaN >> not) |> Array.sort
    if rs.Length < 20 then nan, nan else rs.[int (0.025 * float rs.Length)], rs.[int (0.975 * float rs.Length) - 1]

let methodCode (choice: string) =
    let c = choice.ToLowerInvariant()
    let has (xs: string list) = xs |> List.exists c.Contains
    let picks =
        [ if has [ "edf"; "reference b"; "technique b" ] then yield "B"
          if has [ "kepner"; "tregoe"; "reference c"; "technique c"; "is / is not"; "is/is not" ] then yield "C"
          if has [ "competing hypotheses"; "ach"; "reference d"; "technique d"; "heuer" ] then yield "D"
          if has [ "unstructured"; "careful reasoning"; "reference a"; "technique a" ] then yield "A" ]
    match picks with
    | [] -> "uncoded"
    | [ x ] -> x
    | xs -> String.Join("+", xs)

let fixedArm = Map.ofList [ "A", "A_GENERAL"; "B", "B_EDF03"; "C", "C_KT"; "D", "D_KTACH" ]

let tracks (scoredDir: string) (rawDir: string) (ratingsDir: string) (seed: int) (outPath: string) =
    let rows = readCsv (Path.Combine(scoredDir, "runs.csv"))
    let sb = StringBuilder()
    let line (s: string) = sb.AppendLine s |> ignore
    let fmt = function
        | Some (n, m, lo, hi) -> sprintf "%d | %.3f | [%.3f, %.3f]" n m lo hi
        | None -> "- | - | -"
    line "# OD2 track analyses"
    line ""
    // ---------- Track A ----------
    line "## Track A — structure vs compute"
    line ""
    line "| cell | runs | score | acc | acq | thinking tok | cost USD | time s |"
    line "|---|---|---|---|---|---|---|---|"
    for cond in [ "A_GENERAL"; "B_EDF03"; "A_VOTE5" ] do
        for effort in [ "low"; "medium"; "high"; "max" ] do
            let rs = rows |> List.filter (fun r -> r.["condition"] = cond && r.["tier"] = "sonnet" && r.["effort"] = effort)
            if not rs.IsEmpty then
                let m k = rs |> List.averageBy (fnum k)
                line (sprintf "| %s@sonnet-%s | %d | %.2f | %.3f | %.3f | %.0f | %.4f | %.0f |" cond effort rs.Length (m "score") (m "acc") (m "acq") (m "thinkingTokens") (m "costUsd") (m "durationMs" / 1000.0))
    line ""
    line "| contrast | measure | cases | mean diff | 95% CI |"
    line "|---|---|---|---|---|"
    let score = fun (r: Map<string, string>) -> Some (fnum "score" r)
    let acc = fun (r: Map<string, string>) -> Some (fnum "acc" r)
    let cost = fun (r: Map<string, string>) -> Some (fnum "costUsd" r)
    for (a, b) in [ "A_VOTE5@sonnet-medium", "B_EDF03@sonnet-medium"; "A_VOTE5@sonnet-medium", "A_GENERAL@sonnet-medium"
                    "B_EDF03@sonnet-low", "A_GENERAL@sonnet-low"; "B_EDF03@sonnet-high", "A_GENERAL@sonnet-high"; "B_EDF03@sonnet-max", "A_GENERAL@sonnet-max"
                    "A_GENERAL@sonnet-max", "A_GENERAL@sonnet-medium"; "B_EDF03@sonnet-max", "B_EDF03@sonnet-medium" ] do
        for (mn, m) in [ "score", score; "acc", acc; "costUsd", cost ] do
            line (sprintf "| %s − %s | %s | %s |" a b mn (fmt (paired seed rows m a b)))
    line ""
    // ---------- Track B ----------
    line "## Track B — accuracy vs persuasiveness (rater: Claude Opus 5.5, blind; not independent human evidence)"
    line ""
    let ratings =
        if Directory.Exists ratingsDir then
            Directory.GetFiles(ratingsDir, "*.rating.json")
            |> Array.choose (fun p ->
                let e = readJson p
                match prop "output" e with
                | Some o -> Some (strOr "" "label" e, (num "persuasiveness" o, num "perceivedCorrectness" o, num "actionability" o))
                | None -> None)
            |> Map.ofArray
        else Map.empty
    let rated = rows |> List.filter (fun r -> ratings.ContainsKey r.["runId"])
    let rget sel (r: Map<string, string>) = Map.tryFind r.["runId"] ratings |> Option.bind sel
    let pers = rget (fun (p, _, _) -> p)
    let perc = rget (fun (_, c, _) -> c)
    let act = rget (fun (_, _, a) -> a)
    let gap (r: Map<string, string>) = perc r |> Option.map (fun c -> c / 100.0 - fnum "acc" r)
    line (sprintf "Rated briefs: %d." rated.Length)
    line ""
    line "| cell | briefs | persuasiveness | perceived correctness | actionability | true acc | persuasion gap | Spearman(perceived, acc) [95% CI] |"
    line "|---|---|---|---|---|---|---|---|"
    for c in rated |> List.map cellOf |> List.distinct |> List.sort do
        let rs = rated |> List.filter (fun r -> cellOf r = c)
        let avg f = rs |> List.choose f |> fun xs -> if xs.IsEmpty then nan else List.average xs
        let pairs = rs |> List.choose (fun r -> perc r |> Option.map (fun p -> p, fnum "acc" r))
        let rho = if pairs.Length >= 5 then spearman (pairs |> List.map fst) (pairs |> List.map snd) else nan
        let lo, hi = if pairs.Length >= 5 then spearmanCI seed pairs else nan, nan
        line (sprintf "| %s | %d | %.2f | %.1f | %.2f | %.3f | %.3f | %.3f [%.3f, %.3f] |" c rs.Length (avg pers) (avg perc) (avg act) (avg (fun r -> Some (fnum "acc" r))) (avg gap) rho lo hi)
    line ""
    line "| contrast | measure | cases | mean diff | 95% CI |"
    line "|---|---|---|---|---|"
    for (a, b) in [ "B_EDF03@sonnet-medium", "A_GENERAL@sonnet-medium"; "B_EDF03@sonnet-medium", "C_KT@sonnet-medium"; "F_EDFG@sonnet-medium", "A_GENERAL@sonnet-medium" ] do
        for (mn, m) in [ "persuasiveness", pers; "perceivedCorrectness", perc; "actionability", act; "trueAcc", acc; "persuasionGap", gap ] do
            line (sprintf "| %s − %s | %s | %s |" a b mn (fmt (paired seed rated m a b)))
    line ""
    // ---------- Track C ----------
    line "## Track C — method selection"
    line ""
    let choices =
        Directory.GetFiles(rawDir, "*.json")
        |> Array.choose (fun p ->
            let e = readJson p
            let cond = strOr "" "condition" e
            if cond = "SEL_CLAUDE" || cond = "SEL_EDFO" then
                let choice = prop "run" e |> Option.bind (prop "output") |> Option.bind (str "methodChoice") |> Option.defaultValue ""
                Some (strOr "" "runId" e, (cond, strOr "" "caseId" e, choice))
            else None)
        |> List.ofArray
    let caseMean cond caseId =
        rows |> List.filter (fun r -> r.["condition"] = cond && r.["caseId"] = caseId && r.["tier"] = "sonnet" && r.["effort"] = "medium")
        |> fun rs -> if rs.IsEmpty then None else Some (rs |> List.averageBy (fnum "score"))
    let scoreOf runId = rows |> List.tryFind (fun r -> r.["runId"] = runId) |> Option.map (fnum "score")
    line "| arm | runs | coded choices | selection errors | execution errors | best-arm choices |"
    line "|---|---|---|---|---|---|"
    for arm in [ "SEL_CLAUDE"; "SEL_EDFO" ] do
        let mine = choices |> List.filter (fun (_, (c, _, _)) -> c = arm)
        let coded = mine |> List.map (fun (id, (_, caseId, ch)) -> id, caseId, methodCode ch)
        let dist = coded |> List.countBy (fun (_, _, m) -> m) |> List.sortByDescending snd |> List.map (fun (m, n) -> sprintf "%s:%d" m n)
        let judged =
            coded |> List.choose (fun (id, caseId, m) ->
                match Map.tryFind m fixedArm with
                | Some cond ->
                    let best = [ "A_GENERAL"; "B_EDF03"; "C_KT"; "D_KTACH" ] |> List.choose (fun c -> caseMean c caseId) |> fun xs -> if xs.IsEmpty then None else Some (List.max xs)
                    match caseMean cond caseId, best, scoreOf id with
                    | Some chosen, Some b, Some s -> Some (chosen < b - 5.0, chosen >= b - 5.0 && s < b - 5.0, chosen >= b - 1e-9)
                    | _ -> None
                | None -> None)
        line (sprintf "| %s | %d | %s | %d | %d | %d |" arm mine.Length (String.Join(", ", dist))
                (judged |> List.filter (fun (s, _, _) -> s) |> List.length) (judged |> List.filter (fun (_, e, _) -> e) |> List.length) (judged |> List.filter (fun (_, _, b) -> b) |> List.length))
    line ""
    line "Selection error: chose a fixed method whose case mean is ≥ 5 below the best fixed arm. Execution error: chose within 5 of the best arm but the run itself scored ≥ 5 below it. Combinations and uncoded choices are counted in the distribution only."
    File.WriteAllText(outPath, sb.ToString())
    printfn "%s" (sb.ToString())

/// Preregistration §4 decision rule.
let verdict (m: float) (lo: float) (hi: float) =
    if m >= 5.0 && lo > 0.0 then "material advantage"
    elif m <= -5.0 && hi < 0.0 then "material disadvantage"
    elif lo >= -3.0 && hi <= 3.0 then "practically equivalent"
    else "inconclusive"

let verdicts (scoredDir: string) (seed: int) (outPath: string) =
    let rows = readCsv (Path.Combine(scoredDir, "runs.csv"))
    let items = readCsv (Path.Combine(scoredDir, "items.csv"))
    let sb = StringBuilder()
    let line (s: string) = sb.AppendLine s |> ignore
    let score = fun (r: Map<string, string>) -> Some (fnum "score" r)
    let acc = fun (r: Map<string, string>) -> Some (fnum "acc" r * 100.0)
    let acq = fun (r: Map<string, string>) -> Some (fnum "acq" r * 100.0)
    let sm = "@sonnet-medium"
    let hl = "@haiku-low"
    let tests =
        [ "H1", "EDF vs no framework", "B_EDF03" + sm, "A_GENERAL" + sm
          "H2", "EDF vs Kepner-Tregoe", "B_EDF03" + sm, "C_KT" + sm
          "H3", "EDF vs KT+ACH", "B_EDF03" + sm, "D_KTACH" + sm
          "H4", "gates+VOI vs EDF", "F_EDFG" + sm, "B_EDF03" + sm
          "H5a", "System Context", "B_EDF03" + sm, "E_NOCTX" + sm
          "H5b", "Origin Network + Propagation", "B_EDF03" + sm, "E_NOCAUSAL" + sm
          "H5c", "Evidence evaluation", "B_EDF03" + sm, "E_NOEVID" + sm
          "H5d", "Control Points + Ranking", "B_EDF03" + sm, "E_NOCTRL" + sm
          "H5e", "Diagnostic Sufficiency", "B_EDF03" + sm, "E_NOSUFF" + sm
          "H6", "Sequence", "B_EDF03" + sm, "E_NOSEQ" + sm
          "H7a", "vote-of-5 (no framework) vs EDF", "A_VOTE5" + sm, "B_EDF03" + sm
          "H9a", "Claude-selected method vs KT", "SEL_CLAUDE" + sm, "C_KT" + sm
          "H9b", "EDF orchestrator vs EDF", "SEL_EDFO" + sm, "B_EDF03" + sm
          "H11-H1", "Haiku: EDF vs no framework", "B_EDF03" + hl, "A_GENERAL" + hl
          "H11-H2", "Haiku: EDF vs KT", "B_EDF03" + hl, "C_KT" + hl
          "H11-H4", "Haiku: gates+VOI vs EDF", "F_EDFG" + hl, "B_EDF03" + hl ]
    line "# OD2 preregistered verdicts"
    line ""
    line "Decision rule (preregistration §4): material advantage = Δ ≥ +5 and CI > 0; practically equivalent = CI within [−3, +3]; material disadvantage = Δ ≤ −5 and CI < 0; otherwise inconclusive. Accuracy and acquisition are on a 0–100 scale for comparability."
    line ""
    line "| id | comparison | contrast | cases | Δ score [95% CI] | verdict | Δ accuracy [95% CI] | Δ acquisition [95% CI] |"
    line "|---|---|---|---|---|---|---|---|"
    for (id, name, a, b) in tests do
        match paired seed rows score a b with
        | Some (n, m, lo, hi) ->
            let f x = match paired seed rows x a b with Some (_, m2, l2, h2) -> sprintf "%+.2f [%.2f, %.2f]" m2 l2 h2 | None -> "-"
            line (sprintf "| %s | %s | %s − %s | %d | %+.2f [%.2f, %.2f] | **%s** | %s | %s |" id name a b n m lo hi (verdict m lo hi) (f acc) (f acq))
        | None -> line (sprintf "| %s | %s | %s − %s | 0 | - | not run | - | - |" id name a b)
    line ""
    // H10: test-retest
    line "## H10 — item test–retest agreement (trials 1 vs 2, Sonnet-medium)"
    line ""
    line "| condition | item pairs | agreement [95% Wilson] |"
    line "|---|---|---|"
    for cond in [ "A_GENERAL"; "B_EDF03"; "C_KT"; "D_KTACH"; "F_EDFG"; "SEL_CLAUDE"; "SEL_EDFO" ] do
        let mine = items |> List.filter (fun r -> r.["condition"] = cond && r.["tier"] = "sonnet" && r.["effort"] = "medium")
        let byTrial t = mine |> List.filter (fun r -> r.["trial"] = t) |> List.map (fun r -> (r.["caseId"], r.["itemId"]), r.["given"]) |> Map.ofList
        let t1, t2 = byTrial "1", byTrial "2"
        let common = Set.intersect (t1 |> Map.keys |> Set.ofSeq) (t2 |> Map.keys |> Set.ofSeq)
        if not common.IsEmpty then
            let agree = common |> Set.filter (fun k -> t1.[k] = t2.[k]) |> Set.count
            let lo, hi = wilson { Num = float agree; Den = float common.Count }
            line (sprintf "| %s | %d | %.3f [%.3f, %.3f] |" cond common.Count (float agree / float common.Count) lo hi)
    line ""
    // class breakdown, B vs A and C vs A
    line "## Class and domain breakdown (Sonnet-medium; case means, trials averaged)"
    line ""
    let conds = [ "A_GENERAL"; "B_EDF03"; "C_KT"; "D_KTACH"; "F_EDFG" ]
    let classes = rows |> List.map (fun r -> r.["class"]) |> List.distinct |> List.sort
    line ("| class | " + String.Join(" | ", conds |> List.map (fun c -> c + " score / acc")) + " |")
    line ("|---|" + String.replicate conds.Length "---|")
    for cl in classes @ [ "organizational"; "technical"; "cross-domain" ] do
        let sel (r: Map<string, string>) = r.["class"] = cl || r.["domain"] = cl
        let cells =
            conds |> List.map (fun c ->
                let rs = rows |> List.filter (fun r -> sel r && r.["condition"] = c && r.["tier"] = "sonnet" && r.["effort"] = "medium")
                if rs.IsEmpty then "-" else sprintf "%.1f / %.2f" (rs |> List.averageBy (fnum "score")) (rs |> List.averageBy (fnum "acc")))
        line (sprintf "| %s | %s |" cl (String.Join(" | ", cells)))
    line ""
    // mechanism M5: reads by class
    line "## M5 — evidence collected by class (mean items read, Sonnet-medium)"
    line ""
    let mconds = [ "A_GENERAL"; "B_EDF03"; "E_NOSUFF"; "F_EDFG"; "C_KT" ]
    line ("| class | " + String.Join(" | ", mconds) + " |")
    line ("|---|" + String.replicate mconds.Length "---|")
    for cl in classes do
        let cells =
            mconds |> List.map (fun c ->
                let rs = rows |> List.filter (fun r -> r.["class"] = cl && r.["condition"] = c && r.["tier"] = "sonnet" && r.["effort"] = "medium")
                if rs.IsEmpty then "-" else sprintf "%.2f" (rs |> List.averageBy (fnum "reads")))
        line (sprintf "| %s | %s |" cl (String.Join(" | ", cells)))
    File.WriteAllText(outPath, sb.ToString())
    printfn "%s" (sb.ToString())

match fsi.CommandLineArgs |> Array.toList |> List.tail with
| [ "tracks"; scoredDir; rawDir; ratingsDir; seed; out ] -> tracks scoredDir rawDir ratingsDir (int seed) out
| [ "verdicts"; scoredDir; seed; out ] -> verdicts scoredDir (int seed) out
| _ ->
    eprintfn "usage: tracks <scoredDir> <rawDir> <ratingsDir> <seed> <out.md> | verdicts <scoredDir> <seed> <out.md>"
    exit 2
