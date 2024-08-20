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
        public UnityAction<Component, PlayerMovementAdvanced.MovementState> OnMovementStateChanged;

        //public UnityAction<Component> OnRunning;
    }

    public class GameEvents
    {
        public UnityAction<Component> OnWin;
        public UnityAction<Component> OnHitless;
        public UnityAction<Component, string> OnObjectDestroy;
        public UnityAction<Component, int> OnTutorialTrigger;
        public UnityAction<Component> OnDie;
        
    }
    
}
