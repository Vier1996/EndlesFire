using System;
using ACS.Core.ServicesContainer;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Configs.Enemy;
using InternalAssets.Codebase.Gameplay.Damage;
using InternalAssets.Codebase.Gameplay.Entities.PlayerFolder;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.HealthLogic;
using InternalAssets.Codebase.Gameplay.Weapons.Presenter;
using InternalAssets.Codebase.Interfaces;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Entities.EnemiesFolder
{
    public class WeaponedEnemy : PursuitEnemy
    {
        [SerializeField] private SpearedEnemyComponents _components;
        
        public override Entity Bootstrapp()
        {
            base.Bootstrapp().BindComponents(_components);
            
            return this;
        }
        
        public override Enemy Initialize(EnemyType enemyType)
        {
            EnemyConfig = EnemyConfig as SimpleEnemyConfig;
            
            base.Initialize(enemyType);
            
            ServiceContainer.ForCurrentScene().Get(out Player target);
            
            EnemyWeaponPresenter weaponPresenter = (EnemyWeaponPresenter)GetAbstractComponent<IWeaponPresenter>();
            IDetectionSystem detectionSystem = GetAbstractComponent<IDetectionSystem>();

            weaponPresenter.Enable().PresentWeapon(WeaponType.melee_spear);
            weaponPresenter.SetPresentersTarget(target);
            
            detectionSystem
                .Enable()
                .SetDetectionRadius(1f);
            
            StartPursuit(target);
            
            return this;
        }
        
        public override void ReceiveDamage(DamageArgs damageArgs)
        {
            if (TryGetAbstractComponent(out HealthComponent healthComponent) == false) return;
            
            healthComponent.Operate(damageArgs);
        }
        
        protected override void OnKilled()
        {
            base.OnKilled();
            
            GetAbstractComponent<IWeaponPresenter>().Disable();
            GetAbstractComponent<IDetectionSystem>().Disable();
            
            StopPursuit();
            
            LeanPool.Despawn(GameObject);
        }

#if UNITY_EDITOR
        [Button]
        private void DebugHit(int damage)
        {
            if (TryGetAbstractComponent(out HealthComponent healthComponent) == false) return;
            
            DamageArgs args = new DamageArgs()
            {
                Damage = damage,
                IsCritical = false,
                Type = DamageType.damage
            };
            
            healthComponent.Operate(args);
            
        }

        [Button]
        private void DebugKill(int damage)
        {
            OnKilled();
        }
#endif
    }
    
    [Serializable]
    public class SpearedEnemyComponents : PursuitEnemyComponents
    {
        [BoxGroup("Speared enemy components"), SerializeField] private EnemyWeaponPresenter _weaponPresenter;
        [BoxGroup("Speared enemy components"), SerializeField] private EnemyDetectionSystem _detectionSystem;

        public override EntityComponents Declare(Entity abstractEntity)
        {
            base.Declare(abstractEntity);

            Add(typeof(IDetectionSystem), _detectionSystem);
            Add(typeof(IWeaponPresenter), _weaponPresenter);
            
            return this;
        }
    }
}