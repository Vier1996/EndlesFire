using System;
using ACS.Core.ServicesContainer;
using ACS.SignalBus.SignalBus;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.InventoryFolder;
using InternalAssets.Codebase.Gameplay.Items;
using InternalAssets.Codebase.Interfaces;
using InternalAssets.Codebase.Signals;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Collecting
{
    public class PlayerCollectingComponent : MonoBehaviour, ICollector
    {
        private ISignalBusService _signalBusService;
        private Inventory _inventory;

        private void Start()
        {
            ServiceContainer.Core.Get(out _signalBusService);
            
            _inventory = new();
        }

        public Transform GetCollectorAnchor() => transform;

        public void Collect(ICollectable collectable)
        {
            switch (collectable.GetCollectableData())
            {
                case ItemData itemData: ExecuteItemData(itemData); break;
                case WeaponData weaponData: ExecuteWeaponData(weaponData); break;
            }
        }

        private void ExecuteItemData(ItemData itemData)
        {
            ItemType type = itemData.ItemType;
            long count = itemData.ItemsCount;
            
            if (itemData.CanBePlacedToInventory) 
                _inventory.AddToInventory(type, count);
            
            if (itemData.CanBeSignaled) 
                _signalBusService.Fire(new InventoryItemRegistred(type, count));
        }

        private void ExecuteWeaponData(WeaponData weaponData)
        {
            if (weaponData.CanBeSignaled)
                _signalBusService.Fire(new WeaponItemRegistred(weaponData.WeaponType));
        }
    }
}
