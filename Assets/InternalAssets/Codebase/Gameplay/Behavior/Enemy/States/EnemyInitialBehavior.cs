using System;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Entities.EnemiesFolder;
using InternalAssets.Codebase.Gameplay.Entities.PlayerFolder;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Library.Behavior;
using InternalAssets.Codebase.Services.Animation;

namespace InternalAssets.Codebase.Gameplay.Behavior.Enemy.States
{
    public class EnemyInitialBehavior : EnemyBehaviorState
    {
        private SpriteSheetAnimator _spriteSheetAnimator;
        
        public EnemyInitialBehavior(EnemyInitialBehavior other) => IsDefaultBehavior = other.IsDefaultBehavior;

        public override void Construct(EntityComponents components)
        {
            EntityComponents = components as EnemyComponents;

            if (EntityComponents == null)
                throw new ArgumentException("Components are null or can not convert to [EnemyComponents]");

            EntityComponents.TryGetAbstractComponent(out _spriteSheetAnimator);
        }
        
        public override void Enter(BehaviorComponents behaviorComponents = null)
        {
            _spriteSheetAnimator.SetAnimation(CommonAnimationType.idle);
        }

        public override void Exit()
        {
        }
    }
}