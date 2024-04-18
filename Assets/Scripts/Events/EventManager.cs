using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static readonly PlayerEvents Player = new PlayerEvents();
    public class PlayerEvents
    {
        public UnityAction<Component, float> OnHealthChanged;
        public UnityAction<Component> OnTakingDamage;
        public UnityAction<Component> OnNoTakingDamage;
        public UnityAction<Component, float> OnSpeedChanged;
    }
    
}
