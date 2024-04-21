using InternalAssets.Codebase.Gameplay.InventoryFolder;
using InternalAssets.Codebase.Gameplay.Items;
using InternalAssets.Codebase.Interfaces;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Collecting
{
    public class PlayerCollectingComponent : MonoBehaviour, ICollector
    {
        private Inventory _inventory;

        private void Start() => _inventory = new();

        public Transform GetCollectorAnchor() => transform;

        public void Collect(ICollectable collectable)
        {
            ItemData data = collectable.GetCollectableData();
            
            _inventory.AddToInventory(data);
        }
    }
}
