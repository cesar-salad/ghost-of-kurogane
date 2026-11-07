# M1 - First player-controls and interaction quality pass

Started 2026-11-06, following the developer's request to begin the next phase. M0's baseline remains tagged prototype-baseline-01; its unreported detailed smoke checks are still open. This is a bounded M1 increment, not completion of the full milestone.

## Implemented behavior

- The existing ThirdPersonCamera sphere-casts from its smoothed follow pivot toward its desired orbit position. It immediately reduces distance to stay before a detected blocking surface, then smooths the return when clear.
- Collision settings are Inspector-configurable: layer mask, sphere radius (default 0.25), clearance padding (0.05), and distance-recovery time (0.2 seconds). Keep radius large enough for the camera's near plane, especially if changing FOV or aspect ratio.
- Trigger volumes and colliders on the followed player root/children are ignored. Assign the actual actor root as the follow target; this is already how the playground is wired. A reused fixed hit buffer avoids new per-frame query allocations; saturation conservatively places the camera at the pivot rather than trusting incomplete hits.
- PlayerInteractor emits InteractionPerformed only after a detected target's Interact call returns. Missing, blocked, or out-of-range targets do not emit it. The existing void interaction contract does not represent a richer gameplay success/failure result.
- PrototypeHud subscribes/unsubscribes with its lifecycle and displays a configurable confirmation (default "Interaction complete.") for 2.5 unscaled seconds. It works when wired by the existing scene or Configure method. No scene regeneration, terminal-specific UI, dialogue system, or package installation is required.

Modified source: ThirdPersonCamera.cs, PlayerInteractor.cs, PrototypeHud.cs, and PrototypeGameplayTests.cs. Existing serialized field names and references were preserved. No scenes, settings, metadata, packages, or baseline tags were regenerated or changed.

## Manual smoke test

1. Open the existing Prototype_Playground scene after compilation. Move with WASD, orbit with mouse, sprint with Shift, jump with Space, and confirm ordinary tracking still feels right.
2. Stand with the wall behind the player and orbit so the wall lies between the player and desired camera position. The camera should move forward before entering the wall. Move/orbit clear; it should return smoothly without snapping backward.
3. Repeat near corners, at low/high pitch, while sprinting, and while jumping onto the low obstacle. Inspect screen edges for near-plane clipping and watch for jitter. These visual cases are not approved by headless tests.
4. Face the terminal within range and press E. Confirm the prompt and the separate "Interaction complete." message appear. The confirmation should expire after roughly 2.5 seconds and a new interaction should refresh it.
5. Release E before pressing again. Hold E and confirm it does not repeatedly dispatch each frame. Turn away, move out of range, or put a wall in front: pressing E must not create a new confirmation.
6. Exit/re-enter Play mode. Confirm feedback subscriptions do not duplicate. Escape releases the cursor; click captures it. Check focus loss/recovery manually.
7. For standalone acceptance, use Ghost of Kurogane > Build Windows Baseline, then launch the new executable and repeat steps 1-6. This creates a separate development build and does not move prototype-baseline-01.

## Limitations and follow-up

The fallback is a basic collision response, not a full visibility solver. A cast beginning inside solid geometry cannot depenetrate an embedded pivot; extremely tight spaces, custom near planes, dense collider arrangements, and smoothed tracking around corners still need visual testing. Tune the mask/radius for real environments. Cinemachine remains an optional later choice, not an added dependency.

Scene-boundary recovery (GOK-001), slopes/stairs and variable-frame-rate acceptance, input lock/state ownership, complete controller support/rebinding, and narrative interactions remain unfinished M1 tasks. GOK-006's allocator diagnostic is not addressed by this change. Do not infer a full M0/M1 acceptance pass from this increment.

## Validation

Unity tests and a Windows development build are run in a separate local checkout using Unity 6000.3.27f1 with the existing package versions. Recorded results are appended after observation. The PlayMode suite adds camera contraction/recovery and collider-filtering coverage; the interaction test also checks a single notification, blocked-target suppression, and HUD expiration. Manual visual/input acceptance remains pending.
### Observed results - 2026-11-06

- Unity compilation: successful in 6000.3.27f1; no C# compiler errors or warnings in the recorded test/build logs.
- EditMode: 7 passed, 0 failed, runner exit 0.
- PlayMode: 7 passed, 0 failed, runner exit 0. The two new tests are CameraContractsBeforeWallAndRecovers and CameraIgnoresTriggersAndFollowTargetColliders. InteractionUsesInputAndWallsBlockTarget additionally verifies feedback dispatch, wall suppression, and expiration.
- Windows x64 Development build (StrictMode): Succeeded, 0 errors, 0 warnings, exit 0. Report timestamp 2026-11-06T23:33:55Z; duration 66.97 seconds; reported size 159,861,138 bytes.
- Standalone: launched headlessly for 15 seconds, remained alive, and logged the scene-load marker without exception/assertion, missing-script, or missing-dependency signatures. The test process was intentionally terminated; normal interactive exit was not tested. The prior allocator diagnostic did not appear in this observation, which does not establish its resolution.
- Source hashes for all four changed C# files match the files tested/built in the isolated checkout. git diff --check passes. Existing scene/settings/package files and baseline tag are unchanged.

Runnable local copy: `Builds/Windows/M1-quality-pass-01/GhostOfKurogane.exe`. Keep the executable with its sibling data/runtime files. The reused baseline build utility retains its original M0 report heading; the timestamp and directory above identify this distinct M1 candidate.

Ignored evidence: `Logs/M1-quality-pass-01/` contains M1-EditMode.xml, M1-PlayMode.xml, their logs, M1-Windows-build.log, and M1-player-startup.log. Validation ran from an isolated clone of source 805f45d with only the four changed C# files overlaid. These are working-source changes; no new commit or tag was created.

Commands used (absolute Editor and output paths were supplied during execution):

```text
Unity.exe -batchmode -nographics -projectPath <validation-checkout> -runTests -testPlatform EditMode -testResults <results.xml> -logFile <test.log>
Unity.exe -batchmode -nographics -projectPath <validation-checkout> -runTests -testPlatform PlayMode -testResults <results.xml> -logFile <test.log>
Unity.exe -batchmode -nographics -quit -projectPath <validation-checkout> -buildTarget Win64 -executeMethod GhostOfKurogane.Editor.PrototypeBuild.BuildWindowsBaseline -logFile <build.log>
GhostOfKurogane.exe -batchmode -nographics -logFile <player-startup.log>
```

The manual steps above remain pending. These results establish the first quality increment, not the full M1 definition of done.