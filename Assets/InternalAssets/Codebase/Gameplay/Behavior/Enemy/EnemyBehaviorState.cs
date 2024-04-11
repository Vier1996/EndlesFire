using Codebase.Library.SAD;
using InternalAssets.Codebase.Gameplay.Entities.EnemiesFolder;
using InternalAssets.Codebase.Gameplay.Entities.PlayerFolder;
using InternalAssets.Codebase.Library.Behavior;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Behavior.Enemy
{
    public abstract class EnemyBehaviorState : IEnemyBehavior
    {
        [field: SerializeField] public bool IsDefaultBehavior { get; set; }
        
        protected EnemyComponents EntityComponents;

        public abstract void Construct(EntityComponents components);

        public abstract void Enter(BehaviorComponents behaviorComponents = null);

        public abstract void Exit();

        public virtual void Dispose() { }
    }
}