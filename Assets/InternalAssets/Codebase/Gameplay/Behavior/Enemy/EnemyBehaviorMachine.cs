using Codebase.Library.SAD;
using InternalAssets.Codebase.Library.Behavior;

namespace InternalAssets.Codebase.Gameplay.Behavior.Enemy
{
    public class EnemyBehaviorMachine : IDerivedEntityComponent
    {
        public BehaviorMachine Machine { get; private set; }

        public void Bootstrapp(Entity entity)
        {
            Machine = new BehaviorMachine();
            
            /*PlayerConfig playerConfig = PlayerConfig.GetInstance();
            
            foreach (IPlayerBehavior enemyBehavior in playerConfig.PlayerBehavior)
            {
                Type targetBehaviorType = enemyBehavior.GetType();

                if (Activator.CreateInstance(targetBehaviorType, args: enemyBehavior) is not IBehavior behavior)
                    throw new ArgumentException("Разраб где-то обосрался...");

                Machine
                    .AppendBehavior(targetBehaviorType, behavior, entity.Components)
                    .SwitchToDefaultBehavior();
            }*/
        }

        public void Dispose()
        {
        }
    }
}