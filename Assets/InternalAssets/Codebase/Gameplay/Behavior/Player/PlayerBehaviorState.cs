using InternalAssets.Codebase.Gameplay.Entities.PlayerFolder;
using InternalAssets.Codebase.Library.Behavior;
using InternalAssets.Codebase.Library.MonoEntity;
using InternalAssets.Codebase.Library.MonoEntity.EntityComponent;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Behavior.Player
{
    public abstract class PlayerBehaviorState : IPlayerBehavior
    {
        [field: SerializeField] public bool IsDefaultBehavior { get; set; }
        
        protected PlayerComponents PlayerComponents;

        public abstract void Construct(EntityComponents components);

        public abstract void Enter(BehaviorComponents behaviorComponents = null);

        public abstract void Exit();

        public virtual void Dispose() { }
    }
}