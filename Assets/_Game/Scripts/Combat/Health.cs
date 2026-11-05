using System;
using UnityEngine;

namespace GhostOfKurogane.Combat
{
    /// <summary>Reusable health with bounded changes and a single death notification per lifetime.</summary>
    [DisallowMultipleComponent]
    public sealed class Health : MonoBehaviour
    {
        [SerializeField, Min(1f)] private float maximumHealth = 100f;
        public float MaximumHealth => maximumHealth;
        public float CurrentHealth { get; private set; }
        public bool IsDead => CurrentHealth <= 0f;
        /// <summary>Emits current and maximum health after effective changes.</summary>
        public event Action<float, float> HealthChanged;
        public event Action Died;
        private void Awake() => CurrentHealth = maximumHealth;

        /// <summary>Applies finite positive damage. Dead characters ignore further damage.</summary>
        public void TakeDamage(float amount)
        {
            if (!IsValidAmount(amount) || IsDead) return;
            CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
            bool died = IsDead;
            HealthChanged?.Invoke(CurrentHealth, maximumHealth);
            if (died) Died?.Invoke();
        }

        /// <summary>Heals living characters only; resurrection is outside this component's scope.</summary>
        public void Heal(float amount)
        {
            if (!IsValidAmount(amount) || IsDead || CurrentHealth >= maximumHealth) return;
            CurrentHealth = Mathf.Min(maximumHealth, CurrentHealth + amount);
            HealthChanged?.Invoke(CurrentHealth, maximumHealth);
        }

        private static bool IsValidAmount(float amount) => amount > 0f && !float.IsInfinity(amount) && !float.IsNaN(amount);
        private void OnValidate()
        {
            if (float.IsNaN(maximumHealth) || float.IsInfinity(maximumHealth)) maximumHealth = 100f;
            maximumHealth = Mathf.Max(1f, maximumHealth);
        }
    }
}
