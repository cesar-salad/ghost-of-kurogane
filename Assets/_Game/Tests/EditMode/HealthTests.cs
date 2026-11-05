using GhostOfKurogane.Combat;
using GhostOfKurogane.Core;
using NUnit.Framework;
using UnityEngine;

namespace GhostOfKurogane.Tests
{
    public sealed class HealthTests
    {
        private GameObject actor;
        private Health health;
        [SetUp] public void SetUp()
        {
            actor = new GameObject("Test Actor");
            health = actor.AddComponent<Health>();
            // Explicitly invoke lifecycle in EditMode, where Awake is not guaranteed.
            typeof(Health).GetMethod("Awake", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(health, null);
        }
        [TearDown] public void TearDown() => Object.DestroyImmediate(actor);

        [Test] public void DamageAndHealingClampAndNotify()
        {
            int changes = 0;
            health.HealthChanged += (current, maximum) => { changes++; Assert.That(current, Is.InRange(0f, maximum)); };
            health.TakeDamage(30f);
            Assert.That(health.CurrentHealth, Is.EqualTo(70f));
            health.Heal(200f);
            Assert.That(health.CurrentHealth, Is.EqualTo(100f));
            Assert.That(changes, Is.EqualTo(2));
        }

        [Test] public void DeathFiresOnceAndHealingCannotResurrect()
        {
            int deaths = 0;
            health.Died += () => deaths++;
            health.TakeDamage(500f);
            health.TakeDamage(10f);
            health.Heal(100f);
            Assert.That(health.CurrentHealth, Is.Zero);
            Assert.That(deaths, Is.EqualTo(1));
        }

        [TestCase(-10f)] [TestCase(0f)] [TestCase(float.NaN)] [TestCase(float.PositiveInfinity)]
        public void InvalidAmountsDoNotChangeHealth(float amount)
        {
            health.TakeDamage(amount);
            Assert.That(health.CurrentHealth, Is.EqualTo(100f));
            health.TakeDamage(20f);
            health.Heal(amount);
            Assert.That(health.CurrentHealth, Is.EqualTo(80f));
        }

        [Test] public void GameStateNotifiesOnlyOnTransitions()
        {
            var manager = actor.AddComponent<GameManager>();
            int transitions = 0;
            manager.StateChanged += _ => transitions++;
            manager.BeginPlay();
            manager.BeginPlay();
            manager.SetPaused(true);
            Assert.That(manager.State, Is.EqualTo(GameState.Paused));
            Assert.That(transitions, Is.EqualTo(2));
        }
    }
}
