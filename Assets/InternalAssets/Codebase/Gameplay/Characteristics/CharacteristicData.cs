using System;
using InternalAssets.Codebase.Gameplay.Enums;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Characteristics
{
    [Serializable]
    public class CharacteristicData
    {
        [field: SerializeField] public CharacteristicType Type { get; private set; } = CharacteristicType.none;
        [field: SerializeField] public float Value { get; private set; } = 0f;

        public CharacteristicData ModifyValue(float modificator)
        {
            Value *= modificator;
            return this;
        }
    }
}