using System;
using System.Collections.Generic;
using System.Linq;
using InternalAssets.Codebase.Library.MonoEntity;
using InternalAssets.Codebase.Library.MonoEntity.EntityComponent;

namespace InternalAssets.Codebase.Library.Behavior
{
    public class BehaviorMachine
    {
        private readonly Dictionary<Type, IBehavior> _behaviorStates = new();
        private IBehavior _currentBehaviorState;

        public BehaviorMachine AppendBehavior(Type behaviorType, IBehavior behavior, EntityComponents components = null)
        {
            if(_behaviorStates.TryAdd(behaviorType, behavior))
                _behaviorStates.Last().Value.Construct(components);
            
            return this;
        }

        public void ClearMachine()
        {
            _currentBehaviorState = null;
            _behaviorStates.Clear();
        }

        public void SwitchToDefaultBehavior()
        {
            IBehavior defaultBehavior = _behaviorStates.First(bh => bh.Value.IsDefaultBehavior).Value;
           
            ChangeBehavior(defaultBehavior);
        }

        public void SwitchBehavior<TBehavior>(BehaviorComponents behaviorComponents = null, bool force = false) where TBehavior : IBehavior
        {
            IBehavior nextBehavior = _behaviorStates[typeof(TBehavior)];
            
            if(_currentBehaviorState == nextBehavior && force == false)
                return;

            ChangeBehavior(nextBehavior, behaviorComponents);
        }

        private void ChangeBehavior(IBehavior nextBehavior, BehaviorComponents behaviorComponents = null)
        {
            _currentBehaviorState?.Exit();
            _currentBehaviorState = nextBehavior;
            _currentBehaviorState.Enter(behaviorComponents);
        }

        public void Dispose() => _currentBehaviorState?.Dispose();
    }
}