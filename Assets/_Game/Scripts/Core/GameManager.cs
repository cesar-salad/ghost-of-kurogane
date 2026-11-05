using System;
using UnityEngine;
namespace GhostOfKurogane.Core
{
    public enum GameState { Initializing, Playing, Paused }
    /// <summary>Scene-owned lifecycle without global instances or persistent services.</summary>
    [DisallowMultipleComponent]
    public sealed class GameManager : MonoBehaviour
    {
        public GameState State { get; private set; } = GameState.Initializing;
        public event Action<GameState> StateChanged;
        private void Start() => BeginPlay();
        /// <summary>Completes initialization or resumes this manager's state.</summary>
        public void BeginPlay() => SetState(GameState.Playing);
        /// <summary>Signals pause intent; subscribers decide how to suspend their systems.</summary>
        public void SetPaused(bool paused) => SetState(paused ? GameState.Paused : GameState.Playing);
        private void SetState(GameState state)
        {
            if (State == state) return;
            State = state;
            StateChanged?.Invoke(state);
        }
    }
}
