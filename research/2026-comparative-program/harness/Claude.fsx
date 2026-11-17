// Headless Claude Code runner for OD2 (EDF-COMPARATIVE-2026-002).
// One isolated `claude -p` session per call: an empty working directory, the Read
// tool only, a fixed minimal system prompt, no settings sources, no MCP servers,
// structured output. The full stream-json transcript is preserved for audit.
// The only effects are process execution and writing the transcript file.

open System
open System.Diagnostics
open System.IO
open System.Text.Json

type RunSpec =
    { Label: string
      WorkDir: string          // isolated directory containing only the files the session may read
      Prompt: string           // short instruction; the real task lives in a file under WorkDir
      SystemPrompt: string
      Model: string            // exact model id, e.g. claude-sonnet-5-5
      Effort: string           // low | medium | high | xhigh | max
      Schema: string           // JSON schema for structured output
      TranscriptPath: string
      TimeoutMinutes: int }

type ToolCall = { Name: string; Input: string }

type RunResult =
    { Label: string
      Ok: bool
      Output: JsonElement option
      Error: string option
      Model: string
      Effort: string
      ReportedModels: string list
      InputTokens: int64
      CacheReadTokens: int64
      CacheCreationTokens: int64
      OutputTokens: int64
      ThinkingTokens: int64
      CostUsd: float
      DurationMs: int64
      NumTurns: int
      ToolCalls: ToolCall list
      StartedAt: DateTime
      FinishedAt: DateTime }

let private tryProp (name: string) (e: JsonElement) =
    if e.ValueKind = JsonValueKind.Object then
        match e.TryGetProperty name with
        | true, v when v.ValueKind <> JsonValueKind.Null -> Some v
        | _ -> None
    else None

let private int64Of name e =
    tryProp name e |> Option.bind (fun v -> if v.ValueKind = JsonValueKind.Number then Some (v.GetInt64()) else None) |> Option.defaultValue 0L

let private parseLine (line: string) =
    try Some (JsonDocument.Parse(line).RootElement.Clone()) with _ -> None

/// Tool calls made by the session, from assistant messages in the transcript.
let toolCalls (events: JsonElement list) : ToolCall list =
    events
    |> List.filter (fun e -> tryProp "type" e |> Option.exists (fun t -> t.GetString() = "assistant"))
    |> List.collect (fun e ->
        tryProp "message" e
        |> Option.bind (tryProp "content")
        |> Option.map (fun c -> if c.ValueKind = JsonValueKind.Array then c.EnumerateArray() |> List.ofSeq else [])
        |> Option.defaultValue []
        |> List.filter (fun b -> tryProp "type" b |> Option.exists (fun t -> t.GetString() = "tool_use"))
        |> List.map (fun b ->
            { Name = tryProp "name" b |> Option.map (fun v -> v.GetString()) |> Option.defaultValue ""
              Input = tryProp "input" b |> Option.map (fun v -> v.GetRawText()) |> Option.defaultValue "" }))

/// Pure: interpret a finished transcript.
let interpret (spec: RunSpec) (started: DateTime) (finished: DateTime) (exitCode: int) (stderr: string) (lines: string list) : RunResult =
    let events = lines |> List.choose parseLine
    let result = events |> List.tryFindBack (fun e -> tryProp "type" e |> Option.exists (fun t -> t.GetString() = "result"))
    let usage = result |> Option.bind (tryProp "usage")
    let u name = usage |> Option.map (int64Of name) |> Option.defaultValue 0L
    let thinking =
        usage |> Option.bind (tryProp "output_tokens_details") |> Option.map (int64Of "thinking_tokens") |> Option.defaultValue 0L
    let output = result |> Option.bind (tryProp "structured_output")
    let models =
        result |> Option.bind (tryProp "modelUsage")
        |> Option.map (fun m -> m.EnumerateObject() |> Seq.map (fun p -> p.Name) |> List.ofSeq)
        |> Option.defaultValue []
    let isError = result |> Option.bind (tryProp "is_error") |> Option.exists (fun v -> v.ValueKind = JsonValueKind.True)
    let error =
        match exitCode, output, isError with
        | 0, Some _, false -> None
        | c, _, _ ->
            let sub = result |> Option.bind (tryProp "subtype") |> Option.map (fun v -> v.GetString()) |> Option.defaultValue "no-result"
            Some (sprintf "exit=%d subtype=%s stderr=%s" c sub (stderr.Trim() |> fun s -> if s.Length > 300 then s.Substring(0, 300) else s))
    { Label = spec.Label
      Ok = error.IsNone
      Output = output
      Error = error
      Model = spec.Model
      Effort = spec.Effort
      ReportedModels = models
      InputTokens = u "input_tokens"
      CacheReadTokens = u "cache_read_input_tokens"
      CacheCreationTokens = u "cache_creation_input_tokens"
      OutputTokens = u "output_tokens"
      ThinkingTokens = thinking
      CostUsd = result |> Option.bind (tryProp "total_cost_usd") |> Option.map (fun v -> v.GetDouble()) |> Option.defaultValue 0.0
      DurationMs = result |> Option.map (int64Of "duration_ms") |> Option.defaultValue 0L
      NumTurns = result |> Option.map (int64Of "num_turns" >> int) |> Option.defaultValue 0
      ToolCalls = toolCalls events
      StartedAt = started
      FinishedAt = finished }

let private args (spec: RunSpec) =
    [ "-p"; spec.Prompt
      "--model"; spec.Model
      "--effort"; spec.Effort
      "--tools"; "Read"
      "--disallowedTools"; "mcp__claude-in-chrome"
      "--system-prompt"; spec.SystemPrompt
      "--setting-sources"; ""
      "--mcp-config"; """{"mcpServers":{}}"""
      "--strict-mcp-config"
      "--json-schema"; spec.Schema
      "--output-format"; "stream-json"
      "--verbose" ]

let private killQuietly (p: Process) =
    try p.Kill(true) with _ -> ()

/// Effectful edge: run one isolated session and preserve its transcript.
let run (spec: RunSpec) : Async<RunResult> =
    async {
        let psi = ProcessStartInfo("claude")
        args spec |> List.iter psi.ArgumentList.Add
        psi.WorkingDirectory <- spec.WorkDir
        psi.RedirectStandardOutput <- true
        psi.RedirectStandardError <- true
        psi.RedirectStandardInput <- true
        psi.UseShellExecute <- false
        let started = DateTime.UtcNow
        use p = new Process(StartInfo = psi)
        p.Start() |> ignore
        p.StandardInput.Close()
        let outTask = p.StandardOutput.ReadToEndAsync()
        let errTask = p.StandardError.ReadToEndAsync()
        // Wait asynchronously: a blocking WaitForExit starves the thread pool when many runs are queued.
        use cts = new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(float spec.TimeoutMinutes))
        let! finishedInTime =
            async {
                try
                    do! p.WaitForExitAsync(cts.Token) |> Async.AwaitTask
                    return true
                with _ -> return false
            }
        if not finishedInTime then killQuietly p
        let! stdout = Async.AwaitTask outTask
        let! stderr = Async.AwaitTask errTask
        let finished = DateTime.UtcNow
        Directory.CreateDirectory(Path.GetDirectoryName spec.TranscriptPath) |> ignore
        File.WriteAllText(spec.TranscriptPath, stdout)
        let code = if finishedInTime then p.ExitCode else -1
        let err = if finishedInTime then stderr else "timeout " + stderr
        return interpret spec started finished code err (stdout.Split('\n') |> Array.filter (fun l -> l.Trim() <> "") |> List.ofArray)
    }

/// Run many specs with bounded concurrency, preserving input order.
let runAll (concurrency: int) (onDone: RunResult -> unit) (specs: RunSpec list) : RunResult list =
    let gate = new System.Threading.SemaphoreSlim(concurrency)
    let safeRun (s: RunSpec) =
        async {
            try return! run s
            with ex -> return { interpret s DateTime.UtcNow DateTime.UtcNow -2 ex.Message [] with Error = Some ex.Message }
        }
    let guarded (s: RunSpec) =
        async {
            do! gate.WaitAsync() |> Async.AwaitTask
            let! r = safeRun s
            gate.Release() |> ignore
            onDone r
            return r
        }
    specs
    |> List.map guarded
    |> Async.Parallel
    |> Async.RunSynchronously
    |> List.ofArray

let resultToJson (r: RunResult) : string =
    let o = Collections.Generic.Dictionary<string, obj>()
    o.["label"] <- r.Label
    o.["ok"] <- r.Ok
    o.["error"] <- (r.Error |> Option.toObj)
    o.["model"] <- r.Model
    o.["effort"] <- r.Effort
    o.["reportedModels"] <- r.ReportedModels
    o.["inputTokens"] <- r.InputTokens
    o.["cacheReadTokens"] <- r.CacheReadTokens
    o.["cacheCreationTokens"] <- r.CacheCreationTokens
    o.["outputTokens"] <- r.OutputTokens
    o.["thinkingTokens"] <- r.ThinkingTokens
    o.["costUsd"] <- r.CostUsd
    o.["durationMs"] <- r.DurationMs
    o.["numTurns"] <- r.NumTurns
    o.["toolCalls"] <- (r.ToolCalls |> List.map (fun t -> dict [ "name", box t.Name; "input", box t.Input ]))
    o.["startedAt"] <- r.StartedAt.ToString("o")
    o.["finishedAt"] <- r.FinishedAt.ToString("o")
    o.["output"] <- (r.Output |> Option.map (fun e -> box e) |> Option.toObj)
    JsonSerializer.Serialize(o, JsonSerializerOptions(WriteIndented = true))
