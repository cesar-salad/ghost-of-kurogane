using GhostOfKurogane.Interaction;
using UnityEngine;

namespace GhostOfKurogane.UI
{
    /// <summary>Temporary IMGUI instructions for the playground; replace with production UI later.</summary>
    public sealed class PrototypeHud : MonoBehaviour
    {
        [SerializeField] private PlayerInteractor interactor;
        [SerializeField, Min(0.1f)] private float _feedbackDuration = 2.5f;
        [SerializeField] private string _interactionFeedback = "Interaction complete.";
        private PlayerInteractor _subscribedInteractor;
        private float _feedbackUntil;

        /// <summary>The temporary interaction result, or empty when no notification is active.</summary>
        public string FeedbackMessage => Time.unscaledTime < _feedbackUntil ? _interactionFeedback : string.Empty;

        public void Configure(PlayerInteractor playerInteractor)
        {
            Unsubscribe();
            interactor = playerInteractor;
            if (isActiveAndEnabled) Subscribe();
        }

        private void OnEnable() => Subscribe();
        private void OnDisable()
        {
            Unsubscribe();
            _feedbackUntil = 0f;
        }

        private void Subscribe()
        {
            if (interactor == null || _subscribedInteractor != null) return;
            _subscribedInteractor = interactor;
            _subscribedInteractor.InteractionPerformed += ShowInteractionFeedback;
        }

        private void Unsubscribe()
        {
            if (_subscribedInteractor != null)
                _subscribedInteractor.InteractionPerformed -= ShowInteractionFeedback;
            _subscribedInteractor = null;
        }

        private void ShowInteractionFeedback() => _feedbackUntil = Time.unscaledTime + _feedbackDuration;
        private void OnGUI()
        {
            GUI.Box(new Rect(12f, 12f, 490f, 90f),
                "Ghost of Kurogane - Prototype\nWASD: move | Shift: sprint | Space: jump\nMouse: orbit | Esc: release cursor | Left click: capture");
            if (interactor != null && interactor.Current != null)
                GUI.Box(new Rect(12f, 110f, 490f, 35f), interactor.Current.Prompt);
            if (!string.IsNullOrEmpty(FeedbackMessage))
                GUI.Box(new Rect(12f, 153f, 490f, 35f), FeedbackMessage);
        }
    }
}
