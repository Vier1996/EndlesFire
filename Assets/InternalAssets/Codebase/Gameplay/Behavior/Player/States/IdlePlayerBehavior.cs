using System;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Entities.PlayerFolder;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.ModelsView;
using InternalAssets.Codebase.Library.Behavior;

namespace InternalAssets.Codebase.Gameplay.Behavior.Player.States
{
    public class IdlePlayerBehavior : PlayerBehaviorState
    {
        private ModelViewProvider _modelViewProvider;
        
        public IdlePlayerBehavior(IdlePlayerBehavior other) => IsDefaultBehavior = other.IsDefaultBehavior;

        public override void Construct(EntityComponents components)
        {
            PlayerComponents = components as PlayerComponents;

            if (PlayerComponents == null)
                throw new ArgumentException("Components are null or can not convert to [EnemyComponents]");

            PlayerComponents.TryGetAbstractComponent(out _modelViewProvider);
        }

        public override void Enter(BehaviorComponents behaviorComponents = null) =>
            _modelViewProvider.ModelView.SpriteSheetAnimator.SetAnimation(CommonAnimationType.idle);

        public override void Exit()
        {
        }
    }
}