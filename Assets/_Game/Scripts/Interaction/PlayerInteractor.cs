using GhostOfKurogane.Player;
using UnityEngine;

namespace GhostOfKurogane.Interaction
{
    /// <summary>Detects the first solid object in front of the actor; walls block interaction.</summary>
    [RequireComponent(typeof(PlayerInputReader))]
    public sealed class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private PlayerInputReader input;
        [SerializeField] private Vector3 originOffset = new Vector3(0f, 1f, 0f);
        [SerializeField, Min(0.1f)] private float range = 2.5f;
        [SerializeField] private LayerMask detectionMask = ~0;
        private MonoBehaviour currentBehaviour;
        public IInteractable Current => currentBehaviour != null ? currentBehaviour as IInteractable : null;
        private void Awake() { if (input == null) input = GetComponent<PlayerInputReader>(); }

        private void Update()
        {
            currentBehaviour = null;
            if (Physics.Raycast(transform.position + originOffset, transform.forward, out RaycastHit hit,
                range, detectionMask, QueryTriggerInteraction.Ignore))
            {
                foreach (var behaviour in hit.collider.GetComponentsInParent<MonoBehaviour>())
                {
                    if (behaviour is IInteractable && behaviour.isActiveAndEnabled)
                    {
                        currentBehaviour = behaviour;
                        break;
                    }
                }
            }
            if (input.InteractPressed) Current?.Interact(gameObject);
        }
    }
}
