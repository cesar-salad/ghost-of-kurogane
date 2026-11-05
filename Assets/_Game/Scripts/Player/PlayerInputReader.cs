using UnityEngine;
using UnityEngine.InputSystem;

namespace GhostOfKurogane.Player
{
    /// <summary>Owns Inspector-editable Input Actions and exposes device-independent commands.</summary>
    [DisallowMultipleComponent]
    public sealed class PlayerInputReader : MonoBehaviour
    {
        [SerializeField] private InputAction move = CreateMove();
        [SerializeField] private InputAction look = new InputAction("Look", InputActionType.Value, "<Mouse>/delta");
        [SerializeField] private InputAction jump = new InputAction("Jump", InputActionType.Button, "<Keyboard>/space");
        [SerializeField] private InputAction sprint = new InputAction("Sprint", InputActionType.Button, "<Keyboard>/leftShift");
        [SerializeField] private InputAction interact = new InputAction("Interact", InputActionType.Button, "<Keyboard>/e");
        [SerializeField] private InputAction releaseCursor = new InputAction("ReleaseCursor", InputActionType.Button, "<Keyboard>/escape");
        [SerializeField] private InputAction captureCursor = new InputAction("CaptureCursor", InputActionType.Button, "<Mouse>/leftButton");
        public Vector2 Move => isActiveAndEnabled ? Vector2.ClampMagnitude(move.ReadValue<Vector2>(), 1f) : Vector2.zero;
        /// <summary>Mouse displacement per frame; consumers must not multiply this by deltaTime.</summary>
        public Vector2 Look => isActiveAndEnabled ? look.ReadValue<Vector2>() : Vector2.zero;
        public bool JumpPressed => isActiveAndEnabled && jump.WasPressedThisFrame();
        public bool SprintHeld => isActiveAndEnabled && sprint.IsPressed();
        public bool InteractPressed => isActiveAndEnabled && interact.WasPressedThisFrame();
        public bool ReleaseCursorPressed => isActiveAndEnabled && releaseCursor.WasPressedThisFrame();
        public bool CaptureCursorPressed => isActiveAndEnabled && captureCursor.WasPressedThisFrame();
        private static InputAction CreateMove()
        {
            var action = new InputAction("Move", InputActionType.Value);
            action.AddCompositeBinding("2DVector").With("Up", "<Keyboard>/w").With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a").With("Right", "<Keyboard>/d");
            action.AddBinding("<Gamepad>/leftStick");
            return action;
        }
        private InputAction[] Actions => new[] { move, look, jump, sprint, interact, releaseCursor, captureCursor };
        private void OnEnable() { foreach (var action in Actions) action.Enable(); }
        private void OnDisable() { foreach (var action in Actions) action.Disable(); }
        private void OnDestroy() { foreach (var action in Actions) action.Dispose(); }
    }
}