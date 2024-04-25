using System;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Interfaces;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Items
{
    [Serializable]
    public class WeaponData : IItemData
    {
        [field: SerializeField] public WeaponType WeaponType { get; private set; } = WeaponType.none;
        [field: SerializeField] public bool CanBePlacedToInventory { get; private set; } = true;
        [field: SerializeField] public bool CanBeSignaled { get; private set; } = false;    }
}