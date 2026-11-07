# Ghost of Kurogane

Original third-person narrative action RPG foundation for Windows PC, built in Unity 6.3 LTS with C# and URP. Maddox Takashi, a betrayed cybernetic samurai reconstructed by Daichi Morimoto, seeks revenge against Kurogane Corporation while struggling to uphold a code that forbids killing surrendered enemies.

The design pillars are difficult moral choices, consequential relationships, combat tied to narrative themes, and persistent consequences. This milestone implements only a small movement and interaction playground; narrative, combat, saving, and progression remain future work.

## Requirements and installation

- Unity **6000.3.27f1** (6.3 LTS), installed through Unity Hub. Add Windows Build Support for standalone builds.
- Git and Git LFS. Run `git lfs install` before adding binary art; after cloning, run `git lfs pull`.
- Packages are pinned in `Packages/manifest.json` and resolved in `packages-lock.json`: Input System 1.20.1, URP 17.3.0, Test Framework 1.6.0. Standard Editor-generated modules are retained.

Clone the repository, then use Unity Hub > Add > Add project from disk and select the repository root. Open with the pinned Editor and wait for package resolution and script compilation. This repository was initialized with the installed Unity Editor; do not create a second project over it.

Project Settings > Player > Other Settings > Active Input Handling must be **Input System Package (New)**; restart the Editor if changing it. Asset Serialization is Force Text and Version Control uses Visible Meta Files. Commit all source `.meta` files and project/package configuration.

## Run the prototype

1. Open `Assets/_Game/Scenes/Prototypes/Prototype_Playground.unity` if already generated.
2. Otherwise select **Ghost of Kurogane > Create Prototype Playground**. Save any open scene when prompted. The command generates real Unity assets through Editor APIs and uses a unique filename when a scene already exists.
3. Press Play and focus the Game view. WASD moves relative to the camera, Left Shift sprints, Space jumps, and mouse movement orbits the player.
4. Face the terminal in front of the starting position. Within 2.5 meters, the HUD shows **E: Access terminal**. Press E; inspect the Console for an interaction count.
5. Escape releases the mouse; left click captures it again. Leave Play mode to restore the prior cursor state.

The scene has a ground plane, capsule placeholder, CharacterController, independent follow camera, directional/ambient lighting, three obstacles, a terminal, and temporary instructions. Re-running the generator creates another scene; it never overwrites one or changes the Build Profile scene list. For a Windows build, open File > Build Profiles, select Windows, and add the desired playground scene to its scene list.

Cinemachine was absent during repository inspection. The prototype uses a small independent orbit camera with smooth follow. See [camera setup](docs/development/unity-setup.md) for optional Cinemachine installation and migration. It is not a required compile dependency.

## Project structure

```text
Assets/_Game/
  Art/{Characters,Environments,Materials,Models,Textures,VFX}
  Audio/{Music,SFX,Voice}
  Animations/{Characters,Controllers}
  Prefabs/{Characters,Enemies,Environment,UI}
  Scenes/{Bootstrap,Prototypes,Levels}
  Scripts/{Core,Player,Camera,Combat,AI,Interaction,Narrative,Progression,UI,Editor}
  ScriptableObjects/{Characters,Weapons,Abilities}
  Settings/
  Tests/{EditMode,PlayMode}
Packages/                 # Versioned manifest and resolved dependencies
ProjectSettings/          # Generated and maintained by Unity
docs/{architecture,game-design,development}/
```

Keep external assets in their own folders outside `_Game` and record licenses in [THIRD_PARTY_ASSETS.md](THIRD_PARTY_ASSETS.md). Reserved folders intentionally contain no speculative C# classes.

## Architecture and current features

[Architecture overview](docs/architecture/overview.md) describes component responsibilities. Input Actions are serialized on the input reader and editable in its Inspector; movement and interaction never reference physical keys. The move action already includes a gamepad stick binding. Full gamepad camera and button support is a future task. The first M1 pass adds camera collision and a timed interaction confirmation; see [quality-pass setup and smoke steps](docs/development/m1-quality-pass.md).

Movement includes analog walk/sprint, smooth facing, grounded collision, gravity, and jumping. Interaction uses a forward ray from the actor, respecting the first solid obstruction. Health exposes bounded damage/healing, change events, and one death event. The scene-owned manager exposes initialization, playing, and paused states without a singleton. Paused is a state notification only; a complete pause flow is not implemented.

## Tests and verification

Open Window > General > Test Runner and run the EditMode and PlayMode suites. EditMode covers health limits, invalid values, death semantics, and state notifications. PlayMode exercises simulated Input System commands against real CharacterController collision, camera tracking, and interaction. See [verification](docs/development/verification.md) for recorded results and manual checks.

For batch tests, close the project in the Editor and run the installed Unity executable with `-batchmode -nographics -projectPath <repository> -runTests -testPlatform EditMode -testResults <absolute-results.xml> -logFile <absolute-log.txt>`. Repeat with `PlayMode`. Do not add `-quit` to test runs; the test runner exits when finished. Results/logs should be placed outside source assets (for example in `Logs/`).

## Limitations and next milestones

Placeholder art, IMGUI instructions, no animations or combat. The fallback camera now contracts before detected geometry and smoothly restores its distance; embedded-pivot and tight-corner cases still require visual testing. No death presentation, respawn, pause UI, or complete gamepad support. Health healing cannot resurrect. Interactions are actor-facing and ignore trigger-only colliders. No inventory, quests, dialogue tree, save/load, AI, or world streaming.

Current work: [M1 player controls and interaction quality](docs/development/m1-quality-pass.md), tracked in [PROJECT_ROADMAP.md](PROJECT_ROADMAP.md). The tagged M0 baseline remains available; its detailed manual-check gaps remain recorded. Next M1 work covers scene-boundary recovery and input locking before M2 narrative choices. Combat follows the roadmap's graybox and narrative work.

## Windows baseline build

Use **Ghost of Kurogane > Build Windows Baseline** after script import. It builds the saved playground explicitly into a unique ignored directory under Builds/Windows and writes build-evidence.txt beside the executable. It requires Windows Build Support, does not regenerate the scene, and preserves the Build Profile scene list. See [baseline build instructions](docs/development/unity-setup.md#m0-windows-baseline-build).
