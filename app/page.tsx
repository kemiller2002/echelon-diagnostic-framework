import DiagnosticFlow from "./DiagnosticFlow";

const evidence = [
  { state: "SUPPORTED", confidence: "HIGH", title: "One grammar can analyze failure and success", detail: "Challenger, Boeing, Apollo 11, Pixar, and Toyota produced comparable diagnostic structures.", tone: "supported" },
  { state: "PROVISIONAL", confidence: "MED–HIGH", title: "EDF improves control-point identification", detail: "Multiple validation cases support the claim; quantitative scoring is still needed.", tone: "provisional" },
  { state: "OPEN", confidence: "MEDIUM", title: "EDF scales to everyday operational problems", detail: "A restaurant case is encouraging, but more simple cases are required.", tone: "open" },
];

export default function Home() {
  return (
    <>
      <header className="site-header">
        <a className="brand" href="#top" aria-label="EDF home"><span>EDF</span><small>Echelon Diagnostic Framework</small></a>
        <nav aria-label="Primary navigation">
          <a href="#framework">Framework</a><a href="#how-to">How to use it</a><a href="#examples">Examples</a><a href="#research">Research</a>
        </nav>
        <a className="header-cta" href="#start">Start a diagnosis <span>↗</span></a>
      </header>

      <main id="top">
        <section className="hero">
          <div className="hero-copy">
            <p className="eyebrow"><span className="status-dot" /> Working draft · Specification v0.3</p>
            <h1>Understand the system.<br /><em>Then change it.</em></h1>
            <p className="lede">A structured way to explain how complex systems produce outcomes—and find the points where intervention has the most leverage.</p>
            <div className="hero-actions"><a className="button primary" href="#framework">Explore the framework <span>↓</span></a><a className="button text" href="#research">Examine the evidence <span>↗</span></a></div>
          </div>
          <aside className="diagnostic-card" aria-label="A diagnostic contrast">
            <p className="card-label">THE DEFAULT PATTERN</p>
            <div className="bad-chain"><span>Observation</span><i>→</i><span>Assumption</span><i>→</i><strong>Solution</strong></div>
            <div className="divider"><span>EDF INTERRUPTS HERE</span></div>
            <div className="good-chain">
              <span>Observation</span><i>↓</i><span>Evidence</span><i>↓</i><span>Competing explanations</span><i>↓</i><strong>Understanding</strong><i>↓</i><b>Action</b>
            </div>
          </aside>
        </section>

        <section className="problem band">
          <p className="eyebrow">THE PROBLEM BENEATH THE PROBLEM</p>
          <div className="split-heading"><h2>Organizations rarely fail for lack of solutions.</h2><p>They fail because they become certain too early. A visible symptom becomes “the problem.” A plausible story becomes “the cause.” Activity begins before understanding does.</p></div>
          <div className="failure-grid">
            <article><span>01</span><h3>Symptoms masquerade as causes</h3><p>The place where pain appears is often far from where it began.</p></article>
            <article><span>02</span><h3>One story crowds out alternatives</h3><p>The first coherent explanation earns confidence it has not yet earned.</p></article>
            <article><span>03</span><h3>Origins are mistaken for leverage</h3><p>Where a cause began may not be where the system can best be changed.</p></article>
          </div>
        </section>

        <section id="framework" className="framework section">
          <div className="section-intro"><div><p className="eyebrow">THE DIAGNOSTIC GRAMMAR</p><h2>Seven moves from signal to responsible action.</h2></div><p>EDF is not a checklist that promises certainty. It is a visible reasoning structure: every step produces an artifact that can be challenged, revised, and traced.</p></div>
          <DiagnosticFlow />
        </section>

        <section id="how-to" className="how-to band">
          <div className="section-intro"><div><p className="eyebrow">HOW TO USE EDF</p><h2>Start small. Make the reasoning visible.</h2></div><p>A useful diagnosis is not the longest one. It is the smallest evidence-backed model that supports the next responsible action.</p></div>
          <div className="level-grid">
            <article><div><span>EDF–0</span><small>RAPID</small></div><h3>One local issue.<br />Low disagreement.</h3><p>Use a quick card when the evidence is direct and one action can safely test the explanation.</p><b>Example: a conference-room light will not turn on.</b></article>
            <article className="featured"><div><span>EDF–1</span><small>STANDARD</small></div><h3>Several explanations.<br />Consequences matter.</h3><p>Make alternatives, evidence, unknowns, and control-point ranking explicit.</p><b>Example: a home cooling system hums but does not start.</b></article>
            <article><div><span>EDF–2</span><small>COMPLEX</small></div><h3>Interacting systems.<br />Distributed control.</h3><p>Map technical, human, organizational, and contextual origins and their propagation paths.</p><b>Example: 737 MAX design, certification, training, and safety.</b></article>
          </div>
          <div className="field-guide">
            <div className="field-guide-heading"><p className="eyebrow">A PRACTICAL PASS</p><h3>Complete one row at a time.</h3><p>Do not polish the story as you go. Record contradictions and unknowns; they are diagnostic information.</p></div>
            <ol>
              <li><span>01</span><div><h4>Frame the system</h4><p>Name the primary system, the larger context shaping it, and the narrow focus.</p></div><blockquote>“Checkout flow, within the mobile storefront, focused on payment completion.”</blockquote></li>
              <li><span>02</span><div><h4>State the outcome</h4><p>Write what happened without causal language, blame, or a proposed fix.</p></div><blockquote>“Payment completion fell from 71% to 54% after Tuesday’s release.”</blockquote></li>
              <li><span>03</span><div><h4>Build competing explanations</h4><p>List interacting origins and trace how each could reach the manifestation.</p></div><blockquote>“SDK change → timeout → retry loop → abandoned checkout.”</blockquote></li>
              <li><span>04</span><div><h4>Test with evidence</h4><p>Record support, contradiction, source quality, and what remains unknown.</p></div><blockquote>“Logs support timeouts; web checkout did not decline.”</blockquote></li>
              <li><span>05</span><div><h4>Rank control points</h4><p>Compare influence, controllability, cost, and confidence.</p></div><blockquote>“Rollback outranks retraining: faster, reversible, directly testable.”</blockquote></li>
              <li><span>06</span><div><h4>Check sufficiency</h4><p>Ask whether you know enough for the next responsible action—not whether every uncertainty is gone.</p></div><blockquote>“Yes for rollback; no for declaring the incident fully explained.”</blockquote></li>
            </ol>
          </div>
          <div className="use-rules">
            <p><span>Expand</span> when causes are disputed, consequences rise, or control differs from origin.</p>
            <p><span>Stop</span> when the next action is responsible, testable, and matched to stated confidence.</p>
            <p><span>Return</span> after action to see whether the manifestation and intended control point changed.</p>
          </div>
        </section>

        <section id="examples" className="examples section">
          <div className="section-intro"><div><p className="eyebrow">THREE SCALES · ONE GRAMMAR</p><h2>See what a finished diagnosis looks like.</h2></div><p>The fields stay stable as complexity grows. What changes is the number of plausible origins, the length of propagation, and the rigor needed to rank control.</p></div>
          <div className="example-stack">
            <article><header><div><span>EDF–0</span><small>EVERYDAY · HIGH CONFIDENCE</small></div><h3>Conference-room lighting</h3><p>A fast diagnosis where direct testing is enough.</p></header><div className="example-read"><dl><div><dt>Outcome</dt><dd>Lights did not turn on during use.</dd></div><div><dt>Evidence</dt><dd>A replacement bulb restored lighting.</dd></div><div><dt>Origin → propagation</dt><dd>Failed bulb → no illumination when power was applied.</dd></div></dl><aside><p>TOP CONTROL POINT</p><strong>Replace the bulb</strong><small>High influence · low cost · direct evidence</small></aside></div></article>
            <article><header><div><span>EDF–1</span><small>OPERATIONAL · MEDIUM CONFIDENCE</small></div><h3>Residential cooling startup</h3><p>Several components could produce the symptom, so alternatives matter.</p></header><div className="example-read"><dl><div><dt>Outcome</dt><dd>Indoor temperature stayed above target during cooling demand.</dd></div><div><dt>Evidence</dt><dd>Thermostat called for cooling; outdoor unit hummed; capacitor measured out of range.</dd></div><div><dt>Origin → propagation</dt><dd>Failed capacitor → motor could not start → cooling delivery stopped.</dd></div></dl><aside><p>TOP CONTROL POINT</p><strong>Replace capacitor, then verify startup</strong><small>The compressor remains an explicit unknown</small></aside></div></article>
            <article><header><div><span>EDF–2</span><small>COMPLEX · MED–HIGH CONFIDENCE</small></div><h3>Apollo 11 success</h3><p>EDF can preserve exceptional performance, not only explain failure.</p></header><div className="example-read"><dl><div><dt>Outcome</dt><dd>The mission landed on the moon and returned safely.</dd></div><div><dt>Origin network</dt><dd>Engineering rigor, testing culture, mission clarity, and systems integration reinforced one another.</dd></div><div><dt>Propagation</dt><dd>Clear goals and rigorous testing shaped design, preparation, and execution.</dd></div></dl><aside><p>TOP CONTROL POINT</p><strong>Preserve testing discipline</strong><small>Protect the conditions that repeatedly sustain success</small></aside></div></article>
          </div>
          <p className="example-lesson"><b>Notice the pattern:</b> the lighting case permits a direct fix; the cooling case preserves an unknown and verifies after action; the Apollo case treats successful conditions as a network worth protecting.</p>
        </section>

        <section id="example" className="example band">
          <div className="section-intro"><div><p className="eyebrow">WORKED EXAMPLE · CHALLENGER</p><h2>The O-ring failed.<br />The diagnosis cannot stop there.</h2></div><p>A concise illustration of the difference between a physical origin and a high-leverage control point.</p></div>
          <div className="case-map">
            <div className="case-column"><p>MANIFESTATION</p><strong>Loss of vehicle<br />73 seconds after launch</strong><small>Observable outcome</small></div>
            <div className="case-arrow">→</div>
            <div className="case-column network"><p>ORIGIN NETWORK</p><ul><li>Cold-sensitive O-ring behavior</li><li>Risk normalization</li><li>Schedule pressure</li><li>Fragmented engineering authority</li></ul><small>Interacting contributors</small></div>
            <div className="case-arrow">→</div>
            <div className="case-column control"><p>CONTROL POINT</p><strong>Engineering authority over launch decisions</strong><small>Higher leverage than redesign alone</small></div>
          </div>
          <p className="case-note"><b>Why this matters:</b> “O-ring failure” identifies a physical origin. It does not explain why known warning signals failed to stop the launch—or where future outcomes could be most effectively influenced.</p>
        </section>

        <section id="research" className="research section">
          <div className="section-intro"><div><p className="eyebrow">EVIDENCE, NOT CERTAINTY THEATER</p><h2>What the research supports.<br />And what it does not—yet.</h2></div><p>EDF separates public claims by evidence state. The framework is frozen at v0.3 while validation continues; findings do not silently rewrite the method.</p></div>
          <div className="evidence-list">
            {evidence.map(item => <article key={item.title} className={item.tone}><div><span>{item.state}</span><small>CONFIDENCE {item.confidence}</small></div><h3>{item.title}</h3><p>{item.detail}</p><b>→</b></article>)}
          </div>
          <div className="research-facts">
            <div><strong>6</strong><span>reproducibility cases under Validation Protocol v1.1</span></div>
            <div><strong>0</strong><span>independent human analyst studies completed</span></div>
            <div><strong>3</strong><span>comparative methods examined: RCA, FMEA, FTA</span></div>
            <div><strong>v0.3</strong><span>current frozen specification</span></div>
          </div>
          <p className="disclosure"><b>Known limitation.</b> Current reproducibility results are very high across six cases, but remain provisional until tested with independent human analysts. Speed, learning curve, visualization, and quantitative metrics are active gaps.</p>
          <div className="source-links" aria-label="Primary research sources">
            <p>Trace the claims</p>
            <a href="https://github.com/kemiller2002/Echelon-diagnostic-framework/blob/main/docs/edf/specification.md">Specification v0.3 <span>↗</span></a>
            <a href="https://github.com/kemiller2002/Echelon-diagnostic-framework/blob/main/docs/edf/validation/reproducibility-findings.md">Reproducibility findings <span>↗</span></a>
            <a href="https://github.com/kemiller2002/Echelon-diagnostic-framework/blob/main/docs/edf/validation/evidence-ledger.md">Evidence ledger <span>↗</span></a>
          </div>
        </section>

        <section id="applications" className="applications band">
          <div className="section-intro"><div><p className="eyebrow">WHERE EDF HELPS</p><h2>For systems where the obvious answer is rarely the whole answer.</h2></div><p>Use the smallest EDF level that reaches diagnostic sufficiency—from a rapid card to a complex multi-analyst investigation.</p></div>
          <div className="application-grid">
            {["Software incidents","Product strategy","AI adoption","Digital transformation","Operations","Technology modernization"].map((x,i)=><article key={x}><span>0{i+1}</span><h3>{x}</h3><p>{["Trace how technical, process, and governance conditions combined.","Separate market signals from the organization’s preferred story.","Diagnose capability, workflow, incentive, and data constraints.","Find why local progress fails to produce system-level change.","Explain recurring variance beyond the nearest process failure.","Distinguish aging technology from the systems that keep it in place."][i]}</p></article>)}
          </div>
        </section>

        <section id="start" className="start section">
          <p className="eyebrow">YOUR FIRST EDF–0</p><h2>Before proposing a solution,<br />write down what you actually know.</h2>
          <p className="start-lede">Choose one current, low-risk problem. Keep the first pass to a single page and use plain, observable language.</p>
          <div className="starter-card">
            <div><span>01</span><p>SYSTEM CONTEXT</p><strong>Primary system · larger context · narrow focus</strong></div>
            <div><span>02</span><p>OUTCOME</p><strong>What happened—stated without interpretation?</strong></div>
            <div><span>03</span><p>ORIGIN + PATH</p><strong>What may have contributed, and how did its effect travel?</strong></div>
            <div><span>04</span><p>EVIDENCE</p><strong>What supports the model? What contradicts it? What is unknown?</strong></div>
            <div><span>05</span><p>CONTROL</p><strong>Which intervention ranks highest by influence, control, cost, and confidence?</strong></div>
            <div><span>06</span><p>SUFFICIENCY</p><strong>Do you know enough for the next responsible, testable action?</strong></div>
          </div>
          <div className="start-actions"><a className="button primary" href="https://github.com/kemiller2002/Echelon-diagnostic-framework/blob/main/docs/edf/templates/edf-0-quick-card.md">Open the EDF–0 quick card <span>↗</span></a><a className="button text" href="#how-to">Review the field guide <span>↑</span></a></div>
        </section>
      </main>

      <footer><div className="brand"><span>EDF</span><small>Echelon Diagnostic Framework</small></div><p>A working research framework for understanding systems before changing them.</p><div><a href="#framework">Framework</a><a href="#research">Research</a><a href="#top">Back to top ↑</a></div><small>SPECIFICATION v0.3 · WORKING DRAFT</small></footer>
    </>
  );
}
