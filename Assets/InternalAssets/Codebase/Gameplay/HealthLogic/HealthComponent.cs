﻿using System;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Damage;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Interfaces;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.HealthLogic
{
    public class HealthComponent : MonoBehaviour, IDerivedEntityComponent, IRecycledClass<HealthComponent>
    {
        public event Action HealthEmpty;
        public event Action HealthChanged;

        [SerializeField] private HealthView _currentView;
        [SerializeField] private bool _showFromStart = true;

        private Health _health;
        private bool _isInitialized = false;

        public void Bootstrapp(Entity playerEntity)
        {
            _health = new Health();
        }

        public void Dispose()
        {
            
        }
        
        public HealthComponent Initialize(float health)
        {
            _health.Initialize(health);
            
            _currentView.Initialize(_health.ReactiveHealthRatio);
            
            if(_showFromStart) _currentView.Show();
            else _currentView.Hide();

            return this;
        }
        
        public HealthComponent Enable()
        {
            if (_isInitialized) return this;

            _isInitialized = true;
            
            _health.HealthEmpty += HealthEmpty;
            _health.HealthChanged += HealthChanged;
            
            return this;
        }

        public HealthComponent Disable()
        {
            if (_isInitialized == false) return this;

            _isInitialized = false;
            
            _health.HealthEmpty -= HealthEmpty;
            _health.HealthChanged -= HealthChanged;

            return this;
        }
        
        public void Operate(DamageArgs damage)
        {
            float operateValue = damage.Damage * (damage.Type == DamageType.heal ? 1f : -1f);

            _health.ApplyHealth(operateValue);

            if (_health.CurrentHealth <= 0)
            {
                _currentView
                    .SetActiveStatus(false)
                    .Hide();
                
                return;
            }
            
            _currentView.SetActiveStatus(true).Show();
            
            HealthChanged?.Invoke();
        }
    }
}