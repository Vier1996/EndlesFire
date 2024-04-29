using System;
using System.Collections.Generic;
using System.Linq;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Library.Extension.Properties;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Characteristics
{
    [Serializable]
    public class CharacteristicsContainer : ICloneable
    {
        [SerializeField] private List<CharacteristicData> _characteristics = new();

        public bool _isOriginal = true;

        public CharacteristicsContainer(List<CharacteristicData> characteristics, bool isCopy)
        {
            _characteristics = characteristics;
            _isOriginal = !isCopy;
        }

        public float GetValue(CharacteristicType characteristicType)
        {
            if (Validate() == false) throw new Exception($"Before working with Container make it copy!");
            
            CharacteristicData data = _characteristics.FirstOrDefault(cd => cd.Type == characteristicType);

            if (data == default)
                throw new ArgumentException($"Container doesnt contains characteristic with type:[{characteristicType}]");

            return data.Value;
        }
        
        public float GetModifiedValue(CharacteristicType characteristicType, float modifyPercent = 0f)
        {
            if (Validate() == false) throw new Exception($"Before working with Container make it copy!");

            CharacteristicData data = _characteristics.FirstOrDefault(cd => cd.Type == characteristicType);

            if (data == default)
                throw new ArgumentException($"Container doesnt contains characteristic with type:[{characteristicType}]");
            
            return data.Value * modifyPercent.Normalize01().AsNormalizedPercent();
        }
        
        public float ModifyValueInternally(CharacteristicType characteristicType, float modifyPercent = 0f)
        {
            if (Validate() == false) throw new Exception($"Before working with Container make it copy!");

            CharacteristicData data = _characteristics.FirstOrDefault(cd => cd.Type == characteristicType);

            if (data == default)
                throw new ArgumentException($"Container doesnt contains characteristic with type:[{characteristicType}]");

            data.ModifyValue(
                modifyPercent
                    .Normalize01()
                    .AsNormalizedPercent()
            );
            
            return data.Value;
        }

        public List<CharacteristicData> GetAll() => _characteristics;

        public object Clone() => new CharacteristicsContainer(_characteristics, isCopy: true);

        private bool Validate() => _isOriginal == false;
    }
}