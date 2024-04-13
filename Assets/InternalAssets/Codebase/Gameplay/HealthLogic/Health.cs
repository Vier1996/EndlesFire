using System;
using UniRx;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.HealthLogic
{
    public class Health
    {
        public event Action HealthEmpty;
        public event Action HealthChanged;

        public ReactiveProperty<float> ReactiveHealthRatio { get; private set; }
        public float MaxHealth { get; private set;}
        public float CurrentHealth { get; private set;}

        public Health() { }
        
        public void Initialize(float baseHealthPoints)
        {
            CurrentHealth = MaxHealth = baseHealthPoints;
            ReactiveHealthRatio = new ReactiveProperty<float>(1f);
        }

        public void ApplyPercentFromHealth(float percent) => 
            ApplyHealth((long) Mathf.Clamp(MaxHealth * (percent * 0.01f), 0f, MaxHealth));

        public void ApplyHealth(float healthValue)
        {
            CurrentHealth = (long) Mathf.Clamp((CurrentHealth + healthValue), 0f, MaxHealth);
            
            ReactiveHealthRatio.Value = (CurrentHealth / MaxHealth);

            if (CurrentHealth <= 0) HealthEmpty?.Invoke();
            else HealthChanged?.Invoke();
        }
    }
}