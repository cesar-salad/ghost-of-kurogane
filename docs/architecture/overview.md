# Initial architecture

Three assemblies keep compilation boundaries explicit: Runtime depends only on Unity and Input System; Editor references Runtime plus URP to generate assets; test assemblies reference Runtime and Unity's test tooling. Editor code is excluded from player builds. All components live under GhostOfKurogane namespaces.

## Component communication

```text
Inspector-editable Input Actions
              |
       PlayerInputReader
        /      |       \
PlayerController  PlayerInteractor  ThirdPersonCamera
       |               |                 |
CharacterController  IInteractable    Camera Transform
                       |
                ExampleInteractable
                       |
                 Inspector UnityEvent

Health -> HealthChanged(current, maximum), Died
GameManager -> StateChanged(state)
PlayerInteractor.Current -> PrototypeHud prompt
```

`PlayerInputReader` owns the lifetime of embedded, serialized Input Actions, enables/disables them with the component, and exposes commands. The default bindings are configuration in this adapter, never device checks in gameplay. These actions can later move into a shared `.inputactions` asset when rebinding, control schemes, or multiple characters need it. Mouse look is displacement; do not multiply it by delta time. Future stick look should be a separate angular velocity channel with delta-time scaling.

`PlayerController` projects the assigned camera basis onto the ground plane and combines it with normalized analog movement. One CharacterController.Move call integrates planar motion and vertical velocity. Grounding comes from the last collision result; gravity keeps contact with floors. Sprint, jump height, gravity, rotation smoothing, and terminal fall speed are Inspector values. No Rigidbody or Animator is required. Dependencies are serialized; the setup utility wires them explicitly. A MainCamera lookup is a one-time manual-assembly fallback.

`ThirdPersonCamera` runs in LateUpdate after locomotion. Its yaw and pitch are independent of the player root, preventing camera-relative movement feedback. It smooths the follow pivot, maintains orbit distance, and owns cursor capture/focus behavior. It is replaceable by Cinemachine without touching movement. It sphere-casts toward the desired orbit position, contracts immediately before blocking geometry, and smooths distance recovery. A cached hit buffer ignores trigger volumes and follow-target colliders. It remains a basic collision fallback, not an embedded-pivot or complete visibility solver.

`PlayerInteractor` casts from the player's chest along the player's forward vector using a configurable distance and layer mask. The first non-trigger collider blocks the ray. It resolves enabled IInteractable components on that collider or ancestors and calls Interact with the actor. The HUD reads the same target, so prompts match interaction. The player's own CharacterController contains the ray origin, so Unity does not report it as a hit. Custom compound actors should exclude their extra colliders via interaction layers.

`ExampleInteractable` tracks a runtime access count, logs receipt, and invokes a UnityEvent for local Inspector composition. It is not a terminal or dialogue framework.

`Health` initializes full in Awake. Damage and healing reject non-positive/non-finite values, clamp to bounds, and notify only effective changes. Death follows the final health-change notification and fires once. Dead actors ignore damage and healing. It does not destroy actors or assume an enemy/player type. Subscribers should pair subscriptions with unsubscriptions. Maximum health is authoring configuration, not a runtime mutation API.

`GameManager` belongs to the scene. Start explicitly transitions from Initializing to Playing; repeated state assignments do not emit duplicates. Pause is only a notification hook; no global timescale or input mutation is hidden inside it. There is no persistent singleton, service locator, save manager, or event bus.

## Editor ownership and safety

`PrototypeSceneBuilder` uses Unity APIs to create the scene, renderer, and URP assets. Each asset path is unique, existing pipeline settings are preserved, and the scene command prompts to save dirty work. It sets text serialization and visible metas. It does not alter the Build Profile list. Never call the builder in a project that is in Play mode.

Only local project configuration and source are versioned. Library, Temp, Logs, generated IDE files, and builds remain ignored. Unity generates all `.meta` GUIDs and serialized scene/settings assets. Binary source art uses Git LFS; text scenes and metas remain normal Git text.

## Extension boundaries

Future animation can observe motion; combat can use Health without embedding damage rules in locomotion; NPCs and story objects can implement IInteractable. These are extension points, not prebuilt systems. Reserved directories are organizational placeholders only.


## Input initialization and standalone builds

Default input actions are populated in Reset (Editor authoring) and Awake (runtime fallback), preserving existing named serialized actions. Binding construction must not run in a MonoBehaviour field initializer: standalone scene deserialization can trigger Input System resource queries before Unity permits them. PrototypeBuild creates uniquely named Windows development builds from the saved playground using an explicit scene list and writes a build-evidence.txt report. It does not mutate the configured Build Profile scene list.


## M1 interaction feedback

PlayerInteractor now emits InteractionPerformed after its detected target''s Interact method returns. PrototypeHud subscribes/unsubscribes with its component lifecycle (and handles Configure after OnEnable), then presents a short Inspector-configured confirmation using unscaled time. Gameplay does not depend on the HUD. This notification confirms dispatch of the current void interaction contract; it is not a dialogue result or general success/failure system.
