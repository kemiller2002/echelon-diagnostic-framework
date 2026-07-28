"use client";

import { useState } from "react";

const stages = [
  { verb: "Define", label: "System context", question: "Within what declared context is this diagnosis valid?", output: "Primary system · Context · Focus", mistake: "Treating scope as self-evident" },
  { verb: "Observe", label: "Outcome & manifestations", question: "What happened, and what signals show it?", output: "Observable outcome · Measurable signals", mistake: "Embedding interpretation in the facts" },
  { verb: "Explain", label: "Origin network", question: "What interacting contributors made this possible?", output: "Technical · Human · Organizational origins", mistake: "Stopping at one root cause" },
  { verb: "Explain", label: "Propagation", question: "How did effects move through the system?", output: "Physical · Informational · Organizational paths", mistake: "Listing causes without modeling behavior" },
  { verb: "Validate", label: "Evidence & confidence", question: "What supports this explanation—and what contradicts it?", output: "Evidence · Gaps · Confidence", mistake: "Confusing interpretation with evidence" },
  { verb: "Influence", label: "Control points", question: "Where can we most effectively influence what happens next?", output: "Ranked points of leverage", mistake: "Assuming the origin is the best intervention" },
  { verb: "Validate", label: "Diagnostic sufficiency", question: "Do we understand enough for the next responsible action?", output: "Act · Investigate · Reframe", mistake: "Waiting for certainty—or acting on instinct" },
];

export default function DiagnosticFlow() {
  const [active, setActive] = useState(0);
  const stage = stages[active];
  return (
    <div className="flow-shell">
      <div className="flow-rail" role="tablist" aria-label="EDF diagnostic stages">
        {stages.map((item, index) => (
          <button key={item.label} role="tab" aria-selected={active === index} onClick={() => setActive(index)}>
            <span>{String(index + 1).padStart(2, "0")}</span>{item.label}
          </button>
        ))}
      </div>
      <div className="flow-detail" role="tabpanel">
        <p className="eyebrow">{stage.verb}</p>
        <h3>{stage.label}</h3>
        <blockquote>{stage.question}</blockquote>
        <dl>
          <div><dt>Produces</dt><dd>{stage.output}</dd></div>
          <div><dt>Common failure</dt><dd>{stage.mistake}</dd></div>
        </dl>
        <div className="flow-controls">
          <button disabled={active === 0} onClick={() => setActive(active - 1)}>Previous</button>
          <span>{active + 1} / {stages.length}</span>
          <button disabled={active === stages.length - 1} onClick={() => setActive(active + 1)}>Next stage</button>
        </div>
      </div>
    </div>
  );
}
