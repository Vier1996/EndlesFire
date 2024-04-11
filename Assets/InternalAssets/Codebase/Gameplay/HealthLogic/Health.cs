using System;
using UniRx;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.HealthLogic
{
    public class Health
    {
        public event Action HealthEmpty;
        public event Action HealthChanged;
        
        public readonly ReactiveProperty<float> ReactiveHealthRatio;
        public float HealthPercent => (float)Math.Round((float) _currentHealth / _maxHealth, 2);
        public float HealthRatio => (float) (1f - HealthPercent);
        public float HealthSliderPoint => (float)(_currentHealth / _maxHealth);
        
        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;
        
        private float _maxHealth = default;
        private float _currentHealth = default;

        public Health(float baseHealthPoints, float actualPercent)
        {
            _maxHealth = baseHealthPoints;
            _currentHealth = _maxHealth * actualPercent;

            ReactiveHealthRatio = new ReactiveProperty<float>(1f);
        }

        public void ApplyPercentFromHealth(float percent)
        {
            percent /= 100;
            
            long amount = (long) Mathf.Clamp((float) _maxHealth * percent, 0f, (float) _maxHealth);

            ApplyHealth(amount);
        }
        
        public void ApplyHealth(float healthValue)
        {
            float actualValue = (long) Mathf.Clamp(((float) _currentHealth + (float) healthValue), 0f, (float) _maxHealth);
            
            _currentHealth = actualValue;

            ReactiveHealthRatio.Value = 1f - HealthRatio;

            if (_currentHealth <= 0)
                HealthEmpty?.Invoke();
            else 
                HealthChanged?.Invoke();
        }

        public void UpdateHealthValues(float maxHealth, float currentHealth)
        {
            _maxHealth = maxHealth;
            ApplyHealth(currentHealth - _currentHealth);
            _currentHealth = currentHealth;
        }
        
        public void IncreaseHealth() { }
        
        public void DecreaseHealth() { }
    }
}