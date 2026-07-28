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
          <a href="#framework">Framework</a><a href="#example">Example</a><a href="#research">Research</a><a href="#applications">Applications</a>
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
            <a href="https://github.com/kevinmiller/Echelon-diagnostic-framework/blob/main/docs/edf/specification.md">Specification v0.3 <span>↗</span></a>
            <a href="https://github.com/kevinmiller/Echelon-diagnostic-framework/blob/main/docs/edf/validation/reproducibility-findings.md">Reproducibility findings <span>↗</span></a>
            <a href="https://github.com/kevinmiller/Echelon-diagnostic-framework/blob/main/docs/edf/validation/evidence-ledger.md">Evidence ledger <span>↗</span></a>
          </div>
        </section>

        <section id="applications" className="applications band">
          <div className="section-intro"><div><p className="eyebrow">WHERE EDF HELPS</p><h2>For systems where the obvious answer is rarely the whole answer.</h2></div><p>Use the smallest EDF level that reaches diagnostic sufficiency—from a rapid card to a complex multi-analyst investigation.</p></div>
          <div className="application-grid">
            {["Software incidents","Product strategy","AI adoption","Digital transformation","Operations","Technology modernization"].map((x,i)=><article key={x}><span>0{i+1}</span><h3>{x}</h3><p>{["Trace how technical, process, and governance conditions combined.","Separate market signals from the organization’s preferred story.","Diagnose capability, workflow, incentive, and data constraints.","Find why local progress fails to produce system-level change.","Explain recurring variance beyond the nearest process failure.","Distinguish aging technology from the systems that keep it in place."][i]}</p></article>)}
          </div>
        </section>

        <section id="start" className="start section">
          <p className="eyebrow">BEGIN WITH REALITY</p><h2>Before proposing a solution,<br />write down what you actually know.</h2>
          <div className="starter-card">
            <div><span>01</span><p>PRIMARY SYSTEM</p><strong>What system are you trying to understand?</strong></div>
            <div><span>02</span><p>OUTCOME</p><strong>What happened—stated without interpretation?</strong></div>
            <div><span>03</span><p>EVIDENCE</p><strong>What supports your explanation? What contradicts it?</strong></div>
          </div>
          <a className="button primary" href="#framework">Walk through the diagnostic grammar <span>↑</span></a>
        </section>
      </main>

      <footer><div className="brand"><span>EDF</span><small>Echelon Diagnostic Framework</small></div><p>A working research framework for understanding systems before changing them.</p><div><a href="#framework">Framework</a><a href="#research">Research</a><a href="#top">Back to top ↑</a></div><small>SPECIFICATION v0.3 · WORKING DRAFT</small></footer>
    </>
  );
}
