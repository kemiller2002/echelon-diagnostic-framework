// Builds the OD2 condition modules that derive from the canonical EDF v0.3 text.
// Usage (from research/2026-comparative-program): dotnet fsi harness/Prompts.fsx
// Inputs: docs/edf/specification.md (frozen v0.3) and hand-authored modules in prompts/.
// Output: prompts/<COND>.module.txt for B, E_* ablations, F and the two selection arms.
// Pure text transformations; the only effects are reading and writing files.

open System
open System.IO
open System.Text.RegularExpressions

let specPath = "../../docs/edf/specification.md"
let spec = File.ReadAllText(specPath).Replace("\r\n", "\n")
let lines = spec.Split('\n') |> List.ofArray

let preamble = "Method: apply the following diagnostic framework internally, then answer the scored items.\n\n"

// ---------- section helpers ----------
let isH2 (l: string) = l.StartsWith "## "
let isH3 (l: string) = l.StartsWith "### "
let stepHeading = Regex(@"^### (\d+)\. (.+)$")
let seqItem = Regex(@"^(\d+)\. (.+)$")

/// Drop an H2 section by title (heading through the line before the next H2).
let dropH2 (title: string) (ls: string list) =
    let rec go inside acc =
        function
        | [] -> List.rev acc
        | l :: rest when isH2 l -> if l = "## " + title then go true acc rest else go false (l :: acc) rest
        | l :: rest -> if inside then go true acc rest else go false (l :: acc) rest
    go false [] ls

/// Drop "### N. ..." subsections whose step number is in `steps`.
let dropStepNotes (steps: Set<int>) (ls: string list) =
    let rec go inside acc =
        function
        | [] -> List.rev acc
        | l :: rest when isH2 l -> go false (l :: acc) rest
        | l :: rest when isH3 l ->
            let m = stepHeading.Match l
            let drop = m.Success && steps.Contains(int m.Groups.[1].Value)
            if drop then go true acc rest else go false (l :: acc) rest
        | l :: rest -> if inside then go true acc rest else go false (l :: acc) rest
    go false [] ls

/// Drop numbered sequence items (and their indented sub-bullets) in the Required Diagnostic Sequence section.
let dropSequenceItems (steps: Set<int>) (ls: string list) =
    let rec go inSeq skipping acc =
        function
        | [] -> List.rev acc
        | l :: rest when isH2 l -> go (l = "## Required Diagnostic Sequence") false (l :: acc) rest
        | l :: rest when inSeq && seqItem.IsMatch l ->
            let n = int (seqItem.Match l).Groups.[1].Value
            if steps.Contains n then go inSeq true acc rest else go inSeq false (l :: acc) rest
        | l :: rest when inSeq && skipping && l.StartsWith "   " -> go inSeq true acc rest
        | l :: rest -> go inSeq false (l :: acc) rest
    go false false [] ls

/// Drop "- X" bullets in Required Output Elements whose text is in `names`.
let dropOutputs (names: Set<string>) (ls: string list) =
    let rec go inOut acc =
        function
        | [] -> List.rev acc
        | l :: rest when isH2 l -> go (l = "## Required Output Elements") (l :: acc) rest
        | l :: rest when inOut && l.StartsWith "- " && names.Contains(l.Substring 2) -> go inOut acc rest
        | l :: rest -> go inOut (l :: acc) rest
    go false [] ls

/// Renumber remaining sequence items and step-note headings consecutively, keeping them aligned.
let renumber (ls: string list) =
    let order =
        ls
        |> List.skipWhile (fun l -> l <> "## Required Diagnostic Sequence")
        |> List.tail
        |> List.takeWhile (isH2 >> not)
        |> List.choose (fun l -> let m = seqItem.Match l in if m.Success then Some (int m.Groups.[1].Value) else None)
    let newNumber = order |> List.mapi (fun i n -> n, i + 1) |> Map.ofList
    let rec go inSeq acc =
        function
        | [] -> List.rev acc
        | l :: rest when isH2 l -> go (l = "## Required Diagnostic Sequence") (l :: acc) rest
        | l :: rest when inSeq && seqItem.IsMatch l ->
            let m = seqItem.Match l
            go inSeq (sprintf "%d. %s" (Map.find (int m.Groups.[1].Value) newNumber) m.Groups.[2].Value :: acc) rest
        | l :: rest when stepHeading.IsMatch l ->
            let m = stepHeading.Match l
            go inSeq (sprintf "### %d. %s" (Map.find (int m.Groups.[1].Value) newNumber) m.Groups.[2].Value :: acc) rest
        | l :: rest -> go inSeq (l :: acc) rest
    go false [] ls

let collapseBlankRuns (ls: string list) =
    ls
    |> List.fold (fun (acc, prevBlank) l ->
        let blank = String.IsNullOrWhiteSpace l
        if blank && prevBlank then acc, true else l :: acc, blank) ([], false)
    |> fst
    |> List.rev

let render (ls: string list) = preamble + (ls |> collapseBlankRuns |> String.concat "\n")

let ablate (steps: int list) (outputs: string list) (extra: string list -> string list) =
    lines
    |> extra
    |> dropSequenceItems (Set.ofList steps)
    |> dropStepNotes (Set.ofList steps)
    |> dropOutputs (Set.ofList outputs)
    |> renumber
    |> render

/// Unordered variant: same content, sequence language removed.
let unordered () =
    lines
    |> List.map (fun l ->
        match l with
        | "## Required Diagnostic Sequence" -> "## Required Diagnostic Elements"
        | "Every EDF analysis should follow this sequence:" -> "Every EDF analysis should address these elements, in any order:"
        | "## Sequence Notes" -> "## Element Notes"
        | "The sequence remains the same across levels. The depth changes." -> "The elements remain the same across levels. The depth changes."
        | _ when stepHeading.IsMatch l -> "### " + (stepHeading.Match l).Groups.[2].Value
        | _ when seqItem.IsMatch l -> "- " + (seqItem.Match l).Groups.[2].Value
        | _ -> l)
    |> render

let read name = File.ReadAllText(Path.Combine("prompts", name))
let write name (text: string) = File.WriteAllText(Path.Combine("prompts", name), text.TrimEnd() + "\n")

let bModule = render lines

let techniqueLibrary =
    String.concat "\n\n"
        [ "=== Technique reference A: unstructured careful reasoning ===\nThink carefully about what is really going on in this system before answering."
          "=== Technique reference B: EDF v0.3 ===\n" + bModule.Substring(preamble.Length)
          "=== Technique reference C: Kepner-Tregoe ===\n" + read "C_KT.module.txt"
          "=== Technique reference D: Analysis of Competing Hypotheses ===\n"
          + (let t = read "D_KTACH.module.txt" in
             let s = t.IndexOf "Part 2 - Analysis of Competing Hypotheses"
             let e = t.IndexOf "Part 3 - Decision Analysis"
             t.Substring(s, e - s).Trim()) ]

let selClaude =
    "Method selection. Four diagnostic techniques are given in full below. Before analyzing, choose the technique, or combination of at most two techniques, best suited to this particular problem. Report your choice and a one-sentence reason in the output field methodChoice. Then apply what you chose internally and answer the scored items.\n\n"
    + techniqueLibrary

let selEdfo =
    String.concat "\n"
        [ "Method: use EDF as the diagnostic record and choose techniques step by step (experimental orchestration variant)."
          ""
          "1. Keep an EDF diagnostic record throughout the analysis: System Context (primary system, context, focus); Outcome stated factually; Manifestations; an evidence ledger (for each item: who produced it, whether that source has a stake, whether it is independent, and whether it is observation or inference); the live hypotheses; the Origin Network; Control Points; and Diagnostic Sufficiency."
          "2. At each step, choose the technique from the reference below that fits the question you face now:"
          "   - Localizing a deviation (what, where, when and how much is affected, and what comparable things are not): Kepner-Tregoe specification and distinctions."
          "   - Several live explanations: Analysis of Competing Hypotheses (weigh diagnostic evidence; prefer the hypothesis with the least inconsistent evidence)."
          "   - Disputed, interested or possibly gamed sources: the evidence-ledger provenance check."
          "   - A simple, well-evidenced problem: stop structuring and answer directly."
          "3. When new evidence changes the question, switch technique and keep the record. Do not restart."
          "4. Report the techniques you used, in order, in the output field methodChoice."
          ""
          read "F_GATES.addendum.txt"
          ""
          "Technique reference follows."
          ""
          techniqueLibrary ]

let modules =
    [ "B_EDF03", bModule
      "E_NOCTX", ablate [ 1 ] [ "System Context"; "Primary System"; "Context"; "Focus" ] (dropH2 "Context Principle")
      "E_NOCAUSAL", ablate [ 4; 5 ] [ "Origin Network"; "Propagation" ] id
      "E_NOEVID", ablate [ 6 ] [ "Evidence" ] id
      "E_NOCTRL", ablate [ 7; 8 ] [ "Control Points"; "Control Point Ranking" ] id
      "E_NOSUFF", ablate [ 9 ] [ "Diagnostic Sufficiency" ] id
      "E_NOSEQ", unordered ()
      "F_EDFG", bModule.TrimEnd() + "\n" + read "F_GATES.addendum.txt"
      "SEL_CLAUDE", selClaude
      "SEL_EDFO", selEdfo ]

modules |> List.iter (fun (name, text) -> write (name + ".module.txt") text; printfn "%-12s %6d chars" name text.Length)
