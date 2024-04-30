using System;
using ACS.Core.ServicesContainer;
using InternalAssets.Codebase.Gameplay.Configs.Enemy;
using InternalAssets.Codebase.Gameplay.Damage;
using InternalAssets.Codebase.Gameplay.Entities.PlayerFolder;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.Factory.Vfx;
using InternalAssets.Codebase.Gameplay.Sorting;
using InternalAssets.Codebase.Interfaces;
using InternalAssets.Codebase.Library.MonoEntity.Entities;
using InternalAssets.Codebase.Library.MonoEntity.EntityComponent;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Entities.EnemiesFolder
{
    public class DetonatedEnemy : PursuitEnemy
    {
        [SerializeField] private DetonatedEnemyComponents _components;
        
        private IDetectionSystem _detectionSystem;
        private VfxFactoryProvider _vfxFactoryProvider;

        public override Entity Bootstrapp(EntityComponents components = null)
        {
            if (IsBootstrapped) return this;
            
            base.Bootstrapp(_components);

            ServiceContainer.Global.Get(out _vfxFactoryProvider);
            
            Components.TryGetAbstractComponent(out _detectionSystem);
            Components.TryGetAbstractComponent(out EnemyTranslationComponent);
            Components.TryGetAbstractComponent(out ModelViewProvider);
            Components.TryGetAbstractComponent(out HealthComponent);
            
            return this;
        }
        
        public override Enemy Initialize(EnemyType enemyType)
        {
            EnemyConfig = EnemyConfig as SimpleEnemyConfig;
            
            base.Initialize(enemyType);
            
            ServiceContainer.ForCurrentScene().Get(out Player target);
            
            ModelViewProvider.ModelView.SpriteSheetAnimator.Activate();
            
            _detectionSystem.Enable().SetDetectionRadius(1f);
            
            StartPursuit(target);
            
            return this;
        }
        
        public override void ReceiveDamage(DamageArgs damageArgs)
        {
            HealthComponent.Operate(damageArgs);
        }
        
        protected override void OnKilled()
        {
            base.OnKilled();
            
            _detectionSystem.Disable();
            
            StopPursuit();
            SpawnDeathEffect();
            
            LeanPool.Despawn(GameObject);
        }

        private async void SpawnDeathEffect()
        {
            SortableParticle deathParticle = await _vfxFactoryProvider
                .SpawnFactoryItemAsync(
                    VfxType.enemy_death_base_variation,
                    Transform.position);
            
            deathParticle.Play();
        }
    }
    
    [Serializable]
    public class DetonatedEnemyComponents : PursuitEnemyComponents
    {
        [BoxGroup("Speared enemy components"), SerializeField] private EnemyDetectionSystem _detectionSystem;

        public override EntityComponents Declare(Entity abstractEntity)
        {
            base.Declare(abstractEntity);

            Add(typeof(IDetectionSystem), _detectionSystem);
            
            return this;
        }
    }
}
