using System;
using Codebase.Gameplay.Sorting;
using InternalAssets.Codebase.Gameplay.Configs.Enemy;
using InternalAssets.Codebase.Gameplay.CustomComponents;
using InternalAssets.Codebase.Gameplay.Damage;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.HealthLogic;
using InternalAssets.Codebase.Gameplay.ModelsView;
using InternalAssets.Codebase.Gameplay.Targets;
using InternalAssets.Codebase.Interfaces;
using InternalAssets.Codebase.Library.MonoEntity;
using InternalAssets.Codebase.Library.MonoEntity.Entities;
using InternalAssets.Codebase.Library.MonoEntity.EntityComponent;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Entities.EnemiesFolder
{
    public abstract class Enemy : Entity, IEnemy, ITargetable, IDamageReceiver
    {
        protected EnemyConfig EnemyConfig;
        protected HealthComponent HealthComponent;
        
        private EnemyComponents _enemyComponents;
        private TargetView _targetView;

        public virtual Enemy Initialize(EnemyType enemyType)
        {
            Components.TryGetAbstractComponent(out _targetView);
            
            EnemyConfigsContainer configsContainer = EnemyConfigsContainer.GetInstance();

            EnemyConfig = configsContainer.Get(enemyType);
            
            HealthComponent.Initialize(20).Enable();
            HealthComponent.HealthEmpty += OnKilled;
            
            configsContainer.Release();
            
            return this;
        }

        public virtual Transform GetTargetTransform()
        {
            _enemyComponents ??= Components as EnemyComponents;

            if(_enemyComponents == null)
                throw new ArgumentException($"Can not cast AbstractComponents to {nameof(EnemyComponents)}");
            
            return _enemyComponents.TargetTransform;
        }

        public virtual void EnableMarker() => _targetView.EnableView();
        
        public virtual void DisableMarker() => _targetView.DisableView();
        
        public virtual void ReceiveDamage(DamageArgs damageArgs) { }
        
        protected virtual void OnKilled()
        {
            HealthComponent.HealthEmpty -= OnKilled;

            DisableMarker();
            
            HealthComponent.Disable();
        }
        
#if UNITY_EDITOR
        [Button]
        private void DebugHit(int damage)
        {
            DamageArgs args = new DamageArgs()
            {
                Damage = damage,
                IsCritical = false,
                Type = DamageType.damage
            };
            
            HealthComponent.Operate(args);
            
        }

        [Button]
        private void DebugKill(int damage)
        {
            OnKilled();
        }
#endif
    }
    
    [Serializable]
    public class EnemyComponents : EntityComponents
    {
        [field: SerializeField, BoxGroup("Enemy components/Target components")] public Transform TargetTransform { get; private set; }

        [BoxGroup("Enemy components"), SerializeField] private PhysicPairComponent _physicPairComponent;
        [BoxGroup("Enemy components"), SerializeField] private ModelViewProvider _modelViewProvider;
        [BoxGroup("Enemy components"), SerializeField] private TargetView _targetView;
        [BoxGroup("Enemy components"), SerializeField] private HealthComponent _healthComponent;

        public override EntityComponents Declare(Entity abstractEntity)
        {
            Add(typeof(Entity), abstractEntity);
            Add(_physicPairComponent);
            Add(_targetView);
            Add(_healthComponent);
            Add(_modelViewProvider);
            
            return this;
        }
    }
}