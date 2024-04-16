using InternalAssets.Codebase.Gameplay.Items;
using InternalAssets.Codebase.Interfaces;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Collecting
{
    public class PlayerCollectingComponent : MonoBehaviour, ICollector
    {
        public Transform GetCollectorAnchor() => transform;

        public void Collect(ICollectable collectable)
        {
            ItemData data = collectable.GetCollectableData();
        }
    }
}
