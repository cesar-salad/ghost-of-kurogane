# Ghost of Kurogane — Development Roadmap

> **Living project plan** · Version 1.0 · Last reviewed: 2026-11-06
> **Engine:** Unity 6 / C# / URP · **Initial platform:** Windows PC · **Potential release:** Steam  
> **Current state:** M0 audit and Windows baseline recorded; developer confirms basic standalone visuals/playability and controls. Screenshots and prototype-baseline-01 are recorded on a separate branch; detailed smoke checks remain pending.
> **Next milestone:** M0 — Prototype Audit & Baseline

**Purpose:** Use this file to decide what to build next, track progress, and determine when a milestone is *actually finished*. Keep the first release goal small enough to playtest. The full RPG vision is a direction, **not** the current delivery commitment.

---

## 1. North Star

**One-sentence pitch:** *Ghost of Kurogane* is a third-person, narrative-driven action RPG about Maddox Takashi, a cybernetic samurai betrayed by Kurogane Corporation, who must choose between vengeance and the code of honor that forbids him from killing surrendered enemies.

### Design pillars — in priority order

1. **Morally difficult decisions:** Meaningful options with competing costs, not an obvious good/evil quiz.
2. **Relationships and emotional consequences:** Choices matter to Daichi and other characters, not just a numerical score.
3. **Violence has narrative weight:** Maddox's cybernetic arm tempts aggression; players retain agency over his actions.
4. **Persistent consequences:** Decisions alter later dialogue, available information, and/or mission outcomes.

**Core player promise:** “My decisions reveal who Maddox becomes, and the world responds.”

### Scope boundaries

| Delivery target | Included | Explicitly excluded for now |
| --- | --- | --- |
| **First playable (internal)** | One complete small narrative interaction with a choice and visible consequence | Open world, skill trees, additional protagonists |
| **Vertical slice (external playtest/demo candidate)** | 10–20 minute mission, one polished combat encounter, a consequential choice, follow-up dialogue, beginning/end | Four regions, full RPG progression, large enemy roster |
| **Full game (future, conditional)** | Multiple connected regions, more characters, combat specialties, progression, deeper branching | Not committed until vertical-slice evidence supports expansion |

> **Rule:** If a feature does not strengthen the first two pillars or allow the current milestone to function, defer it.

---

## 2. How to use this roadmap

- Change `[ ]` to `[x]` only after the task is **verified in a playable build or a repeatable test**.
- Mark blocked work `[!]` in the weekly log rather than pretending it is complete. (Markdown checklist tooling may not render `[!]`; this is intentional in notes only.)
- Each milestone has **Exit Criteria**. A phase is not complete until *all* exit criteria pass.
- Track scope changes in the **Decision Log** instead of silently adding requirements.
- Keep tasks small enough to close in one or two focused sessions; split anything that grows beyond that.
- Update the dashboard and weekly log at the end of each development week.
- Treat estimates as rough planning ranges, not deadlines. Solo animation, writing, and art integration are high-variance.

**Status vocabulary:** `Not started` · `In progress` · `Blocked` · `Done` · `Deferred`.

### Milestone dashboard

| ID | Milestone | Status | Target evidence | Depends on |
| --- | --- | --- | --- | --- |
| M0 | Prototype audit & project baseline | **In progress** | Recorded baseline build and known issues | Existing prototype |
| M1 | Player controls & interaction quality | Not started | Playable navigation/interaction test | M0 |
| M2 | Narrative foundation & first decision | Not started | Branching dialogue + saved outcome within session | M0, M1 interaction |
| M3 | The Weight of Mercy — graybox | Not started | Fully traversable 10–20 min scenario | M1, M2 |
| M4 | Combat & bloodlust narrative integration | Not started | One reliable encounter and consequential power choice | M3 graybox |
| M5 | Characters, cinematics & presentation | Not started | Coherent mission with placeholder/final-ish presentation | M3, M4 |
| M6 | Vertical slice stabilization & playtests | Not started | External testers complete unaided; findings acted on | M5 |
| M7 | Demo/release decision gate | Not started | Go/no-go based on player evidence and cost | M6 |
| F1 | Full-game preproduction *(optional)* | Deferred | Approved scope and production plan | M7 Go |
| F2 | Full-game production *(optional)* | Deferred | First content-complete campaign build | F1 |
| F3 | Commercial release preparation *(optional)* | Deferred | Store, compliance, QA, launch readiness | F2 |

**Suggested working cadence:** one playable improvement per week; one external playtest at each meaningful gate; one scope review per month.

---

## 3. Vertical slice: target experience

**Working mission title:** *The Weight of Mercy*  
**Target duration:** 10–20 minutes  
**Playable hero:** Maddox Takashi  
**Supporting character:** Daichi Morimoto  
**Conflict character:** Dr. Sora Minami (working name; replace freely)  
**Environment:** Daichi's workshop + compact Kurogane research annex (can be one scene with separated areas initially)

**Narrative beats:**

1. **Workshop:** Daichi and Maddox discuss Kurogane; player can reveal or conceal anxiety about the arm.
2. **Approach:** Player navigates a small district/annex; learns interaction and one movement technique.
3. **Confrontation:** One enemy encounter introduces violence and cybernetic temptation.
4. **Surrender:** Minami surrenders. Maddox must decide what to do while weighing the risk to other prisoners.
5. **Aftermath:** Daichi responds differently; recovered information and relationship state vary; a short hook points toward Kurogane.

**Narrative quality caution:** Avoid a false choice. “Spare = all rewards, execute = all punishment” is too easy. Rewrite the dilemma until both outcomes have credible benefits and harms. For example, sparing Minami preserves testimony but gives a dangerous collaborator the chance to mislead or escape; executing her stops that risk but destroys human testimony. The **player**, not the combat meter, makes the final choice.

### Vertical-slice definition of done

- [ ] New player understands Maddox's betrayal, his code, and Daichi's role without reading external lore.
- [ ] At least one choice is difficult for testers to decide, for reasons they can explain.
- [ ] The choice causes **two clearly differentiated aftermaths** (dialogue + at least one world/info change).
- [ ] Narrative state persists reliably through scene transitions during the session.
- [ ] Controls, interactions, combat, and dialogue are readable without developer coaching.
- [ ] No progression-blocking bug is observed in a full clean playthrough.
- [ ] A standalone Windows build runs without the Unity Editor.
- [ ] Playtest notes identify actionable fixes and at least one iteration has been completed.

---

## 4. Milestones and task checklists

### M0 — Prototype audit & project baseline

**Goal:** Know what already works; do not rebuild functioning systems.  
**Rough effort:** 1–3 focused sessions.

**Tasks**

- [x] Run the existing prototype from a clean checkout; document Unity version, packages, and setup instructions.
- [x] Inventory working systems: movement, camera, collision, jump/sprint, interaction, animation, input, health, builds.
- [x] Mark each system `Working`, `Partial`, `Missing`, or `Broken` in `docs/development/prototype-audit.md`.
- [x] Record baseline screenshots or 30–60 seconds of gameplay capture.
- [x] Create a fresh Windows build (or log exact build blockers).
- [x] Verify Git ignores Unity generated directories, tracks `.meta` files, and uses Git LFS for large binaries as appropriate.
- [x] Confirm no secrets, commercial asset source files prohibited from redistribution, or unsupported plugins are committed.
- [x] Create a prioritized bug list (`P0` blocker, `P1` major, `P2` polish).
- [x] Commit/tag stable baseline (example: `prototype-baseline-01`).

**Exit criteria:** A second developer—or future you—can clone the repo, open the correct Unity version, run the prototype, and understand known limitations.

**Evidence (2026-11-05):** [Prototype audit](docs/development/prototype-audit.md), [prioritized issues](docs/development/prototype-bugs.md), and [pending standalone acceptance](docs/development/playtest-notes.md). Clean source tests passed (7 EditMode, 4 PlayMode); the standalone exposed an input-initialization blocker, now fixed and verified with 7 EditMode + 5 PlayMode tests, a successful Windows build, and a headless startup check. Secret/license checks cover the current tracked inventory and limited signature scan, not all Git history. Developer subsequently confirmed that the Windows build looked good, was playable, and basic commands worked. M0 remains In progress pending the unreported detailed smoke checks.

**Do not do yet:** Refactor all code for theoretical future character classes.

**Baseline evidence (2026-11-06):** Three original [Editor Game-view screenshots](docs/development/baseline/prototype-baseline-01/README.md) are preserved through Git LFS. Branch `codex/m0-baseline-01` records this evidence; annotated tag `prototype-baseline-01` identifies its baseline commit. These images complement the reported Windows playtest but do not certify additional standalone smoke checks.

### M1 — Player controls & interaction quality

**Goal:** Make basic navigation feel reliable, even with placeholder art.

**Tasks**

- [ ] Confirm camera-relative move, rotate, sprint and optional jump work consistently at variable frame rates.
- [ ] Confirm input rebinding strategy and controller support approach; keyboard/mouse must work first.
- [ ] Handle slopes, stairs, ledges, collisions, and scene boundaries without obvious breaks.
- [ ] Add simple input lock/unlock for dialogue and cinematics.
- [ ] Support contextual interaction with a clear prompt and limited range.
- [ ] Implement one reusable interactable example (terminal or door).
- [ ] Add camera collision/occlusion behavior or acceptable fallback.
- [ ] Define a minimal player state model (exploring, interacting, combat, cutscene); avoid conflicting inputs.
- [ ] Write smoke-test steps for movement, interaction, and camera.

**Exit criteria:** A tester can traverse the graybox, use a terminal, and enter/exit an interaction without control glitches or getting stuck.

**Dependencies:** M0.

### M2 — Narrative foundation & first meaningful choice

**Goal:** Validate your top design pillar before investing in content scale.

**Tasks**

- [ ] Create a one-page outline of *The Weight of Mercy*: setup, conflict, choice, consequences.
- [ ] Define Maddox's non-negotiable history and values; separate them from player-controlled behavior.
- [ ] Write Daichi's motivations, concerns, and limits—not just exposition about Maddox.
- [ ] Write Minami's accountability, motivations, available information, and possible deception.
- [ ] Draft the surrender scene in plain text, including the case for each choice.
- [ ] Implement minimal dialogue data (lines, speaker, choices, conditional links).
- [ ] Implement a dialogue runner/UI with next, choice select, and safe exit behavior.
- [ ] Implement simple narrative state: `sparedMinami`, `toldDaichiAboutArm`, `recoveredPrisonerData`, etc.
- [ ] Implement conditional aftermath dialogue based on narrative state.
- [ ] Store choices across scene changes; define simple save/load requirements only if needed for this milestone.
- [ ] Write unit tests for dialogue branching, choice effects, and missing references where possible.
- [ ] Conduct at least two **script-only** playtests; ask what each player believes each choice costs.
- [ ] Revise the dilemma if one option has no credible downside or if the tradeoff is unclear.

**Exit criteria:** In a minimal Unity scene, a player speaks to an NPC, makes a meaningful choice, and sees an appropriately changed later conversation.

**Dependencies:** M0; M1 interaction hook.

### M3 — *The Weight of Mercy* level graybox

**Goal:** Prove narrative pacing in navigable 3D space.

**Tasks**

- [ ] Create a top-down sketch and flow of workshop → approach → annex → confrontation → return.
- [ ] Decide whether workshop and annex share one Unity scene or use scene loading; choose simplest reliable approach.
- [ ] Block out with primitives / ProBuilder; prioritize readability, sightlines, and traversal time.
- [ ] Place mission objectives and interactions; avoid aimless wandering.
- [ ] Add encounter location, surrender trigger, and aftermath trigger.
- [ ] Add checkpoints/restart path that does not corrupt narrative state.
- [ ] Add minimal objective UI (single active objective; no full quest journal).
- [ ] Add basic lighting/audio cues to distinguish safe and threatening spaces.
- [ ] Capture a full start-to-finish playthrough and note dead time or confusion.
- [ ] Reduce navigation, exposition, or repetitive interactions that don't support tension.

**Exit criteria:** A complete beginning-middle-end plays in graybox form with no manual intervention; the critical choice is reached naturally.

**Dependencies:** M1, M2.

### M4 — Combat and bloodlust, tied to story

**Goal:** Make one encounter readable and thematically meaningful; **not** build the whole action RPG.

**Tasks**

- [ ] Implement one primary katana attack and hit detection with reliable timing and feedback.
- [ ] Implement one defensive response: dodge **or** parry (choose based on prototype feel).
- [ ] Implement one basic enemy with telegraphed attack, damage, hit reaction, and defeat.
- [ ] Add simple health, damage, and retry logic to support the encounter.
- [ ] Add one cybernetic power, e.g. brief overdrive with a visible cost/aftereffect.
- [ ] Define bloodlust rules in writing: triggers, feedback, limits, and any narrative effects.
- [ ] Ensure player agency: the arm tempts or pressures Maddox; it should not silently choose moral outcomes for the player.
- [ ] Give the player at least one way to control or resist the arm.
- [ ] Record whether/how combat behavior influences Daichi's aftermath dialogue.
- [ ] Test enemy readability, hit feedback, recovery, and response time with multiple players.
- [ ] Avoid adding ranged, magic, skill trees, or multiple weapons in this phase.

**Exit criteria:** The combat encounter is understandable and repeatable; cybernetic power changes the player's experience and is acknowledged by the story.

**Dependencies:** M3 playable graybox.

### M5 — Characters, cinematics & sensory presentation

**Goal:** Replace only the placeholders that most affect comprehension and emotion.

**Tasks**

- [ ] Lock Maddox's silhouette: cloth-forward feudal Japan aesthetic plus clearly recognizable right cybernetic arm.
- [ ] Create/obtain a legitimately licensed placeholder humanoid with usable animations; integrate a temporary katana.
- [ ] Distinguish Maddox, Daichi, and Minami with clear shapes, color/wardrobe, and staging.
- [ ] Build a concise cinematic shot list for opening, surrender moment, and ending.
- [ ] Use Unity Timeline/Cinemachine for camera blocking; keep cinematics short.
- [ ] Add subtitles and readable dialogue presentation before pursuing voice acting.
- [ ] Animate or stage key emotional beats (hesitation, lowered weapon, surrender, Daichi's reaction).
- [ ] Add sound effects for footsteps, sword strikes, cybernetic activation, UI, and key environments.
- [ ] Add simple music/ambience using appropriately licensed material; track licenses.
- [ ] Define minimal visual language for choices, consequences, and the arm's unstable state.
- [ ] Make basic accessibility passes: subtitle readability, UI scaling, no color-only indicators.
- [ ] Profile the intended Windows build; optimize the largest issues rather than polishing everything.

**Exit criteria:** Testers understand who is speaking, what the dilemma is, and what happened afterward with minimal external explanation.

**Dependencies:** M3, M4.

### M6 — Vertical slice stabilization & external playtests

**Goal:** Establish whether the experience works for people who haven't helped design it.

**Tasks**

- [ ] Produce a clean standalone Windows build with a clear version number.
- [ ] Create a short tester instruction sheet with controls and bug-report method; avoid revealing intended moral answers.
- [ ] Recruit 5–8 testers unfamiliar with the story, if feasible.
- [ ] Observe sessions without coaching; record stuck points, crashes, and choice rationale.
- [ ] Ask: Why did you choose that outcome? Did the options feel credible? Did Daichi's reaction matter? Would you replay?
- [ ] Track completion time and whether players reached the ending unaided.
- [ ] Classify bugs `P0`/`P1`/`P2`; fix every P0 and priority P1 issue.
- [ ] Revise unclear dialogue, interactions, prompts, or pacing based on repeated patterns.
- [ ] Run regression tests of both branching outcomes after each narrative or code change.
- [ ] Rebuild and retest; compare results to first sessions.
- [ ] Create a short gameplay capture showing narrative → action → choice → consequence.

**Suggested success signals (targets, not universal standards)**

- At least 4 of 5 testers complete the slice without assistance.
- A majority accurately describe Maddox's core conflict and the cost of their choice.
- Both outcome paths work with no progression-blocking errors.
- Several testers express curiosity about the alternate path or a future chapter.
- A stable build plays from start to finish on a second machine.

**Exit criteria:** A reproducible, stable build plus written playtest findings and at least one round of evidence-based improvements.

**Dependencies:** M5.

### M7 — Go / no-go: demo or preproduction

**Goal:** Choose whether to polish a public demo, expand into preproduction, or revise the core design.

**Tasks**

- [ ] Compare playtest evidence with the four design pillars.
- [ ] List what players repeatedly loved, ignored, misunderstood, or disliked.
- [ ] Estimate actual hours spent per milestone (code, art, animation, writing, QA).
- [ ] Identify systems that scaled poorly and need refactoring based on demonstrated pain.
- [ ] Estimate cost and time to make one more level at the tested quality bar.
- [ ] Choose explicitly: **Polish demo**, **Iterate core slice**, **Start full-game preproduction**, or **Stop/reshape**.
- [ ] Write the decision and rationale in the Decision Log.

**Exit criteria:** One documented decision, with scope, next target, and a reason grounded in playtesting—not just excitement.

---

## 5. Future expansion — only after M7 approval

These are **backlog themes**, not active commitments. Do not start them simply because they sound exciting.

### F1 — Full-game preproduction

- [ ] Define game length, target audience, and commercial positioning.
- [ ] Write a compact story outline with beginning, midpoint, climax, endings, and required branches.
- [ ] Create a narrative dependency map: choices, persistent flags, and reconvergence points.
- [ ] Define playable character roster **only if** each contributes a unique narrative/mechanical purpose.
- [ ] Establish world lore and a consistent explanation of magic (if magic remains in scope).
- [ ] Create a map plan for up to four regions: purpose, distinctive gameplay, narrative role, asset reuse.
- [ ] Prototype one additional combat specialization before committing to four.
- [ ] Define progression/skill-tree purpose and test whether it improves player agency.
- [ ] Write content production budgets for scenes, characters, animations, environments, and audio.
- [ ] Approve a realistic staffing, schedule, cash budget, and risk plan.

**Gate:** Full-game scope matches demonstrated production capacity and budget.

### F2 — Full-game production

- [ ] Build a modular mission pipeline and branching dialogue authoring workflow.
- [ ] Expand save/load, quest state, transitions, accessibility, and telemetry as actual needs arise.
- [ ] Create and test each region sequentially; finish one at quality target before starting all four.
- [ ] Add additional protagonists only after their narrative and mechanics are prototyped.
- [ ] Add ranged/tech/magic specializations selectively and verify balance/cost.
- [ ] Build progression with meaningful choices rather than filler skill nodes.
- [ ] Develop and integrate music, VO, motion/animation, VFX, and cinematics under asset budgets.
- [ ] Run regular end-to-end branch testing; maintain a consequence matrix.
- [ ] Complete content lock, localization-ready text, optimization, and comprehensive QA.

**Gate:** Content-complete playable campaign with traceable choice consequences.

### F3 — Steam/commercial release preparation

- [ ] Recheck current Steamworks onboarding, fees, policy, disclosure, store, and build requirements.
- [ ] Verify publishing rights for all art, audio, fonts, animations, plugins, and code.
- [ ] Audit third-party asset licenses and attribution obligations.
- [ ] Evaluate trademark/name conflicts for *Ghost of Kurogane* before marketing investment.
- [ ] Prepare store assets, trailer, screenshots, description, and appropriate disclosures.
- [ ] Set up release branches, versioning, crash reporting, and QA/build automation appropriate to scope.
- [ ] Test supported hardware, input devices, graphics settings, save integrity, and accessibility.
- [ ] Plan community feedback, demo distribution (if applicable), release candidate testing, and post-launch support.

**Gate:** Release candidate passes compliance, licensing, performance, and stability checks.

---

## 6. Cross-cutting workstreams

### Narrative & writing

- [ ] Maintain a **story bible**: timeline, character goals, character knowledge, world rules, terminology.
- [ ] Maintain a **choice ledger**: choice ID, options, immediate effects, later effects, test coverage.
- [ ] Define Maddox's values vs. player-controlled decisions clearly.
- [ ] Review branches for false choices, plot holes, and accidental contradictions.
- [ ] Keep content in editable data assets rather than hardcoding entire dialogue sequences in scripts.

### Engineering & Unity

- [ ] Separate game rules/data from presentation where it materially simplifies testing.
- [ ] Prefer composition and small Unity components over speculative manager hierarchies.
- [ ] Keep assemblies and folder structure pragmatic; introduce `.asmdef` files when dependencies justify them.
- [ ] Build player-facing functionality first, then add abstractions based on repeated need.
- [ ] Keep main branch buildable; use short-lived feature branches and clear commits.
- [ ] Track scene, prefab, and ScriptableObject references carefully during refactors.

### Art, audio & licensing

- [ ] Prioritize readability, silhouette, and performance over asset count.
- [ ] Preserve Maddox's recognizable cloth-first ronin design and cybernetic arm contrast.
- [ ] Record source, creator, license, and attribution for every third-party asset in `THIRD_PARTY_ASSETS.md`.
- [ ] Avoid using unlicensed reference material as production assets.
- [ ] Keep placeholder and final asset status visible to prevent accidental release of test content.

### QA & accessibility

- [ ] Test both moral branches after story changes.
- [ ] Make dialogue skippable or replayable as appropriate, without breaking mission state.
- [ ] Provide subtitles, readable interaction prompts, sensible default keybindings, and clear combat feedback.
- [ ] Check that a single missed interaction cannot soft-lock the mission.
- [ ] Test a packaged build regularly, not only Play Mode in the Editor.

---

## 7. Backlog prioritization rules

For any proposed feature, answer these questions before adding it to the active milestone:

1. **Pillar fit:** Which of the four pillars does it strengthen?
2. **Player evidence:** What observed problem or opportunity justifies it?
3. **Smallest experiment:** Can we test the idea in one room, one encounter, or one conversation?
4. **Production cost:** How much new art, animation, AI, UI, writing, and testing does it require?
5. **Cut condition:** What result would convince us not to continue?

**Default priority:** narrative clarity → complete playable flow → input/combat reliability → emotional presentation → polish → expanded content.

### Explicit “not now” list

- [ ] Four fully populated open-world regions
- [ ] Multiple playable heroes
- [ ] Full tech / close-combat / ranged / magic class trees
- [ ] Large inventory/crafting/economy system
- [ ] Procedural world generation
- [ ] Multiplayer or online backend
- [ ] Full voice acting and complex facial capture
- [ ] Custom dialogue editor before basic data-driven dialogue proves insufficient

These are not rejected forever. They are **deferred until the vertical slice earns them**.

---

## 8. Risks and mitigations

| Risk | Early warning sign | Response |
| --- | --- | --- |
| Scope creep | More systems started than completed | Freeze new features until current milestone exits |
| Combat overshadows narrative | Testing focuses on DPS; no one discusses choice | Prototype consequences before adding moves |
| Obvious morality choice | Nearly everyone picks one option with no hesitation | Rewrite competing costs and uncertainty |
| Unmanageable branching | Every decision requires entirely new levels | Branch locally, reconverge deliberately, preserve state |
| Animation bottleneck | Weeks spent on the hero model before a fun slice | Use licensed placeholders; polish critical shots only |
| Technical debt | Constant regressions or unclear ownership | Refactor proven hotspots; add focused tests |
| Licensing exposure | Assets with unknown usage rights | Maintain register from first import; replace uncertain assets |
| Solo-dev fatigue | Stalled progress across many simultaneous workstreams | Limit active WIP; target a small weekly playable change |

---

## 9. Suggested repo documentation

```text
/
├── README.md
├── PROJECT_ROADMAP.md             # This living plan
├── THIRD_PARTY_ASSETS.md
├── docs/
│   ├── game-design/
│   │   ├── game-vision.md
│   │   ├── story-bible.md
│   │   ├── weight-of-mercy.md
│   │   └── choice-ledger.md
│   ├── architecture/
│   │   ├── overview.md
│   │   └── narrative-state.md
│   └── development/
│       ├── prototype-audit.md
│       ├── playtest-notes.md
│       └── decision-log.md
└── Assets/_Game/...
```

Create the supporting files **only when they have useful content**. This tree is a guide, not a requirement to generate empty documents.

---

## 10. Weekly progress log

Copy the template for each new week, keeping the most recent entry first.
### Week of 2026-11-02

- **Current milestone:** M0 - Prototype audit and project baseline
- **Status:** In progress
- **Playable improvement shipped:** Fixed standalone startup failure caused by Input System binding setup during scene deserialization; no gameplay expansion.
- **Completed tasks:** Clean-checkout import/tests, system and repository audit, prioritized issues, repeatable Windows development build.
- **Open blockers (with owner/next action):** Developer: standalone visual/input smoke pass and real gameplay capture; then review/commit/tag baseline. Investigate allocator diagnostic if it reproduces with normal rendering.
- **Playtest observation or evidence:** Existing developer Editor feedback was positive. Clean baseline: 7 EditMode / 4 PlayMode pass. Corrected candidate: 7 EditMode / 5 PlayMode pass; Windows build 0 errors / 0 warnings; headless startup loads scene without the original exception.
- **Next week's single most important outcome:** Finish M0 acceptance and establish a tagged baseline before M1.
- **Scope changes / decisions:** Only fix an observed M0 startup blocker; defer camera/boundary polish to M1.
- **Build or commit reference:** Source da8cb5e plus current M0 changes; local Builds/Windows/M0-da8cb5e-fixed; not yet committed/tagged.

### Week of YYYY-MM-DD

- **Current milestone:** M0 / M1 / ...
- **Status:** Not started / In progress / Blocked / Done
- **Playable improvement shipped:**
- **Completed tasks:**
- **Open blockers (with owner/next action):**
- **Playtest observation or evidence:**
- **Next week's single most important outcome:**
- **Scope changes / decisions:**
- **Build or commit reference:**

---

## 11. Decision log

| Date | Decision | Reason | Revisit when |
| --- | --- | --- | --- |
| 2026-11-05 | Narrative and difficult moral choices are the primary MVP focus | Distinctive player experience and manageable scope | After first external playtests |
| 2026-11-05 | First slice uses Maddox, one short mission, and one consequential choice | Validate core experience without building four worlds | M7 |
| 2026-11-05 | Four regions, extra protagonists, and four combat classes remain future scope | Avoid premature content and systems expansion | F1 approval |

---

## 12. Immediate next actions

1. **Complete M0:** Audit the prototype you already have, produce a standalone build, and write down what actually works.
2. **Choose one narrative moment:** Implement a short Maddox–Daichi interaction with a stateful choice.
3. **Prove consequences:** Return to Daichi after the mission decision and show a genuinely different reaction.
4. **Only then build the full graybox:** Connect the workshop, annex, encounter, surrender, and aftermath.

> **Definition of progress:** Not lines of code, number of systems, or amount of terrain. Progress is a more complete, more compelling, more testable player experience.
