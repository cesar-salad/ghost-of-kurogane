# M0 - Prototype audit and baseline

Audit date: 2026-11-05. Status: In progress. Source baseline: `da8cb5e` (foundation implementation: `1adbaa6`). Gameplay was inspected without refactoring or upgrading dependencies.

## Reproduction and evidence

The developer previously reported that the playground worked in the Unity Editor. That is useful manual feedback, not a recorded pass for every acceptance check.

A separate local clone was created with `git clone --local --no-hardlinks` under the user's Temp directory (`ghost-kurogane-m0-da8cb5e`). No Library, Temp, UserSettings, or existing test artifacts were copied. Unity imported and resolved the committed project from scratch. Both suites ran before any new build utility was added, and `git status --porcelain` remained empty afterward:

- Unity 6000.3.27f1: 7/7 EditMode tests passed, runner exit 0.
- Unity 6000.3.27f1: 4/4 PlayMode tests passed, runner exit 0.
- These PlayMode tests instantiate an isolated rig; they do not load the saved playground scene. The Windows baseline uses the actual saved playground.
- Build validation and fix evidence are recorded below. The initial build overlaid only the new Editor-only `PrototypeBuild.cs`. After it exposed GOK-005, the corrected run additionally overlaid PlayerInputReader.cs and its PlayMode lifecycle regression test. Scene/assets/settings/packages were preserved. The corrected evidence therefore refers to da8cb5e plus the current uncommitted M0 changes, not an unchanged baseline commit.

Requirements: Windows, Unity 6000.3.27f1 via Hub, Windows Build Support, Git LFS, and package-registry access or a suitable local package cache. Input System 1.20.1, URP 17.3.0, and Test Framework 1.6.0 are pinned. No package upgrades were made. See [setup](unity-setup.md) and the root README for controls and installation.

## System inventory

Working means demonstrated by the listed test or evidence. Partial means the basic implementation exists but has a known gap. Missing identifies intentionally unimplemented functionality, not necessarily a defect. No system is currently classified Broken based on observed evidence.

| System | Status | Evidence and limits |
| --- | --- | --- |
| Movement / facing | Working | WalkSprintAndCameraFollow and MovementUsesAssignedCameraDirection pass; configurable camera-relative movement and rotation. Variable-frame-rate tuning is M1. |
| Camera | Partial | Follow displacement is tested; mouse orbit is implemented and the developer reported a playable Editor attempt. No collision/occlusion handling or recorded mouse-focus acceptance. |
| Collision / grounding | Partial | Floor contact and landing pass; wall/obstacle colliders exist. No edge recovery, slope/stair matrix, or boundary protection. |
| Jump / sprint | Working | JumpLeavesGroundAndLands and WalkSprintAndCameraFollow pass. |
| Interaction | Working | InteractionUsesInputAndWallsBlockTarget passes; prompt, actor-facing range, one access per press, and wall occlusion. Receipt is logged, without visible confirmation. |
| Animation | Missing | Capsule placeholder; no Animator/controller/clips. Deferred presentation work, not an M0 blocker. |
| Input | Partial | Keyboard commands are exercised through real Input System events. Mouse bindings exist; only movement has a gamepad binding. Rebinding UI and complete controller support are absent. |
| Health | Working | Damage/healing clamps, invalid amounts, events, death-once semantics and no resurrection covered by EditMode. No damage controls or death presentation. |
| Game state | Partial | Transition notifications tested. Paused is a notification, not a complete pause/input-lock flow. |
| Windows build | Working | Windows x64 Development build succeeds and its scene loads in a headless startup check after fixing GOK-005. Developer confirms the Windows build looked good, was playable, and basic commands worked. Detailed smoke-check results remain pending. |
| Narrative / choices | Missing | Intentionally deferred to M2. Do not introduce narrative frameworks during M0. |

## Repository checks

- Initial audit inventory: 146 tracked files, including 64 tracked `.meta` files. All 64 point to existing source files or folders; no duplicate project GUIDs were found.
- Ignore probes passed for Library, Temp, Obj, Logs, Builds, and UserSettings. No generated-cache directories are tracked. Source assets and their metadata are tracked normally.
- LFS filters resolve for PNG, FBX, WAV, and MP4 probes; `.meta` has no LFS filter. There are currently no LFS assets, so no real binary transfer was exercised.
- Force Text serialization and Visible Meta Files are configured. All ten saved-playground script references resolve against project/package metadata, including URP components.
- A scan of tracked current files found zero common private-key, AWS access-key, GitHub-token, or OpenAI project-key signatures. This was a limited signature scan, not a full history or security audit.
- No external art/audio/models, vendor DLLs, or non-Unity plugins were found in tracked source. Dependencies are recorded in manifest/lock and THIRD_PARTY_ASSETS.md. Multiplayer Center is an existing Editor default; no multiplayer gameplay is implemented. No asset redistribution rights claim is made beyond this inventory.

## Windows baseline

The initial Windows build compiled successfully (0 errors, 0 warnings), but its headless startup logged a TypeInitializationException: CreateMove ran from a MonoBehaviour field initializer, causing InputSystem to call Resources.FindObjectsOfTypeAll during scene deserialization. This is GOK-005, a real P0 that the pre-initialized Editor test environment did not expose.

The fix moves default action/binding setup into Reset/Awake, retaining existing named serialized actions and field names. No scene regeneration or binding migration is required. A new PlayMode test verifies that disabling the reader suppresses movement and re-enabling it restores input.

Corrected run:

- EditMode: 7 passed, 0 failed, exit 0.
- PlayMode: 5 passed, 0 failed, exit 0 (includes InputReaderCanBeDisabledAndReenabled).
- Windows x64 Development build, StrictMode: Succeeded, 0 errors, 0 warnings, exit 0. Duration 24.26 seconds, reported output size 159,858,277 bytes.
- Build timestamp: 2026-11-05T23:36:05Z. Original build directory: Baseline-20261105-233541-b5392f9d6ed944eb8f2e136f4b3744b6 in the isolated checkout.
- Runnable local copy: `Builds/Windows/M0-da8cb5e-fixed/GhostOfKurogane.exe`. Keep its data/runtime files alongside it. The older `M0-da8cb5e` directory is the known-bad initial reproduction, not the recommended build.
- Headless startup (`-batchmode -nographics`): remained alive for the 15-second observation window; log contains the post-load UnloadTime marker and no exception/assertion, missing-script, or missing-dependency signatures. The test process was intentionally terminated afterward; normal interactive exit was not tested.
- The first scripted log read encountered Unity's open-file lock; the log was then inspected successfully after process termination. The runtime result is based on that observed log, not the failed file-read command.
- Startup also emitted `Internal: deleting an allocation that is older than its permitted lifetime of 4 frames (age = 6)`. This allocator diagnostic is recorded as GOK-006; its cause and occurrence in normal rendering are unverified. Build warnings and runtime diagnostics are separate evidence.

The utility explicitly includes Prototype_Playground; the committed Build Profile scene list remains empty. No Unity/package versions changed.

Local ignored evidence: `Logs/M0/M0-Fixed-EditMode.xml`, `M0-Fixed-PlayMode.xml`, their logs, `M0-Fixed-Windows-build.log`, and `M0-fixed-player-startup.log`. Initial failing startup and original clean-checkout test results remain in the same directory. These machine-local logs and build outputs are not committed.

## Manual Windows feedback - 2026-11-05

The developer reports that the Windows build looked good and was playable with basic commands working. This supplements the automated/headless evidence with a reported visual and basic-control check. See [playtest-notes.md](playtest-notes.md) for scope; detailed checks and the allocator diagnostic remain open.

## Remaining M0 acceptance

- [ ] Capture baseline screenshots or 30-60 seconds of actual gameplay. Suggested shots: starting overview, jump/obstacle traversal, terminal prompt. Keep the capture tied to the build/source reference; do not substitute generated concept imagery.
- [ ] Complete the standalone manual smoke checklist in [playtest-notes.md](playtest-notes.md).
- [ ] Commit reviewed M0 changes and tag the chosen stable revision as `prototype-baseline-01`. No commit or tag is created automatically by this audit.

Known issues are prioritized in [prototype-bugs.md](prototype-bugs.md). M0 remains In progress until its remaining evidence and stable baseline are recorded.
