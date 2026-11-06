# Baseline playtest notes

## Existing Editor feedback

The developer tested the initial playground locally and described it as a solid first attempt. No recording, hardware details, timings, or per-check results were supplied. This is broad positive feedback only.

## M0 standalone acceptance - basic playtest confirmed

Candidate: Builds/Windows/M0-da8cb5e-fixed/GhostOfKurogane.exe (da8cb5e plus current M0 input fix/build tooling; build UTC 2026-11-05T23:36:05Z). Record the exact build-evidence.txt path and eventual source commit when testing. The older M0-da8cb5e directory reproduces the fixed startup bug and should not be used for acceptance. Run the Windows executable directly; do not rely on Play Mode for this pass.

| Check | Result | Observation |
| --- | --- | --- |
| Standalone launch and basic visual presentation | Reported pass | Developer reports the Windows build looked good and was playable. |
| Basic controls | Reported pass | Developer reports basic commands worked; individual commands were not enumerated. |
| WASD remains camera-relative after mouse orbit; facing feels sensible | Pending | |
| Shift increases speed; Space jumps and lands on ground/obstacles | Pending | |
| Walls and obstacles block movement | Pending | |
| Terminal prompt appears only in range while facing it; E logs one access | Pending | Development Player log contains the receipt; visible confirmation is GOK-003. |
| A wall between actor and terminal blocks interaction | Pending | |
| Escape releases cursor; click re-captures; focus loss/recovery is usable | Pending | |
| Camera near wall and movement near scene edges | Pending | Record GOK-001 / GOK-002 reproduction. |
| Quit the standalone normally and inspect Player log for errors | Pending | |
| Record screenshots or 30-60 seconds of real gameplay | Pending | |

Record date, tester, Windows/GPU, resolution, build/source reference, completion or failure, and concrete reproduction steps. Store a small approved capture under docs/development/baseline/ when one exists (binary formats already use LFS), or link to its actual local/shared location. Do not create empty capture files.

Headless tests and a startup check cannot approve rendering, mouse feel, focus behavior, or the complete standalone control loop. Health event behavior is verified by automated tests; there is no combat/damage button in this baseline.

## Developer Windows playtest - 2026-11-05

The developer confirmed testing the Windows build: it looked good, was playable, and basic commands worked. This records a manual standalone visual/playability check. The candidate linked above is the build provided for testing; its exact executable path was not restated in the report. Detailed interaction, collision, cursor/focus, normal-exit/log checks, hardware details, and capture were not reported and remain pending. The allocator diagnostic is not considered resolved by this general feedback.
