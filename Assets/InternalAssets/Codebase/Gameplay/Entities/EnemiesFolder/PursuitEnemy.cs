using System;
using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.ModelsView;
using InternalAssets.Codebase.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Entities.EnemiesFolder
{
    public abstract class PursuitEnemy : Enemy
    {
        private EnemyTranslationComponent _enemyTranslationComponent;
        private ModelViewProvider _modelViewProvider;
        
        public override Enemy Initialize(EnemyType enemyType)
        {
            base.Initialize(enemyType);

            TryGetAbstractComponent(out _enemyTranslationComponent);
            TryGetAbstractComponent(out _modelViewProvider);

            return this;
        }

        public void StartPursuit(ITargetable targetable, Action onReceiveTarget = null)
        {
            _modelViewProvider.ModelView.SpriteSheetAnimator.SetAnimation(CommonAnimationType.walk);
            
            _enemyTranslationComponent
                .WithParams(1f, 0.9f)
                .TransferTo(targetable, onReceiveTarget);
        }

        public void StopPursuit()
        {
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