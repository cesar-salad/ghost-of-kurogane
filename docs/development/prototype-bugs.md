# Prototype issues - M0 baseline

Baseline: da8cb5e, audited 2026-11-05. P0 = blocker/crash/prevents evaluating the prototype; P1 = major reliability issue; P2 = feedback or polish issue. Static findings are clearly distinguished from manually reproduced findings. A standalone-only P0 (GOK-005) was found and fixed during this audit. Visual standalone acceptance is tracked separately.

| ID | Priority | Finding / reproduction | Evidence | Next action |
| --- | --- | --- | --- | --- |
| GOK-001 | P1 | Walk off the finite ground plane; there is no return path or recovery. | Static: scene has a finite Plane; PlayerController has gravity/terminal velocity but no bounds/respawn. Manual repro pending. | M1: choose a simple boundary or reset volume, then test recovery. |
| GOK-002 | P1 | Orbit beside/behind the wall; the fallback camera can enter solid geometry. | Static: ThirdPersonCamera places the camera at a fixed orbit distance without collision queries. Manual repro pending. | M1: add minimal camera collision or adopt a verified Cinemachine setup. |
| GOK-003 | P2 | Press E at the terminal; a standalone player has no visible success indication. | Static + passing access-count test: ExampleInteractable only logs receipt and invokes an empty UnityEvent; HUD shows only the current prompt. | M1: provide a short visible response without implementing dialogue yet. |
| GOK-004 | P2 | Connect a controller; movement binding exists but look/jump/sprint/interact bindings are absent. | Static binding audit. Full controller support is not claimed. | M1: decide control schemes/rebinding approach and test a complete device flow. |

Animation, combat, dialogue, saving, and progression are intentionally absent and are not bug tickets for M0. Do not fix the above by expanding scope during the baseline audit. Add actual visual/input/build findings here after the manual smoke pass, with clear reproduction steps and severity.

## Findings from the Windows baseline

| ID | Priority | Status | Finding and evidence | Next action |
| --- | --- | --- | --- | --- |
| GOK-005 | P0 | Fixed; headless startup verified | Initial executable logged InputSystem TypeInitializationException because PlayerInputReader.CreateMove ran in a field initializer during scene load. Default setup now runs in Reset/Awake and preserves serialized bindings. Rebuilt with 0 errors/warnings; 7 EditMode + 5 PlayMode tests pass; corrected player loads without the exception. | Include normal keyboard/mouse acceptance in the standalone smoke pass before tagging. |
| GOK-006 | P2 | Open; environment-dependent behavior unverified | Corrected headless startup logged an allocator lifetime diagnostic (4 permitted frames, allocation age 6). No managed exception/assertion followed and process remained running. Cause is unknown; no gameplay source fix is justified yet. | Capture whether it repeats in a normal rendered launch; retain Player log and escalate investigation if persistent or accompanied by a fault. |

## M1 follow-up - 2026-11-06

The M0 rows above describe the tagged historical baseline. Current working-source updates:

- **GOK-002:** Basic camera collision fallback implemented. PlayMode tests verify wall clearance, smooth recovery, and trigger/follow-target filtering. Embedded pivots, near-plane corners, and normal-rendered camera feel remain manual acceptance items.
- **GOK-003:** Timed HUD confirmation implemented through PlayerInteractor.InteractionPerformed. PlayMode coverage verifies one notification per access, blocked-target suppression, and expiration. Visual placement/readability remain manual acceptance items.
- **GOK-001 / GOK-004 / GOK-006:** Remain open. Boundary recovery, full gamepad support, and the prior allocator diagnostic are outside this first quality pass.

See [M1 quality-pass smoke steps](m1-quality-pass.md). No new visual acceptance is inferred from automated results.
