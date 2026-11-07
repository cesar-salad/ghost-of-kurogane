using GhostOfKurogane.Player;
using UnityEngine;

namespace GhostOfKurogane.CameraSystem
{
    /// <summary>Mouse orbit camera with smooth tracking and world-geometry collision handling.</summary>
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
        [Header("Collision")]
        [SerializeField] private LayerMask _collisionMask = ~0;
        [SerializeField, Min(0.01f)] private float _collisionRadius = 0.25f;
        [SerializeField, Min(0f)] private float _collisionPadding = 0.05f;
        [SerializeField, Min(0.01f)] private float _distanceRecoveryTime = 0.2f;
        private readonly RaycastHit[] _collisionHits = new RaycastHit[32];
        private float _currentDistance;
        private float _distanceVelocity;
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
            pivotVelocity = Vector3.zero;
            _distanceVelocity = 0f;
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
                _currentDistance = distance;
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
            Vector3 backward = rotation * Vector3.back;
            float clearDistance = GetClearDistance(backward);
            // Contract immediately so damping never carries the camera through a blocking wall.
            // Only the return to the desired orbit distance is smoothed.
            if (clearDistance <= _currentDistance)
            {
                _currentDistance = clearDistance;
                _distanceVelocity = 0f;
            }
            else
            {
                _currentDistance = Mathf.Min(clearDistance, Mathf.SmoothDamp(_currentDistance,
                    clearDistance, ref _distanceVelocity, _distanceRecoveryTime));
            }
            transform.SetPositionAndRotation(pivot + backward * _currentDistance, rotation);
        }

        private float GetClearDistance(Vector3 direction)
        {
            int count = Physics.SphereCastNonAlloc(pivot, _collisionRadius, direction,
                _collisionHits, distance, _collisionMask, QueryTriggerInteraction.Ignore);
            // A full buffer may omit the closest obstacle. Stay at the pivot rather than
            // trusting an incomplete result in unusually dense geometry.
            if (count == _collisionHits.Length) return 0f;
            float clearDistance = distance;
            for (int index = 0; index < count; index++)
            {
                Transform hitTransform = _collisionHits[index].collider.transform;
                if (hitTransform == target || hitTransform.IsChildOf(target)) continue;
                clearDistance = Mathf.Min(clearDistance, Mathf.Max(0f,
                    _collisionHits[index].distance - _collisionPadding));
            }
            return clearDistance;
        }

        private static void CaptureCursor() { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
        private static void ReleaseCursor() { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
        private void OnApplicationFocus(bool focused) { if (!focused) ReleaseCursor(); }
        private void OnDisable() { Cursor.lockState = previousLock; Cursor.visible = previousVisibility; }
    }
}
