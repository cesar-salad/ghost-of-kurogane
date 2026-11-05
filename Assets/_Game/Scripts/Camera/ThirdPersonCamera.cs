using GhostOfKurogane.Player;
using UnityEngine;

namespace GhostOfKurogane.CameraSystem
{
    /// <summary>Small mouse orbit fallback used until Cinemachine is installed.</summary>
    [DisallowMultipleComponent]
    public sealed class ThirdPersonCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private PlayerInputReader input;
        [SerializeField] private Vector3 targetOffset = new Vector3(0f, 1.5f, 0f);
        [SerializeField, Min(0.1f)] private float distance = 5f;
        [SerializeField, Min(0.01f)] private float trackingSmoothTime = 0.08f;
        [SerializeField, Min(0f)] private float mouseSensitivity = 0.15f;
        [SerializeField] private float minimumPitch = -25f;
        [SerializeField] private float maximumPitch = 65f;
        private float yaw;
        private float pitch = 15f;
        private Vector3 pivot;
        private Vector3 pivotVelocity;
        private CursorLockMode previousLock;
        private bool previousVisibility;
        private bool initialized;

        public void Configure(Transform followTarget, PlayerInputReader reader)
        {
            target = followTarget;
            input = reader;
        }

        private void OnEnable()
        {
            previousLock = Cursor.lockState;
            previousVisibility = Cursor.visible;
            initialized = false;
        }

        private void Start()
        {
            if (target == null || input == null)
            {
                Debug.LogError("ThirdPersonCamera requires a target and input reader.", this);
                enabled = false;
                return;
            }
            CaptureCursor();
        }

        private void LateUpdate()
        {
            if (target == null || input == null) return;
            if (input.ReleaseCursorPressed) ReleaseCursor();
            else if (input.CaptureCursorPressed && Application.isFocused) CaptureCursor();
            if (!initialized)
            {
                pivot = target.position + targetOffset;
                yaw = target.eulerAngles.y;
                initialized = true;
            }
            if (Cursor.lockState == CursorLockMode.Locked && Application.isFocused)
            {
                Vector2 delta = input.Look * mouseSensitivity;
                yaw += delta.x;
                pitch = Mathf.Clamp(pitch - delta.y, minimumPitch, maximumPitch);
            }
            pivot = Vector3.SmoothDamp(pivot, target.position + targetOffset,
                ref pivotVelocity, trackingSmoothTime);
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            transform.SetPositionAndRotation(pivot - rotation * Vector3.forward * distance, rotation);
        }

        private static void CaptureCursor() { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
        private static void ReleaseCursor() { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
        private void OnApplicationFocus(bool focused) { if (!focused) ReleaseCursor(); }
        private void OnDisable() { Cursor.lockState = previousLock; Cursor.visible = previousVisibility; }
    }
}
