using System;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Entities.EnemiesFolder;
using InternalAssets.Codebase.Gameplay.Entities.PlayerFolder;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Library.Behavior;
using InternalAssets.Codebase.Services.Animation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InternalAssets.Codebase.Gameplay.Behavior.Enemy.States
{
    public class EnemyPursuitBehavior : EnemyBehaviorState
    {
        [SerializeField] private float _minSpeed;
        [SerializeField] private float _maxSpeed;

        private EnemyPursuitBehaviorComponents _behaviorComponents;
        private EnemyTranslationComponent _enemyTranslationComponent;
        private SpriteSheetAnimator _spriteSheetAnimator;

        public EnemyPursuitBehavior(EnemyPursuitBehavior other)
        {
            IsDefaultBehavior = other.IsDefaultBehavior;
            
            _minSpeed = other._minSpeed;
            _maxSpeed = other._maxSpeed;
        }

        public override void Construct(EntityComponents components)
        {
            EntityComponents = components as EnemyComponents;

            if (EntityComponents == null)
                throw new ArgumentException("Components are null or can not convert to [EnemyComponents]");
            
            EntityComponents.TryGetAbstractComponent(out _enemyTranslationComponent);
            EntityComponents.TryGetAbstractComponent(out _spriteSheetAnimator);
        }
        
        public override void Enter(BehaviorComponents behaviorComponents = null)
        {
            if (behaviorComponents != null)
            {
                _behaviorComponents = behaviorComponents as EnemyPursuitBehaviorComponents;

                if (_behaviorComponents == null)
                    throw new ArgumentException("Передайте разрабу что он даун");
            }
            
            if(EntityComponents.TryGetAbstractComponent(out Entity entity) == false) return;

            Entities.EnemiesFolder.Enemy enemy = entity as Entities.EnemiesFolder.Enemy;
            
            if(enemy == null || enemy.CurrentTarget == null)
                return;
            
            _spriteSheetAnimator.SetAnimation(CommonAnimationType.walk);
            
            _enemyTranslationComponent
                .WithParams(Random.Range(_minSpeed, _maxSpeed))
                .TransferTo(enemy.CurrentTarget, _behaviorComponents.PursuitCompletedCallback);
        }

        public override void Exit()
        {
            _enemyTranslationComponent.CancelTranslate();
        }
    }
    
    public class EnemyPursuitBehaviorComponents : BehaviorComponents
    {
        public Action PursuitCompletedCallback;
    }
}