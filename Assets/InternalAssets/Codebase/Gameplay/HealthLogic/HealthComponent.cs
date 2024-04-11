using System;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.HealthLogic
{
    public class HealthComponent : MonoBehaviour
    {
        public event Action HealthEmpty;
        public event Action<float> HealthChanged;

        [SerializeField] private HealthView _currentView;

        public Health Health { get; private set; }

        public void SetupHealth(float health)
        {
            InstallHealth(health, 1);

            _currentView.Initialize(Health.HealthRatio);
            _currentView.Display();

            Health.HealthEmpty -= OnHealthEmpty;
            Health.HealthEmpty += OnHealthEmpty;
        }

        private void InstallHealth(float health, float actualPercent) => Health = new Health(health, actualPercent);

        public void SetHealthBarActive(bool value) => _currentView.gameObject.SetActive(value);
        
        public void Operate(float value)
        {
            value = -(long)(value * 1f);

            Health.ApplyHealth(value);

            if (Health.CurrentHealth <= 0)
            {
                SetHealthBarActive(false);
                return;
            }

            SetHealthBarActive(true);
            _currentView.ChangeHealthProgress(Health.HealthRatio);
            _currentView.Display();

            _currentView.PlayHealthHitAnimation();

            HealthChanged?.Invoke(value);
        }
        
        
        private void OnHealthEmpty()
        {
            Health.HealthEmpty -= OnHealthEmpty;

            HealthEmpty?.Invoke();
        }

        private void OnDisable()
        {
            if (_currentView == null)
                return;

            _currentView.Remove();
        }
    }
}