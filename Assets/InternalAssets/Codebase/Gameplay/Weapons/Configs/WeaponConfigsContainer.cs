using System;
using System.Collections.Generic;
using System.Linq;
using Codebase.Library.Extension.ScriptableObject;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Library.ProjectAssets;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Weapons.Configs
{
    [CreateAssetMenu(fileName = nameof(WeaponConfigsContainer), menuName = "App/Configs/Weapon/" + nameof(WeaponConfigsContainer))]
    public class WeaponConfigsContainer : LoadableScriptableObject<WeaponConfigsContainer>
    {
        [SerializeField, ReadOnly] private List<WeaponConfig> _weaponConfigs = new();

        public WeaponConfig GetConfig(WeaponType weaponType)
        {
            WeaponConfig config = _weaponConfigs.FirstOrDefault(wc => wc.WeaponType == weaponType);

            if (config == default)
                throw new ArgumentException($"Can not get config with type:[{weaponType}]");

            return config;
        }

#if UNITY_EDITOR
        [Button]
        private void GetAllConfigs()
        {
            _weaponConfigs.Clear();

            if (AssetsCollector.TryGetAssets(out List<WeaponConfig> list))
                _weaponConfigs = list;
        }
#endif
    }
}
