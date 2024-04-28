using System;
using System.Collections.Generic;
using System.Linq;
using InternalAssets.Codebase.Gameplay.Enums;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Characteristics
{
    [Serializable]
    public class CharacteristicsContainer : ICloneable
    {
        [SerializeField] private List<CharacteristicData> _characteristics = new();

        public CharacteristicsContainer(List<CharacteristicData> characteristics) => 
            _characteristics = characteristics;

        public float GetValue(CharacteristicType characteristicType)
        {
            CharacteristicData data = _characteristics.FirstOrDefault(cd => cd.Type == characteristicType);

            if (data == default)
                throw new ArgumentException($"Container doesnt contains characteristic with type:[{characteristicType}]");

            return data.Value;
        }

        public List<CharacteristicData> GetAll() => _characteristics;

        public object Clone() => new CharacteristicsContainer(_characteristics);
    }

    [Serializable]
    public class CharacteristicData
    {
        [field: SerializeField] public CharacteristicType Type { get; private set; } = CharacteristicType.none;
        [field: SerializeField] public float Value { get; private set; } = 0f;
    }
}