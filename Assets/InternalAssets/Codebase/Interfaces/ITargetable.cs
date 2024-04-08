using UnityEngine;

namespace InternalAssets.Codebase.Interfaces
{
    public interface ITargetable
    {
        public Transform GetTargetTransform();

        public void EnableMarker();
        public void DisableMarker();
    }
}