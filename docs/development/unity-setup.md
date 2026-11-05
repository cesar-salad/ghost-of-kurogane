# Unity Editor setup

The repository started with only a standard Unity `.gitignore`. Unity 6000.3.27f1 was detected locally and used with `-createProject` to generate real ProjectSettings. The generated manifest was extended with Input System 1.20.1, URP 17.3.0, and Test Framework 1.6.0, matching the installed Editor's package catalog. Existing default module entries were preserved. Unity resolves transitive dependencies and writes the lock file.

## Opening and generating

Add the root to Unity Hub and select 6000.3.27f1. Allow package import and compilation. Confirm Player > Active Input Handling is Input System Package (New), restarting after changes. Run Ghost of Kurogane > Create Prototype Playground if the generated scene is absent. It creates uniquely named URP renderer/pipeline assets only if no default pipeline is assigned, and uniquely named scenes on every invocation. Save open work when prompted. It does not create prefabs or change Build Profiles.

If the menu does not appear, fix Console compilation/package errors before trying to assemble anything. If package restore fails, check Package Manager network access. Do not hand-create scene YAML or meta GUIDs.

For manual assembly: create a Plane at origin scaled (3,1,3). Create an empty player at (0,0.1,0), add CharacterController with height 2, radius .4, center (0,1,0), and add PlayerInputReader, PlayerController, Health, PlayerInteractor. Add a capsule visual child at local (0,1,0), removing its CapsuleCollider. Add a MainCamera-tagged Camera with AudioListener and ThirdPersonCamera, then assign its target and reader and assign the camera Transform to PlayerController. Add a directional light and a cube terminal at (0,1,2) with ExampleInteractable and collider. Add GameManager and optionally PrototypeHud with the interactor reference. Configure URP using Assets > Create > Rendering > URP Asset (with Universal Renderer), assign it in Graphics and the active Quality level. Save as Prototype_Playground under Scenes/Prototypes.

## Optional Cinemachine migration

Cinemachine was not installed in the repository; no Cinemachine namespace or assembly dependency is fabricated. To adopt it:

1. Window > Package Management > Package Manager > Unity Registry: install Cinemachine. Select a released 3.1 version compatible with this Editor, and commit manifest/lock changes. Do not install the legacy 2.x package merely because an archive is bundled with the Editor.
2. Disable/remove ThirdPersonCamera on Main Camera. Add a Cinemachine Brain to that Camera.
3. Create an independent camera target Transform (not inheriting the player's rotation). Follow the player at chest/head height and rotate that target from the input reader's mouse delta, clamping pitch. This can reuse the yaw/pitch logic from the fallback component in a small target-only adapter.
4. Create a Cinemachine Camera with Third Person Follow and assign that target. Set Camera Distance to approximately 5, Shoulder Offset to taste, and per-axis Damping to start near 0.1. Set collision filtering to world geometry while excluding player colliders.
5. Keep PlayerController's movement-camera reference assigned to the real Main Camera, which the Brain drives. Verify orbit, wall collision, slopes, sprint, and jumping before removing the fallback source.
6. Replace the fallback's cursor capture with a dedicated input/cursor component when removing it. Verify escape, focus loss, and re-capture.

Third Person Follow follows the target's rotation and does not read input itself. See Unity's [Third Person Follow manual](https://docs.unity.cn/Packages/com.unity.cinemachine%403.1/manual/CinemachineThirdPersonFollow.html) and [camera guide](https://docs.unity.cn/Packages/com.unity.cinemachine%403.1/manual/ThirdPersonCameras.html). This migration is optional and has not been implemented in this foundation.

## Version control

Install Git LFS and run `git lfs install`; run `git lfs pull` after cloning. The attributes cover common binary art/audio/video formats. Use lowercase extensions consistently. Force Text serialization does not make every `.asset` file textual: add specific binary asset types to LFS when introduced. Do not apply a Unity YAML merge driver unless UnityYAMLMerge is actually configured on contributor machines. Keep `.meta` files beside every imported asset; move assets inside Unity so GUIDs survive.
