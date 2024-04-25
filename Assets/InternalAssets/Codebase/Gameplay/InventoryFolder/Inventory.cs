using System;
using System.Collections.Generic;
using ACS.Core.ServicesContainer;
using ACS.SignalBus.SignalBus;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.Items;
using InternalAssets.Codebase.Interfaces;
using InternalAssets.Codebase.Signals;

namespace InternalAssets.Codebase.Gameplay.InventoryFolder
{
    public class Inventory : IDisposable
    {
        private readonly Dictionary<ItemType, long> _inventoryItems = new();

        public Inventory() { }
        
        public void Dispose()
        {
            _inventoryItems.Clear();
        }
        
        public void AddToInventory(ItemType itemType, long count)
        {
            _inventoryItems[itemType] += count;
        }
        
        public void RemoveFromToInventory(ItemType itemType)
        {
            if (_inventoryItems.ContainsKey(itemType))
                _inventoryItems.Remove(itemType);
        }

        public long GetCount(ItemType itemType)
        {
            if (_inventoryItems.TryGetValue(itemType, out long value))
                return value;

            return 0;
        }

        public Dictionary<ItemType, long> GetAll() => _inventoryItems;
    }
}