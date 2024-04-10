using System;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Entities.EnemiesFolder;
using InternalAssets.Codebase.Library.Behavior;

namespace InternalAssets.Codebase.Gameplay.Behavior.Enemy.States
{
    public class EnemySpearedAttackBehavior : EnemyBehaviorState
    {
        public EnemySpearedAttackBehavior(EnemySpearedAttackBehavior other) => IsDefaultBehavior = other.IsDefaultBehavior;

        public override void Construct(EntityComponents components)
        {
            EntityComponents = components as EnemyComponents;

            if (EntityComponents == null)
                throw new ArgumentException("Components are null or can not convert to [EnemyComponents]");
        }
        
        public override void Enter(BehaviorComponents behaviorComponents = null)
        {
        }

        public override void Exit()
        {
        }
    }
}