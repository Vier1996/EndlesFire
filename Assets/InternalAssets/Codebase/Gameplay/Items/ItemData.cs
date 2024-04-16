using System;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.Values;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Items
{
    [Serializable]
    public class ItemData
    {
        [field: SerializeField] public ItemType ItemType { get; private set; } = ItemType.none;
        [field: SerializeField] public ValueConfig Value { get; private set; } = new();
        
        public long ItemsCount { get; private set; } = 1;
    }
}