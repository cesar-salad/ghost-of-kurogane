using UnityEngine;

namespace GhostOfKurogane.Interaction
{
    /// <summary>Contract for a nearby object receiving an interaction from an actor.</summary>
    public interface IInteractable
    {
        string Prompt { get; }
        void Interact(GameObject actor);
    }
}
