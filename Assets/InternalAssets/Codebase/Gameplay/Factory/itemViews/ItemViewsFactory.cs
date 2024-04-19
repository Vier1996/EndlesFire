using System.Collections.Generic;
using Codebase.Library.Extension.ScriptableObject;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.Items;
using InternalAssets.Codebase.Library.Design.Factory;
using Sirenix.Serialization;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Factory.itemViews
{
    [CreateAssetMenu(fileName = nameof(ItemViewsFactory), menuName = "App/Factory/" + nameof(ItemViewsFactory))]
    public class ItemViewsFactory : LoadableScriptableObject<ItemViewsFactory>, IFactory<ItemView, ItemType>
    {
        [OdinSerialize] private Dictionary<ItemType, ItemView> _factoryData = new();
        
        public ItemView GetFactoryValue(ItemType factoryType)
        {
            if (_factoryData.ContainsKey(factoryType) == false)
            {
                Debug.LogError($"{nameof(ItemViewsFactory)} not contains value with type:[{factoryType.ToString()}]");
                return null;
            }

            return _factoryData[factoryType];
        }
    }
}