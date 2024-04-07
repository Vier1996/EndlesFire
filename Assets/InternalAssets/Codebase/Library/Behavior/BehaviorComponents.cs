using System;
using Codebase.Library.SAD;

namespace InternalAssets.Codebase.Library.Behavior
{
    public abstract class BehaviorComponents { }
    
    public interface IBehavior : IDisposable
    {
        public bool IsDefaultBehavior { get; set; }

        public void Construct(EntityComponents components);

        public void Enter(BehaviorComponents behaviorComponents = null);
        
        public void Exit();
    }
}
