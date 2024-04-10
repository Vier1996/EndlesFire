using System;
using Codebase.Library.SAD;
using Cysharp.Threading.Tasks;
using InternalAssets.Codebase.Gameplay.Behavior.Enemy;
using InternalAssets.Codebase.Gameplay.Behavior.Enemy.States;
using InternalAssets.Codebase.Gameplay.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Entities.EnemiesFolder
{
    public abstract class PursuitEnemy : Enemy
    {
        protected EnemyBehaviorMachine BehaviorMachine;

        public override async UniTask<Enemy> Initialize(EnemyType enemyType)
        {
            await base.Initialize(enemyType);

            TryGetAbstractComponent(out BehaviorMachine);

            return this;
        }

        public void StartPursuit(Action onReceiveTarget = null)
        {
            BehaviorMachine.Machine.SwitchBehavior<EnemyPursuitBehavior>(new EnemyPursuitBehaviorComponents()
            {
                PursuitCompletedCallback = onReceiveTarget
            });
        }

        public void StopPursuit()
        {
            BehaviorMachine.Machine.SwitchBehavior<EnemyInitialBehavior>();
        }
    }

    [Serializable]
    public class PursuitEnemyComponents : EnemyComponents
    {
        [BoxGroup("Pursuit enemy components"), SerializeField] private EnemyTranslationComponent _enemyTranslationComponent;
        
        public override EntityComponents Declare(Entity abstractEntity)
        {
            base.Declare(abstractEntity);

            Add(_enemyTranslationComponent);
            
            return this;
        }
    }
}