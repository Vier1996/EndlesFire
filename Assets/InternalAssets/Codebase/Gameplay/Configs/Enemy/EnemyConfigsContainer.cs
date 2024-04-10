using System;
using System.Collections.Generic;
using System.Linq;
using Codebase.Library.Extension.ScriptableObject;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Library.ProjectAssets;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Configs.Enemy
{
    [CreateAssetMenu(fileName = nameof(EnemyConfigsContainer), menuName = "App/Configs/Enemy/" + nameof(EnemyConfigsContainer))]
    public class EnemyConfigsContainer : LoadableScriptableObject<EnemyConfigsContainer>
    {
        [SerializeField, ReadOnly] private List<EnemyConfig> _configs = new();

        public EnemyConfig Get(EnemyType enemyType)
        {
            EnemyConfig config = _configs.FirstOrDefault(cfg => cfg.Type == enemyType);

            if (config == default)
                throw new ArgumentException($"Can not get config with type:[{enemyType}]");

            return config;
        }

        [Button]
        private void GetConfigs()
        {
            if (AssetsCollector.TryGetAssets(out List<EnemyConfig> configs))
            {
                _configs.Clear();
                _configs = configs;
            }
        }
    }
}