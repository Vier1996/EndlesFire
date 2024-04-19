using System.Collections.Generic;
using Codebase.Library.Extension.ScriptableObject;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Gameplay.Factory.itemViews;
using InternalAssets.Codebase.Gameplay.Sorting;
using InternalAssets.Codebase.Library.Design.Factory;
using Sirenix.Serialization;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Factory.Vfx
{
    [CreateAssetMenu(fileName = nameof(VfxFactory), menuName = "App/Factory/" + nameof(VfxFactory))]
    public class VfxFactory : LoadableScriptableObject<VfxFactory>, IFactory<SortableParticle, VfxType>
    {
        [OdinSerialize] private Dictionary<VfxType, SortableParticle> _factoryData = new();
        
        public SortableParticle GetFactoryValue(VfxType factoryType)
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