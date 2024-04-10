using System;
using Codebase.Library.SAD;
using Cysharp.Threading.Tasks;
using InternalAssets.Codebase.Gameplay.Behavior.Enemy.States;
using InternalAssets.Codebase.Gameplay.Configs.Enemy;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.Weapons.Presenter;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Entities.EnemiesFolder
{
    public class SpearedEnemy : PursuitEnemy
    {
        [SerializeField] private SpearedEnemyComponents _components;
        
        public override Entity Bootstrapp()
        {
            base.Bootstrapp().BindComponents(_components);
            
            return this;
        }
        
        [Button]
        public override async UniTask<Enemy> Initialize(EnemyType enemyType)
        {
            EnemyConfig = EnemyConfig as SimpleEnemyConfig;
            
            await base.Initialize(enemyType);
            
            //EnableSpearedLogic();
            
            return this;
        }

        [Button]
        private void EnableSpearedLogic()
        {
            StartPursuit(AttackPlayer);
        }

        private void AttackPlayer()
        {
            BehaviorMachine.Machine.SwitchBehavior<EnemySpearedAttackBehavior>();
        }
        
        private void DisableSpearedLogic()
        {
            
        }
    }
    
    [Serializable]
    public class SpearedEnemyComponents : PursuitEnemyComponents
    {
        [BoxGroup("Speared enemy components"), SerializeField] private EnemyWeaponPresenter _weaponPresenter;

        public override EntityComponents Declare(Entity abstractEntity)
        {
            base.Declare(abstractEntity);

            Add(_weaponPresenter);
            
            return this;
        }
    }
}