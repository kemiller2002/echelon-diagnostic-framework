// Builds the preregistered OD2 held-out run plan from the frozen case list.
// Usage (from research/2026-comparative-program):
//   dotnet fsi harness/Plan.fsx heldout <out.json>
// Pure construction; writes one JSON file.

open System.IO
open System.Text.Json

let sonnet = "claude-sonnet-5-5"
let haiku = "claude-haiku-5-5"

type P = { condition: string; caseId: string; trial: int; model: string; effort: string }

/// (condition, model, effort, trials) per preregistration §3.
let design =
    [ "A_GENERAL", sonnet, "medium", 5
      "B_EDF03", sonnet, "medium", 3
      "C_KT", sonnet, "medium", 2
      "D_KTACH", sonnet, "medium", 2
      "E_NOCTX", sonnet, "medium", 2
      "E_NOCAUSAL", sonnet, "medium", 2
      "E_NOEVID", sonnet, "medium", 2
      "E_NOCTRL", sonnet, "medium", 2
      "E_NOSUFF", sonnet, "medium", 2
      "E_NOSEQ", sonnet, "medium", 2
      "F_EDFG", sonnet, "medium", 3
      "SEL_CLAUDE", sonnet, "medium", 2
      "SEL_EDFO", sonnet, "medium", 2
      "A_GENERAL", sonnet, "low", 1
      "A_GENERAL", sonnet, "high", 1
      "A_GENERAL", sonnet, "max", 1
      "B_EDF03", sonnet, "low", 1
      "B_EDF03", sonnet, "high", 1
      "B_EDF03", sonnet, "max", 1
      "A_GENERAL", haiku, "low", 2
      "B_EDF03", haiku, "low", 2
      "C_KT", haiku, "low", 2
      "F_EDFG", haiku, "low", 2 ]

let plan (caseIds: string list) =
    [ for c in caseIds do
        for (cond, model, effort, trials) in design do
            for t in 1 .. trials do
                yield { condition = cond; caseId = c; trial = t; model = model; effort = effort } ]

match fsi.CommandLineArgs |> Array.toList |> List.tail with
| [ "heldout"; out ] ->
    let ids =
        Directory.GetFiles("benchmark/cases/heldout", "*.case.json")
        |> Array.map (fun p -> Path.GetFileName(p).Replace(".case.json", "")) |> Array.sort |> List.ofArray
    let runs = plan ids
    let doc = dict [ "phase", box "heldout"; "seed", box 20261117; "cases", box ids; "runs", box runs ]
    File.WriteAllText(out, JsonSerializer.Serialize(doc, JsonSerializerOptions(WriteIndented = true)))
    printfn "%d cases, %d runs" ids.Length runs.Length
| _ ->
    eprintfn "usage: heldout <out.json>"
    exit 2
