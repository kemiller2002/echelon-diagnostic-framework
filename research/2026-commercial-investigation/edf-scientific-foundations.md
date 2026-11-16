# Scientific foundations for EDF-like diagnostic approaches (Workstream B)

Retrieval date for all fetched material: 2026-11-16.

**Retrieval limits (read first):** The shared web-search budget was exhausted after roughly 25 searches in this workstream; several publisher pages returned HTTP 403 (BMJ Quality & Safety full texts, Birmingham repository PDF, PubMed abstract pages) or redirect-only stubs (Elsevier). Where a source was retrieved only via search-result snippets or a secondary summary (AHRQ PSNet), that is stated. Classic works marked **[not retrieved]** are cited from background knowledge only for orientation; no statistics are attributed to them, and they should be verified before external use.

Rating scale: **Empirically supported** (multiple controlled/comparative studies with consistent results) / **Mixed** (some controlled evidence, inconsistent or context-limited) / **Theoretical** (conceptual, case-based, or practitioner evidence; little or no controlled testing).

---

## 1. Root cause analysis (RCA) in healthcare — closest institutional analogue

| Evidence | What it shows | Source |
|---|---|---|
| Martin-Delgado et al. 2020, systematic review, 21 studies (MEDLINE/Scopus/EMBASE to Dec 2018) | Only 2 studies (together 54 RCAs) showed "to some extent" that RCAs improved safety; **no included study directly measured whether RCA reduced future adverse events**; recommendations often targeted active errors rather than latent causes; implementation hampered by time, resources, committee authority; lack of follow-up. Conclusion: RCA is useful "for the identification of the remote and immediate causes of safety incidents" "but not for implementing effective measures to prevent their recurrence." | SRC-B-029 (PMC full text fetched) |
| Kellogg et al. 2017, BMJ Qual Saf 26(5):381–387; 302 RCAs over 8 years, one US academic medical centre | 106 RCAs proposed solutions; most common solutions were weaker actions (education, process changes, policy enforcement); serious events (e.g., retained surgical sponges) recurred despite repeated RCAs. | SRC-B-031 (search snippets + PSNet; full text 403) |
| Trbovich & Shojania 2017 editorial (same issue) | Person-based corrective actions (training, policy reinforcement, warnings) are less effective than system-level changes; corrective actions should be treated as hypotheses to test. | SRC-B-053 (search snippet only) |
| Peerally, Carr, Waring, Dixon-Woods 2017, BMJ Qual Saf 26(5):417–422 | Identifies eight problems with RCA in healthcare, including the reductionist implication of a single "root" cause, poor analysis quality, and insufficient feedback; argues RCA has been applied without attention to the conditions that make it work. | SRC-B-028 (search + PSNet; full text 403) |
| Mira et al. 2019, Rev Panam Salud Publica 43:e51 | Cause identification usually succeeds; action implementation often fails because it is handed to a different group that did not do the analysis; recommends involving managers with decision authority. | SRC-B-030 (PMC fetched) |

**Rating: Mixed for cause identification; weak/largely unsupported for preventing recurrence.** The failure point is the **diagnosis→action→verification link and action strength**, not the representation of causes.

**Transfer to EDF (CONCLUSION):** EDF's distinctive moves — network rather than single root cause; control points that differ from origins; ranked interventions; "Understand again" verification loop — map directly onto the RCA critique (Peerally's anti-reductionism; Kellogg/Trbovich on weak actions; Mira on implementation). That is a credible *design rationale*. But the RCA literature shows the bottleneck is organisational (authority, follow-up, resourcing, action strength), which a diagnostic template does not by itself change. EDF has no action-strength hierarchy (cf. person-based vs system-level actions), no ownership/authority enforcement beyond an "Owner" field, and no follow-up evidence. HYPOTHESIS: EDF's value, if any, would come from the control-point and verification parts, which are exactly the parts it has not tested.

## 2. Five Whys

Card 2017, BMJ Qual Saf 26(8):671–677: popularity "is not the result of any evidence that it is effective"; oversimplifies complex problems, forces a single causal path, may miss important causes; useful mainly as a teaching tool (SRC-B-032, via search snippets and PSNet; full text not retrieved). Secondary sources report that different users reach different causes for the same problem (low reliability) — secondary, unverified.
**Rating: Theoretical / critiqued; no supporting effectiveness evidence found.** EDF's own CFCV-001 makes the same critique but its comparison was not executed (REPO EV-EDF-003).

## 3. Ishikawa (fishbone) diagrams, Fault Tree Analysis, FMEA
- Not retrieved this session (search budget). Background: these are standard quality/reliability tools with long engineering use; FTA is formally quantifiable when failure probabilities are known; controlled evidence that fishbone diagrams improve diagnostic accuracy or outcomes is, to my knowledge, sparse. **Rating: Theoretical for Ishikawa (widely used, little outcome evidence); FTA/FMEA established engineering practice with formal semantics but limited comparative outcome evidence in organisational settings** — LOW confidence, verify.
- Relevance: FTA/bow-tie already separate causes, propagation and barriers; EDF's HY-2026-A016 (separate causal mechanisms from detection/protective controls) is a known concept in bow-tie/barrier analysis.

## 4. Systems approaches: STAMP/CAST, causal loop diagrams, system dynamics, group model building
- STAMP/CAST (Leveson, *Engineering a Safer World*, 2011) [not retrieved] models accidents as inadequate control in a hierarchical control structure — the closest academic analogue to EDF's "control points ≠ origins". EDF does not cite it.
- System dynamics / causal loop diagrams (Sterman, *Business Dynamics*, 2000) [not retrieved]. Group model building effectiveness review: Scott, Cavana & Cameron 2016, EJOR (doi 10.1016/j.ejor.2015.06.078) — retrieval failed (redirect stub); content not verified.
- **Rating: Theoretical-to-mixed (LOW confidence; not retrieved).** RQ-B-004 records the need to retrieve comparative CAST-vs-RCA studies.

## 5. Organisational diagnosis models
| Model | Evidence found | Rating |
|---|---|---|
| Weisbord Six-Box (1976) | Only small applied studies (e.g., Iranian university medical sciences, Pakistani hospitals) reporting Cronbach's alpha ~0.76–0.86 and expert face validity; no factor-analytic or predictive validation found. | Theoretical (SRC-B-037, search-level) |
| Burke-Litwin (1992) | Exploratory utility study (Martins & Coetzee 2009, SAJHRM) and a South African questionnaire psychometric validation; no verified test of the model's causal paths found; secondary sites' claims of confirmed causal structure could not be traced to a primary source. | Theoretical-to-mixed (SRC-B-038) |
| Nadler-Tushman congruence (1980) [not retrieved] | Not checked. | Theoretical (unverified) |
| McKinsey 7S (Waterman, Peters & Phillips 1980) [not retrieved] | Not checked. | Theoretical (unverified) |
| Falletta review of diagnostic models (2005/2008 white paper) | Describes models and proposes path analysis/SEM to test them; found only on unofficial uploads; does not itself provide validation. A separate paper notes "little attention has been given to the diagnostic process itself." | — (SRC-B-039) |

CONCLUSION: The organisational-diagnosis tradition offers **content models** (what to look at) with weak validation; EDF offers a **process grammar** (how to reason from outcome to control point). That is a genuine positioning difference, but EDF is no better validated than these incumbents, which have 30–50 years of practitioner familiarity.

## 6. Diagnostic error research in medicine
- Graber, Franklin & Gordon 2005, Arch Intern Med 165:1493–1499: 100 diagnostic errors; system factors contributed in 65%, cognitive factors in 74%; **premature closure** was the single most common cognitive cause; error typically multifactorial (SRC-B-033, search-level).
- Croskerry (2003, Acad Med, cognitive biases/"cognitive forcing") [not retrieved].
- Lambe et al. 2016, BMJ Qual Saf 25(10):808–820 (28 studies): guided reflection was the most consistently successful approach; cognitive forcing strategies improved accuracy in some studies; field described as young (SRC-B-034, search-level).
- Al-Khafaji et al. 2022, BMJ Open 12(4):e058219: 14 checklist studies measuring error — **7 improved, 6 no improvement, 1 mixed**; **task-specific checklists** (e.g., stepwise ECG/radiograph reading) more often reduced errors (5/7) than **generic cognitive checklists** (4/10); authors call findings preliminary (SRC-B-035, fetched).
- 2022 medRxiv systematic review of debiasing/educational interventions: all intervention types showed mixed results (SRC-B-036, search-level, preprint).

**Rating: Mixed.** Transfer: EDF is a *generic cognitive structure*, the category with the weaker track record in medicine; the medical evidence base is mostly vignettes with known answers — similar to EDF's synthetic-case benchmark — and does not establish real-world outcome effects. Explicitly considering alternatives (EDF's competing origins; guided reflection) is the best-supported element. Premature closure ↔ EDF's homepage claim "One story crowds out alternatives" is a well-founded target problem.

## 7. Sensemaking (Weick) and Cynefin
- Weick, *Sensemaking in Organizations* (1995) [not retrieved]: theoretical, qualitative case tradition. **Rating: Theoretical.**
- Cynefin (Kurtz & Snowden 2003, IBM Systems Journal [not retrieved]): critiques that it rests mostly on anecdotal evidence; no controlled or comparative study found showing that Cynefin classification improves decisions (SRC-B-044, secondary/course material; low confidence). **Rating: Theoretical.** Relevance: EDF-0/1/2 escalation resembles Cynefin-style "match method to situation" logic.

## 8. Differential diagnosis and structured analytic techniques
- EDF names a "Differential Diagnosis Lens". Analysis of Competing Hypotheses (Heuer; intelligence analysis) is the obvious analogue; I recall controlled studies finding limited benefit of ACH but **did not retrieve them** (RQ-B-005). **Rating: Mixed (unverified).**

## 9. Inter-rater reliability of organisational/incident assessments
- Not retrieved (search budget). Background expectation: causal classification schemes (e.g., HFACS) report variable inter-rater reliability at finer category levels. EDF's own HY-005 (cross-executor comparability) is unsupported, and its only multi-evaluator data show 2/33 outputs with ≥20-point evaluator disagreement under ceiling conditions (REPO EV-EDF-009). **Rating: unknown for EDF; mixed-to-weak for the field (LOW confidence).**

## 10. Maturity models (relevant because buyers often expect "levels")
- Becker, Knackstedt & Pöppelbuß 2009 (BISE 1(3):213–222): >100 IT-management maturity models with "a certain degree of arbitrariness" in development and incomplete methodological documentation (SRC-B-040).
- de Bruin et al. 2005 (ACIS): six-phase development method (scope, design, populate, test, deploy, maintain) (SRC-B-041).
- Wendler 2012 (IST 54(12):1317–1339): 237 articles mapped; secondary sources report weaknesses in validation (SRC-B-042; primary not retrieved).
**Rating: Theoretical / weakly validated as a class.** Implication: EDF wisely avoids maturity levels; if Echelon adds an "organisational diagnostic maturity" score commercially, it would inherit this critique.

## 11. Theory of Constraints
Mabin & Balderstone 2003 (IJOPM 23(6):568–595): meta-analysis of 80+ successful TOC applications with significant operational and financial improvements; "no reports of failures" despite extensive searches (SRC-B-043, abstract fetched). **Rating: Mixed** — positive but drawn from published success cases, strongly exposed to publication/selection bias. Relevance: TOC's Current Reality Tree is a direct competitor to EDF's origin network/propagation for operational diagnosis, with a large practitioner base.

---

## Synthesis: does making diagnoses explicit improve accuracy or intervention outcomes?

1. **Accuracy:** Some controlled (mostly vignette) evidence in medicine that reflection and task-specific structure improve diagnostic accuracy; generic cognitive checklists are inconsistent (SRC-B-034, SRC-B-035, SRC-B-036). EDF's own benchmark found a generic structured prompt performed as well as EDF on easy cases (REPO EV-EDF-009). **Overall: Mixed; no evidence specific to EDF.**
2. **Intervention outcomes:** The best-studied institutional practice of explicit diagnosis (healthcare RCA) shows weak evidence of preventing recurrence; failures concentrate in weak actions and missing implementation/follow-up (SRC-B-029, SRC-B-031, SRC-B-030, SRC-B-028). **Overall: Largely unsupported** that explicit diagnosis alone improves outcomes.
3. **Diagnosis tools failing to lead to action:** Directly documented for RCA (Kellogg: recurrence despite repeated RCAs; Martin-Delgado: no study measured recurrence reduction; Mira: handoff to a different group). **Empirically supported as a failure mode.**
4. **Implication for EDF (CONCLUSION, medium confidence):** EDF's commercial value case cannot rest on "better diagnoses" in the abstract. It would need to show (a) better *control-point/action selection* (action strength), (b) actual implementation and verification, and (c) advantage over cheap generic structure — under real, hard, ambiguous cases with human users. None is currently evidenced.

| Approach | Rating |
|---|---|
| RCA (identification) | Mixed |
| RCA (preventing recurrence) | Largely unsupported / negative |
| Five Whys | Theoretical, critiqued |
| Ishikawa | Theoretical (not retrieved) |
| FTA / FMEA | Established engineering practice; comparative outcome evidence not retrieved |
| STAMP/CAST, system dynamics, GMB | Theoretical-to-mixed (not retrieved) |
| Weisbord / Burke-Litwin / Nadler-Tushman / 7S | Theoretical (weak validation) |
| Medical debiasing / checklists / reflection | Mixed |
| Sensemaking, Cynefin | Theoretical |
| Differential diagnosis / ACH | Mixed (unverified) |
| Maturity models | Theoretical, weakly validated class |
| Theory of Constraints | Mixed (publication bias) |
| EDF itself | Theoretical; one null comparative result |
