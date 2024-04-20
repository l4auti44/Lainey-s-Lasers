using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static readonly PlayerEvents Player = new PlayerEvents();
    public static readonly GameEvents Game = new GameEvents();
    public class PlayerEvents
    {
        public UnityAction<Component, float> OnHealthChanged;
        public UnityAction<Component> OnTakingDamage;
        public UnityAction<Component> OnNoTakingDamage;
        public UnityAction<Component, float> OnSpeedChanged;
        public UnityAction<Component, PlayerMovement.MovementState> OnMovementStateChanged;

        //public UnityAction<Component> OnRunning;
    }

    public class GameEvents
    {
        public UnityAction<Component> OnWin;
    }
    
}
