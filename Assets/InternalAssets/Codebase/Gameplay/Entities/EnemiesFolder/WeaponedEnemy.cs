using System;
using ACS.Core.ServicesContainer;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Configs.Enemy;
using InternalAssets.Codebase.Gameplay.Entities.PlayerFolder;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.Weapons.Presenter;
using InternalAssets.Codebase.Interfaces;
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
        
        [Button]
        public override Enemy Initialize(EnemyType enemyType)
        {
            EnemyConfig = EnemyConfig as SimpleEnemyConfig;
            
            base.Initialize(enemyType);
            
            ServiceContainer.ForCurrentScene().Get(out Player target);
            
            EnemyWeaponPresenter weaponPresenter = (EnemyWeaponPresenter)GetAbstractComponent<IWeaponPresenter>();

            weaponPresenter.Enable();
            weaponPresenter.SetPresentersTarget(target);
            
            GetAbstractComponent<IDetectionSystem>()
                .Enable()
                .SetDetectionRadius(1f);

            StartPursuit(target);
            
            return this;
        }
        
        
        private void DisableSpearedLogic() 
        {
            GetAbstractComponent<IWeaponPresenter>().Disable();
            GetAbstractComponent<EnemyDetectionSystem>().Disable();
        }
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