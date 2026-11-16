# Held-out policy (EX-EDF-2026-OD1)

1. Cases are split into `dev` (10) and `heldout` (12) **before** authoring. Out-of-distribution class J exists only in held-out.
2. Dev and held-out cases are authored by different agent instances from separate briefs; no author sees a variant prompt (none exist at authoring time).
3. After adversarial review, every case and truth file is hashed into `benchmarks/frozen-cases.sha256` and committed before the first analyzer run.
4. Dev analyzer outputs may be inspected to build the failure taxonomy and design experimental variants.
5. Held-out analyzer outputs are generated only after every experimental variant prompt is frozen (hashed into `experiments/frozen-prompts.sha256`) and committed.
6. Nothing is edited after a held-out run. Invalid outputs are terminal. Any post-hoc analysis is labeled post-hoc.
7. Limitation: the experimenter (the orchestrating agent) can read held-out cases; it commits not to tune variants on them, and the commit order in Git is the audit trail.
