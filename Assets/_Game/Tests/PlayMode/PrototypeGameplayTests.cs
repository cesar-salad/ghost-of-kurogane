using System.Collections;
using GhostOfKurogane.CameraSystem;
using GhostOfKurogane.Interaction;
using GhostOfKurogane.Player;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.TestTools;

namespace GhostOfKurogane.Tests
{
    public sealed class PrototypeGameplayTests
    {
        private GameObject root;
        private Transform actor;
        private Transform view;
        private Keyboard keyboard;
        private float previousCaptureDeltaTime;
        private InputSettings.BackgroundBehavior previousBackgroundBehavior;
#if UNITY_EDITOR
        private InputSettings.EditorInputBehaviorInPlayMode previousEditorInputBehavior;
#endif

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            previousCaptureDeltaTime = Time.captureDeltaTime;
            Time.captureDeltaTime = 1f / 60f;
            // Synthetic devices need explicit focus-independent routing in headless Editor tests.
            previousBackgroundBehavior = InputSystem.settings.backgroundBehavior;
            InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
#if UNITY_EDITOR
            previousEditorInputBehavior = InputSystem.settings.editorInputBehaviorInPlayMode;
            InputSystem.settings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
#endif
            keyboard = InputSystem.AddDevice<Keyboard>();
            root = new GameObject("Test Rig");
            root.SetActive(false);
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.transform.SetParent(root.transform);
            floor.transform.position = new Vector3(0f, -0.5f, 0f);
            floor.transform.localScale = new Vector3(50f, 1f, 50f);
            var player = new GameObject("Test Player");
            player.transform.SetParent(root.transform);
            actor = player.transform;
            actor.position = new Vector3(0f, 0.1f, 0f);
            var controller = player.AddComponent<CharacterController>();
            controller.height = 2f;
            controller.radius = 0.4f;
            controller.center = Vector3.up;
            controller.minMoveDistance = 0f;
            var reader = player.AddComponent<PlayerInputReader>();
            var camera = new GameObject("Test Camera");
            camera.transform.SetParent(root.transform);
            view = camera.transform;
            var orbit = camera.AddComponent<ThirdPersonCamera>();
            orbit.Configure(actor, reader);
            player.AddComponent<PlayerController>().Configure(reader, view);
            player.AddComponent<PlayerInteractor>();
            root.SetActive(true);
            Physics.SyncTransforms();
            for (int frame = 0; frame < 30; frame++) yield return null;
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Object.Destroy(root);
            InputSystem.RemoveDevice(keyboard);
            InputSystem.settings.backgroundBehavior = previousBackgroundBehavior;
#if UNITY_EDITOR
            InputSystem.settings.editorInputBehaviorInPlayMode = previousEditorInputBehavior;
#endif
            Time.captureDeltaTime = previousCaptureDeltaTime;
            yield return null;
        }

        private void Press(params Key[] keys) => InputSystem.QueueStateEvent(keyboard, new KeyboardState(keys));

        [UnityTest]
        public IEnumerator InputReaderCanBeDisabledAndReenabled()
        {
            var reader = actor.GetComponent<PlayerInputReader>();
            reader.enabled = false;
            Vector3 start = actor.position;
            Press(Key.W);
            for (int frame = 0; frame < 10; frame++) yield return null;
            Assert.That(actor.position.z, Is.EqualTo(start.z).Within(0.01f));
            reader.enabled = true;
            for (int frame = 0; frame < 20; frame++) yield return null;
            Assert.That(actor.position.z - start.z, Is.GreaterThan(0.5f));
        }
        [UnityTest]
        public IEnumerator WalkSprintAndCameraFollow()
        {
            Vector3 start = actor.position;
            Vector3 cameraStart = view.position;
            Press(Key.W);
            for (int frame = 0; frame < 20; frame++) yield return null;
            float walked = actor.position.z - start.z;
            float sprintStart = actor.position.z;
            Press(Key.W, Key.LeftShift);
            for (int frame = 0; frame < 20; frame++) yield return null;
            float sprinted = actor.position.z - sprintStart;
            Assert.That(walked, Is.GreaterThan(0.5f));
            Assert.That(sprinted, Is.GreaterThan(walked * 1.6f));
            Assert.That(view.position.z - cameraStart.z, Is.GreaterThan(1f));
            Assert.That(actor.GetComponent<PlayerController>().IsGrounded, Is.True);
        }

        [UnityTest]
        public IEnumerator JumpLeavesGroundAndLands()
        {
            Assert.That(actor.GetComponent<PlayerController>().IsGrounded, Is.True);
            float startHeight = actor.position.y;
            Press(Key.Space);
            for (int frame = 0; frame < 5; frame++) yield return null;
            Assert.That(actor.position.y, Is.GreaterThan(startHeight + 0.2f));
            Assert.That(actor.GetComponent<PlayerController>().IsGrounded, Is.False);
            Press();
            for (int frame = 0; frame < 90; frame++) yield return null;
            Assert.That(actor.GetComponent<PlayerController>().IsGrounded, Is.True);
            Assert.That(actor.position.y, Is.EqualTo(startHeight).Within(0.1f));
        }

        [UnityTest]
        public IEnumerator MovementUsesAssignedCameraDirection()
        {
            view.GetComponent<ThirdPersonCamera>().enabled = false;
            view.rotation = Quaternion.Euler(0f, 90f, 0f);
            Vector3 start = actor.position;
            Press(Key.W);
            for (int frame = 0; frame < 20; frame++) yield return null;
            Assert.That(actor.position.x - start.x, Is.GreaterThan(0.5f));
            Assert.That(Mathf.Abs(actor.position.z - start.z), Is.LessThan(0.1f));
        }

        [UnityTest]
        public IEnumerator InteractionUsesInputAndWallsBlockTarget()
        {
            var terminal = GameObject.CreatePrimitive(PrimitiveType.Cube);
            terminal.transform.SetParent(root.transform);
            terminal.transform.position = new Vector3(0f, 1f, 2f);
            var interactable = terminal.AddComponent<ExampleInteractable>();
            Physics.SyncTransforms();
            yield return null;
            Assert.That(actor.GetComponent<PlayerInteractor>().Current, Is.SameAs(interactable));
            Press(Key.E);
            yield return null;
            yield return null;
            Assert.That(interactable.InteractionCount, Is.EqualTo(1));
            Press();
            yield return null;
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.transform.SetParent(root.transform);
            wall.transform.position = new Vector3(0f, 1f, 1f);
            wall.transform.localScale = new Vector3(1f, 2f, 0.1f);
            Physics.SyncTransforms();
            yield return null;
            Assert.That(actor.GetComponent<PlayerInteractor>().Current, Is.Null);
            Press(Key.E);
            yield return null;
            yield return null;
            Assert.That(interactable.InteractionCount, Is.EqualTo(1));
        }
    }
}
