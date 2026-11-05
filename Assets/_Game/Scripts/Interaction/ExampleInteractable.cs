using UnityEngine;
using UnityEngine.Events;
namespace GhostOfKurogane.Interaction
{
    /// <summary>Prototype terminal demonstrating interaction and Inspector event wiring.</summary>
    public sealed class ExampleInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private string prompt = "E: Access terminal";
        [SerializeField] private UnityEvent interacted = new UnityEvent();
        public string Prompt => prompt;
        public int InteractionCount { get; private set; }
        public void Interact(GameObject actor)
        {
            if (actor == null) return;
            InteractionCount++;
            Debug.Log($"{actor.name} accessed {name} ({InteractionCount}).", this);
            interacted.Invoke();
        }
    }
}
