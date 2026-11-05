# AGENTS.md — Ghost of Kurogane

> Instructions for Codex and other coding agents working in this repository.
> Scope: the entire repository, unless a more specific `AGENTS.md` in a subdirectory overrides a section.
> **Project:** Ghost of Kurogane — Unity 6, C#, third-person narrative-driven action RPG.

## 1. Mission and priorities

Build a **small, playable, narrative-first vertical slice** about Maddox Takashi: a betrayed cybernetic samurai seeking revenge on the Kurogane Corporation while resisting the violent impulses of his prosthetic arm.

Prioritize, in order:
1. **Meaningful moral choices** with understandable trade-offs and observable consequences.
2. **Character relationships**, especially Maddox and Daichi Morimoto.
3. **Functional, responsive third-person interaction and combat** in service of the story.
4. **Maintainable, testable Unity code**, without speculative complexity.

Maddox's guiding rule is **never kill an enemy who has surrendered**. Player choices may challenge this rule, and the story must acknowledge their consequences. Do not hard-code moral judgments into a generic good/evil score.

**Current target:** One short mission / vertical slice (working title: *The Weight of Mercy*) with a workshop setup, small Kurogane annex, a consequential encounter, and a changed outcome or response. Treat the existing basic prototype as the baseline. Check `PROJECT_ROADMAP.md` and the repo before assuming what is already implemented.

## 2. Start every task by inspecting the project

Before making changes:
- Read this file, `README.md`, and relevant milestones in `PROJECT_ROADMAP.md` when present.
- Inspect `ProjectSettings/ProjectVersion.txt`, `Packages/manifest.json`, existing scenes, scripts, assemblies, tests, and established conventions.
- Respect the **actual** Unity version and installed package versions. Do not upgrade Unity or packages as a side effect of unrelated tasks.
- State which files/systems need modification and select the smallest coherent change.
- Reuse existing implementations where reasonable. Do not overwrite project assets, settings, scenes, prefabs, or user-authored content without explicit justification.
- If a requested change is much larger than one focused milestone, implement the smallest useful vertical step and identify follow-up tasks.

## 3. Repository and directory conventions

Use the existing layout rather than reorganizing it casually. Preferred first-party locations:

```text
Assets/_Game/
  Animations/
  Art/
  Audio/
  Prefabs/
  Scenes/
    Bootstrap/
    Prototypes/
    Levels/
  Scripts/
    Core/
    Player/
    Camera/
    Combat/
    AI/
    Interaction/
    Narrative/
    Progression/
    UI/
    Editor/
  ScriptableObjects/
  Settings/
  Tests/
    EditMode/
    PlayMode/
docs/
  architecture/
  game-design/
  development/
PROJECT_ROADMAP.md
THIRD_PARTY_ASSETS.md
```

- Put game-owned assets under `Assets/_Game/`; keep third-party packages/assets identifiable and separate when practical.
- Place each script near the system it serves. Match important class names and file names.
- Use `Editor/` directories for editor-only scripts; do not reference `UnityEditor` from runtime assemblies.
- Only create directories/files when needed; avoid empty scripts and speculative scaffolding.
- Commit Unity `.meta` files; preserve GUIDs when moving assets. Prefer moving/renaming assets in the Unity Editor so references survive.
- Do **not** hand-author Unity `.unity`, `.prefab`, `.asset`, or `.meta` serialization/YAML unless necessary, justified, and safely validated. Prefer Unity Editor tooling or clear manual setup instructions.

## 4. C# and Unity coding conventions

- Use standard C# conventions: `PascalCase` for types, methods, and properties; `camelCase` for locals/parameters; `_camelCase` for private instance fields; `I` prefix for interfaces.
- Use namespaces such as `GhostOfKurogane.Player`, `GhostOfKurogane.Narrative`, and `GhostOfKurogane.Combat`.
- Use `[SerializeField] private` fields for Inspector configuration, with appropriate validation; avoid public mutable fields and magic numbers.
- Keep `MonoBehaviour` classes small and focused. Prefer **composition** and explicit dependencies over inheritance chains, global state, or a large `GameManager`.
- Prefer readable, direct solutions. Do not introduce dependency-injection frameworks, service locators, custom event buses, reflection-based registries, or generic systems without an actual use case.
- Prefer Unity Input System for player controls; do not add legacy `Input.GetAxis` / `Input.GetKey` code to new features.
- Use Unity lifecycle methods correctly: subscribe/unsubscribe symmetrically (typically `OnEnable` / `OnDisable`); remove listeners and dispose resources when needed.
- Separate input, player movement, camera, interaction, combat rules, and animation presentation. Avoid tight cross-system dependencies.
- Use ScriptableObjects for stable designer-editable **definitions** (e.g., abilities/weapons), not as an accidental global mutable runtime state store.
- Use events or interfaces for genuine decoupling, but do not overabstract one-off behavior.
- Prefer clear, descriptive names. Add XML docs for non-obvious public contracts and comments explaining *why*, not narrating *what* the code visibly does.
- Handle missing serialized references defensively and surface actionable Inspector errors where appropriate. Avoid swallowing exceptions silently.
- Avoid repeated allocations and broad object searches in hot paths (`Update`, `FixedUpdate`) where a cached reference or event suffices; only optimize after identifying a real bottleneck.
- Keep source files formatted consistently with the existing project; don't reformat unrelated files.

## 5. Narrative and gameplay design rules

- **Story first:** Implement the smallest interaction proving player agency and consequence before adding breadth.
- Decisions need credible competing motivations, not an obvious benevolent answer versus a cartoonishly evil answer.
- Track consequential facts explicitly (e.g., whether an NPC was spared, information recovered, and Daichi's response). Favor a few well-defined state variables over a general-purpose morality framework.
- Keep narrative logic/data separate from UI/camera/animation presentation where practical, so conversations and consequences can be tested independently.
- When creating branching dialogue, ensure every branch reaches a coherent outcome. A branch can reconverge, but later dialogue or state should acknowledge the choice.
- Do not add dialogue, lore, characters, endings, or moral consequences that materially change approved canon without flagging them as proposals.
- Combat, bloodlust, and upgrades should reinforce Maddox's ethical conflict—not overwhelm it. Avoid removing player agency through unexplained randomness or irreversible effects with no warning.
- Prioritize one polished protagonist, one compact level, and one consequential choice over multiple playable characters, massive maps, or numerous classes.

## 6. Explicit scope restrictions

**Unless directly requested and tied to an approved roadmap milestone, do not implement:**
- Four open-world regions, world streaming, procedural generation, or expansive traversal systems.
- Multiple playable protagonists or character-switching architectures.
- Four complete weapon/class trees (tech, close-combat, ranged, magic), elaborate leveling, crafting, or inventory.
- Multiplayer, network architecture, online services, accounts, telemetry, or monetization.
- A bespoke rendering pipeline, custom physics framework, full cinematic editor, or generalized quest engine.
- Large dialogue/quest frameworks before a single branching encounter is playable.
- Large refactors, speculative package additions, or wholesale folder reorganizations during feature tasks.

When a request suggests one of these, propose a thin vertical slice or an isolated extension point first. Do not silently expand project scope.

## 7. Unity-specific validation requirements

Validate **in increasing order of cost**, based on what the execution environment actually supports:

1. **Static inspection:** Check references, namespaces, assembly boundaries, scene/prefab dependencies, package APIs, and compilation risks in changed files.
2. **EditMode tests:** Add/run focused Unity Test Framework tests for deterministic non-visual logic (e.g., narrative conditions, health rules, dialogue state) when practical.
3. **PlayMode tests:** Use when behavior depends on MonoBehaviours, scenes, time, collisions, or runtime initialization.
4. **Unity Editor compilation:** Open the project with its recorded Unity version and check Console errors/warnings. Resolve newly introduced compile errors.
5. **Manual smoke test:** In a prototype scene, confirm movement, camera, interaction, dialogue choices, branch consequences, and scene transitions as relevant to the change.
6. **Standalone build test:** For milestones or release-impacting changes, generate and launch a Windows build when the build environment is available.

**Do not claim Unity compilation, tests, scene validation, or a build passed unless they actually ran and results were observed.** VS Code or .NET compilation alone is not equivalent to Unity Editor validation. If Unity is unavailable, explicitly say what was checked and provide exact manual Editor verification steps.

### Manual smoke-test baseline

When relevant, verify that:
- The project opens without new Console errors.
- The player can move and the camera behaves correctly.
- Interaction prompts and triggers fire once per intended action.
- Dialogue progresses without dead ends or duplicate choices.
- Player decisions persist at least through the current scenario and visibly change follow-up dialogue/outcomes.
- Scene/prefab references remain intact.
- Entering/exiting Play Mode does not create stale subscriptions or broken state.

## 8. Unity assets, packages, and Git safety

- Treat `Assets/`, `Packages/manifest.json`, `Packages/packages-lock.json`, and `ProjectSettings/` as project-critical. Avoid broad automated changes.
- Do not commit generated `Library/`, `Temp/`, `Obj/`, `Logs/`, `Build/`, `Builds/`, IDE caches, or secrets. Respect the existing Unity `.gitignore`.
- Use Git LFS as configured for large binary assets. Do not change LFS tracking en masse without checking repository implications.
- Never delete or regenerate `.meta` files casually; changing GUIDs can break serialized references.
- Don't add third-party packages or download assets without explaining their license, source, and necessity. Update `THIRD_PARTY_ASSETS.md` when incorporating external content.
- Keep copyrighted or licensed game assets out of public redistribution unless the applicable rights allow it.
- Do not add telemetry, credentials, tokens, or cloud service configuration without explicit approval.
- Avoid destructive Git commands (e.g., `reset --hard`, force push, blanket clean) and don't discard existing user changes.

## 9. Testing and documentation expectations

For every meaningful code change:
- Add or update a focused test where feasible, especially for narrative branches and gameplay rules.
- Document any required Inspector wiring, Input Actions, package requirements, and scene setup.
- Update `README.md` when setup or execution changes.
- Update `docs/architecture/` when a new architectural contract is introduced.
- Update `PROJECT_ROADMAP.md` only when progress is **verified** or a milestone/scope decision is explicitly made; never mark a checkbox complete based solely on code generation.
- Use TODOs sparingly and tie major deferred work to an identifiable milestone.

## 10. Expected Codex response format

At the end of each task, report concisely:

1. **Implemented:** Actual functionality and rationale.
2. **Files changed:** Created/modified paths.
3. **Validation:** Exact commands/tests run, their results, and anything not validated.
4. **Unity Editor steps:** Required component wiring or manual checks, if any.
5. **Risks / follow-ups:** Known limitations and the smallest sensible next step.

When blocked by missing Unity Editor access, assets, or setup details, make the safest useful code/documentation changes possible and state the blocker. Never imply unfinished functionality is playable.

## 11. Definition of done for a feature

A feature is done only when:
- It meets the requested user-visible behavior and stays within the agreed scope.
- Integration with the current project is clear, with required scene/prefab/Inspector wiring completed or documented.
- No known new compilation errors or broken serialized references were introduced (to the degree verifiable).
- Relevant tests pass when executable; otherwise, verification gaps are explicitly identified.
- Any meaningful story choice has an observable consequence and appropriate state handling.
- Documentation and roadmap status accurately reflect the actual implementation.

**Guiding principle:** Prove the emotional experience with the smallest reliable playable system. Favor a complete, tested vertical slice over a broad but unfinished RPG architecture.
