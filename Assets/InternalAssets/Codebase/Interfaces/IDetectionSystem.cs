using System;

namespace InternalAssets.Codebase.Interfaces
{
    public interface IDetectionSystem : IRecycledClass<IDetectionSystem>
    {
        public event Action<ITargetable> OnTargetDetected;

        public ITargetable GetCurrentTarget();
        public void SetDetectionRadius(float radius);
    }
}