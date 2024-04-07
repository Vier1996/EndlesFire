using System;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Entities.PlayerFolder;
using InternalAssets.Codebase.Library.Behavior;

namespace InternalAssets.Codebase.Gameplay.Behavior.Player.States
{
    public class PlayerDodgeBehaviour : PlayerBehaviorState
    {
        private PlayerAnimator _playerAnimator;
        
        public PlayerDodgeBehaviour(PlayerDodgeBehaviour other) => IsDefaultBehavior = other.IsDefaultBehavior;

        public override void Construct(EntityComponents components)
        {
            PlayerComponents = components as PlayerComponents;

            if (PlayerComponents == null)
                throw new ArgumentException("Components are null or can not convert to [EnemyComponents]");

            PlayerComponents.TryGetAbstractComponent(out _playerAnimator);
        }
        
        public override void Enter(BehaviorComponents behaviorComponents = null) => 
            _playerAnimator.ToDodge();

        public override void Exit()
        {
        }
    }
}