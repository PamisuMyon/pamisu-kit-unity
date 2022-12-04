using System;
using UnityEngine;

namespace Pamisu.TopDownShooter
{
    public class ActorAttributes : MonoBehaviour
    {
        public float Health;
        public float MaxHealth;
        public int Life;

        public event Action<float, float, float> OnHealthChanged;
        public event Action OnDied;
        public bool IsDied { get; private set; }

        public void HealthChange(float delta)
        {
            var oldHealth = Health;
            Health = Mathf.Clamp(Health += delta, 0, MaxHealth);
            var actualDelta = Health - oldHealth;

            OnHealthChanged?.Invoke(actualDelta, Health, MaxHealth);
            
            if (actualDelta < 0 && Health <= 0)
            {
                IsDied = true;
                OnDied?.Invoke();
            }
        }
        
    }
}