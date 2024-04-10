using System;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Configs.Enemy;
using InternalAssets.Codebase.Library.Behavior;

namespace InternalAssets.Codebase.Gameplay.Behavior.Enemy
{
    public class EnemyBehaviorMachine : IDerivedEntityComponent
    {
        public BehaviorMachine Machine { get; private set; }

        private Entity _entity;
        
        public void Bootstrapp(Entity entity)
        {
            Machine = new BehaviorMachine();
            _entity = entity;
        }

        public void Initialize(EnemyConfig config)
        {
            Machine.ClearMachine();
            
            foreach (IEnemyBehavior enemyBehavior in config.Behavior)
            {
                Type targetBehaviorType = enemyBehavior.GetType();

                if (Activator.CreateInstance(targetBehaviorType, args: enemyBehavior) is not IBehavior behavior)
                    throw new ArgumentException("Разраб где-то обосрался...");

                Machine.AppendBehavior(targetBehaviorType, behavior, _entity.Components);
            }
            
            Machine.SwitchToDefaultBehavior();
        }

        public void Dispose()
        {
        }
    }
}