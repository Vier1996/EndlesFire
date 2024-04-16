using UnityEngine;

namespace InternalAssets.Codebase.Interfaces
{
    public interface ICollector
    {
        public Transform GetCollectorAnchor();
        public void Collect(ICollectable collectable);
    }
}