using GhostOfKurogane.Interaction;
using UnityEngine;

namespace GhostOfKurogane.UI
{
    /// <summary>Temporary IMGUI instructions for the playground; replace with production UI later.</summary>
    public sealed class PrototypeHud : MonoBehaviour
    {
        [SerializeField] private PlayerInteractor interactor;
        public void Configure(PlayerInteractor playerInteractor) => interactor = playerInteractor;
        private void OnGUI()
        {
            GUI.Box(new Rect(12f, 12f, 490f, 90f),
                "Ghost of Kurogane - Prototype\nWASD: move | Shift: sprint | Space: jump\nMouse: orbit | Esc: release cursor | Left click: capture");
            if (interactor != null && interactor.Current != null)
                GUI.Box(new Rect(12f, 110f, 490f, 35f), interactor.Current.Prompt);
        }
    }
}
