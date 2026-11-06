using UnityEngine;
using UnityEngine.InputSystem;

namespace GhostOfKurogane.Player
{
    /// <summary>Owns Inspector-editable Input Actions and exposes device-independent commands.</summary>
    [DisallowMultipleComponent]
    public sealed class PlayerInputReader : MonoBehaviour
    {
        [SerializeField] private InputAction move;
        [SerializeField] private InputAction look;
        [SerializeField] private InputAction jump;
        [SerializeField] private InputAction sprint;
        [SerializeField] private InputAction interact;
        [SerializeField] private InputAction releaseCursor;
        [SerializeField] private InputAction captureCursor;
        public Vector2 Move => isActiveAndEnabled ? Vector2.ClampMagnitude(move.ReadValue<Vector2>(), 1f) : Vector2.zero;
        /// <summary>Mouse displacement per frame; consumers must not multiply this by deltaTime.</summary>
        public Vector2 Look => isActiveAndEnabled ? look.ReadValue<Vector2>() : Vector2.zero;
        public bool JumpPressed => isActiveAndEnabled && jump.WasPressedThisFrame();
        public bool SprintHeld => isActiveAndEnabled && sprint.IsPressed();
        public bool InteractPressed => isActiveAndEnabled && interact.WasPressedThisFrame();
        public bool ReleaseCursorPressed => isActiveAndEnabled && releaseCursor.WasPressedThisFrame();
        public bool CaptureCursorPressed => isActiveAndEnabled && captureCursor.WasPressedThisFrame();
        // Binding setup can initialize InputSystem and query Unity resources. Keep it out of
        // constructors/field initializers, which also run while a player scene is deserializing.
        private void Reset() => InitializeActions();
        private void Awake() => InitializeActions();

        private void InitializeActions()
        {
            // Preserve named serialized actions, including deliberately unbound actions.
            if (move == null || string.IsNullOrEmpty(move.name)) move = CreateMove();
            if (look == null || string.IsNullOrEmpty(look.name)) look = new InputAction("Look", InputActionType.Value, "<Mouse>/delta");
            if (jump == null || string.IsNullOrEmpty(jump.name)) jump = new InputAction("Jump", InputActionType.Button, "<Keyboard>/space");
            if (sprint == null || string.IsNullOrEmpty(sprint.name)) sprint = new InputAction("Sprint", InputActionType.Button, "<Keyboard>/leftShift");
            if (interact == null || string.IsNullOrEmpty(interact.name)) interact = new InputAction("Interact", InputActionType.Button, "<Keyboard>/e");
            if (releaseCursor == null || string.IsNullOrEmpty(releaseCursor.name)) releaseCursor = new InputAction("ReleaseCursor", InputActionType.Button, "<Keyboard>/escape");
            if (captureCursor == null || string.IsNullOrEmpty(captureCursor.name)) captureCursor = new InputAction("CaptureCursor", InputActionType.Button, "<Mouse>/leftButton");
        }
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