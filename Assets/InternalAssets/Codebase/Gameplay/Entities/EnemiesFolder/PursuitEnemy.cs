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
        protected EnemyTranslationComponent EnemyTranslationComponent;
        protected ModelViewProvider ModelViewProvider;
        
        protected void StartPursuit(ITargetable targetable, Action onReceiveTarget = null)
        {
            ModelViewProvider.ModelView.SpriteSheetAnimator.SetAnimation(CommonAnimationType.walk);
            
            EnemyTranslationComponent
                .WithParams(1f, 0.6f)
                .TransferTo(targetable, onReceiveTarget);
        }

        protected void StopPursuit()
        {
            ModelViewProvider.ModelView.SpriteSheetAnimator.Deactivate();
            
            EnemyTranslationComponent.Dispose();
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