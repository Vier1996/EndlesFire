using System;
using InternalAssets.Codebase.Gameplay.Configs;
using InternalAssets.Codebase.Library.Behavior;
using InternalAssets.Codebase.Library.MonoEntity;
using InternalAssets.Codebase.Library.MonoEntity.Entities;
using InternalAssets.Codebase.Library.MonoEntity.Interfaces;

namespace InternalAssets.Codebase.Gameplay.Behavior.Player
{
    public class PlayerBehaviorMachine : IDerivedEntityComponent
    {
        public BehaviorMachine Machine { get; private set; }

        public void Bootstrapp(Entity entity)
        {
            Machine = new BehaviorMachine();
            
            PlayerConfig playerConfig = PlayerConfig.GetInstance();
            
            foreach (IPlayerBehavior enemyBehavior in playerConfig.PlayerBehavior)
            {
                Type targetBehaviorType = enemyBehavior.GetType();

                if (Activator.CreateInstance(targetBehaviorType, args: enemyBehavior) is not IBehavior behavior)
                    throw new ArgumentException("Разраб где-то обосрался...");

                Machine.AppendBehavior(targetBehaviorType, behavior, entity.Components);
            }
            
            Machine.SwitchToDefaultBehavior();
        }

        public void Dispose()
        {
        }
    }
}