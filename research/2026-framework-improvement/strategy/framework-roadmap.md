# EDF framework roadmap

Separates **what the evidence supports now** from **proposed future research**.

## Now — supported by evidence from this programme

1. **Freeze EDF v0.3; correct claims.** State plainly: on synthetic organizational cases with curated evidence, EDF v0.3 performs no better than a general-purpose AI prompt (B − A +1.4 [−0.7, 3.1]), a strong established method (C − B −0.6 [−2.0, 0.7]) or, by the preregistered margin, matched structure. It is a coherent representational grammar without demonstrated diagnostic advantage.
2. **Make OD1 (fixed) the evaluation gate.** No EDF version or claim is promoted without a preregistered, held-out, deterministic, independently re-scored comparison against the general-AI and established-method baselines.
3. **Clarity.** Keep the sequential handoff: EDF diagnosis, then a Clarity decision record for consequential or low-reversibility decisions. Do not embed Clarity in diagnosis.
4. **Records.** Update `docs/edf/validation/evidence-ledger.md` and the hypothesis registry with EX-EDF-001 and EX-EDF-2026-OD1. Correct the A003 journal inconsistency (CON-05).

## Next (1–2 research cycles) — proposed, not yet validated

5. **OD2 benchmark.**
   - Evidence acquisition is part of the task.
   - Decisive evidence is absent unless requested; noisy, longer files; randomized request menus.
   - Cross-provider analyzers and case authors.
   - Sharper operational definitions of premature vs harmful and contradicted vs association-only.
6. **Candidate "EDF-lite" profile** to confirm or reject on OD2. Its components:
   - Decision gates (CI-01): harm under every live hypothesis; "no intervention warranted" as a valid sufficient conclusion; next evidence chosen for discriminating power.
   - Typed causal edges (CI-02) with loop and delay annotation.
   - A sequential evidence-acquisition loop with a stopping rule (CI-03).
   - Optionally, a KT-style contrast step (CI-10) and organizational guards (CI-04) if their targeted tests succeed.
   - EDF's descriptive scaffolding (System Context triple, origin taxonomy, four-criterion ranking) becomes optional output, retained only if the human study shows communication value.
7. **Human-analyst study.** The decisive test of whether EDF's vocabulary helps people (comprehension, reviewability, time to actionable finding) even though it does not help models.

## Later — requires real organizations and authorization

8. **Intervention-effectiveness evaluation** (stepped-wedge or matched comparison), separating selection quality from execution quality.
9. **Longitudinal assessment** only with a defined purpose, aggregate data, consent, an accountable owner and drift checks. Never as default surveillance.

## Explicit non-goals

- Making EDF more elaborate. The evidence points the other way: long generic procedure hurt, and shorter rules did as well or better.
- Industry-specific EDF variants. No evidence of differential performance by industry; the OOD cases did not collapse.
- Quantitative confidence scoring by multiplying criteria (false precision).
