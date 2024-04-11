using System;

namespace InternalAssets.Codebase.Interfaces
{
    public interface IDetectionSystem
    {
        public event Action<ITargetable> OnTargetDetected;
    }
}