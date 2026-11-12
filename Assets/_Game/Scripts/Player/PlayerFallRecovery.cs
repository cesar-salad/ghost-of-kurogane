using GhostOfKurogane.CameraSystem;
using UnityEngine;

namespace GhostOfKurogane.Player
{
    /// <summary>Playground safety net: returns a fallen actor to a configured point or its initial pose.</summary>
    [RequireComponent(typeof(PlayerController))]
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(100)]
    public sealed class PlayerFallRecovery : MonoBehaviour
    {
        [SerializeField] private float _minimumWorldY = -10f;
        [SerializeField] private Transform _returnPoint;
        [SerializeField] private ThirdPersonCamera _camera;
        private PlayerController _movement;
        private Vector3 _initialPosition;
        private Quaternion _initialRotation;

        /// <summary>Assigns the scene camera; recovery itself never changes gameplay-input ownership.</summary>
        public void Configure(ThirdPersonCamera followCamera) => _camera = followCamera;

        private void Awake()
        {
            _movement = GetComponent<PlayerController>();
            _initialPosition = transform.position;
            _initialRotation = transform.rotation;
        }

        private void Update()
        {
            if (transform.position.y >= _minimumWorldY) return;
            Vector3 returnPosition = _returnPoint != null ? _returnPoint.position : _initialPosition;
            Quaternion returnRotation = _returnPoint != null ? _returnPoint.rotation : _initialRotation;
            if (returnPosition.y <= _minimumWorldY)
            {
                Debug.LogError("PlayerFallRecovery return position must be above Minimum World Y. Recovery has been disabled.", this);
                enabled = false;
                return;
            }
            _movement.Teleport(returnPosition, returnRotation);
            if (_camera != null) _camera.SnapToTarget();
        }
    }
}