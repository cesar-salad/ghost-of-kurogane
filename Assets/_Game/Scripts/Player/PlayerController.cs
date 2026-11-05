using UnityEngine;

namespace GhostOfKurogane.Player
{
    /// <summary>Camera-relative CharacterController locomotion, independent of device bindings.</summary>
    [RequireComponent(typeof(CharacterController), typeof(PlayerInputReader))]
    [DisallowMultipleComponent]
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerInputReader input;
        [SerializeField] private Transform movementCamera;
        [SerializeField, Min(0f)] private float walkSpeed = 3f;
        [SerializeField, Min(0f)] private float sprintSpeed = 6f;
        [SerializeField, Min(0.01f)] private float rotationSmoothTime = 0.12f;
        [SerializeField, Min(0f)] private float jumpHeight = 1.3f;
        [SerializeField] private float gravity = -22f;
        [SerializeField] private float groundedVelocity = -2f;
        [SerializeField, Min(1f)] private float terminalSpeed = 40f;
        private CharacterController controller;
        private float verticalVelocity;
        private float rotationVelocity;
        public bool IsGrounded => controller != null && controller.isGrounded;
        /// <summary>Assigns explicit scene dependencies during prototype construction.</summary>
        public void Configure(PlayerInputReader reader, Transform cameraTransform)
        {
            input = reader;
            movementCamera = cameraTransform;
        }
        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            if (input == null) input = GetComponent<PlayerInputReader>();
            if (movementCamera == null && UnityEngine.Camera.main != null) movementCamera = UnityEngine.Camera.main.transform;
            if (movementCamera == null)
            {
                Debug.LogError("PlayerController requires a movement camera.", this);
                enabled = false;
            }
        }
        private void Update()
        {
            if (!controller.enabled) return;
            Vector3 forward = Vector3.ProjectOnPlane(movementCamera.forward, Vector3.up).normalized;
            if (forward.sqrMagnitude < 0.001f) forward = Vector3.ProjectOnPlane(movementCamera.up, Vector3.up).normalized;
            Vector3 right = Vector3.Cross(Vector3.up, forward);
            Vector2 command = input.Move;
            Vector3 direction = forward * command.y + right * command.x;
            if (direction.sqrMagnitude > 0.001f)
            {
                float targetYaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float yaw = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetYaw, ref rotationVelocity, rotationSmoothTime);
                transform.rotation = Quaternion.Euler(0f, yaw, 0f);
            }
            if (controller.isGrounded && verticalVelocity < 0f) verticalVelocity = groundedVelocity;
            if (controller.isGrounded && input.JumpPressed) verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            verticalVelocity = Mathf.Max(verticalVelocity + gravity * Time.deltaTime, -terminalSpeed);
            controller.Move((direction * (input.SprintHeld ? sprintSpeed : walkSpeed) + Vector3.up * verticalVelocity) * Time.deltaTime);
        }
        private void OnValidate()
        {
            gravity = Mathf.Min(-0.01f, gravity);
            groundedVelocity = Mathf.Min(-0.01f, groundedVelocity);
        }
    }
}
