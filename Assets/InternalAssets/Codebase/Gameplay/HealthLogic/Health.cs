using UniRx;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.HealthLogic
{
    public class Health
    {
        public ReactiveProperty<float> ReactiveHealthRatio { get; private set; } = new(1f);
        public float MaxHealth { get; private set;}
        public float CurrentHealth { get; private set;}

        private bool _isHealthEmpty = true;
        
        public Health() { }
        
        public void Initialize(float baseHealthPoints)
        {
            _isHealthEmpty = false;
            CurrentHealth = MaxHealth = baseHealthPoints;
        }

        public void ApplyPercentFromHealth(float percent)
        {
            if(_isHealthEmpty) return;
            
            ApplyHealth((long)Mathf.Clamp(MaxHealth * (percent * 0.01f), 0f, MaxHealth));
        }

        public void ApplyHealth(float healthValue)
        {
            if(_isHealthEmpty) return;

            CurrentHealth = (long) Mathf.Clamp((CurrentHealth + healthValue), 0f, MaxHealth);
            
            ReactiveHealthRatio.Value = (CurrentHealth / MaxHealth);

            if (CurrentHealth <= 0) 
                _isHealthEmpty = true;
        }
    }
}