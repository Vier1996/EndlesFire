using System;
using System.Collections.Generic;
using Codebase.Library.Extension.ScriptableObject;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Configs
{
    [CreateAssetMenu(fileName = nameof(PlayerProgressionConfig), menuName = "App/Configs/Progression/" + nameof(PlayerProgressionConfig))]
    public class PlayerProgressionConfig : LoadableScriptableObject<PlayerProgressionConfig>
    {
        [SerializeField] private List<PlayerProgressionSetup> _setup = new();

        public PlayerProgressionSetup Get(int level)
        {
            if (level <= 0 || level > _setup.Count)
                throw new ArgumentException("Level is out of Range");
            
            return _setup[level - 1];
        }
    }

    [Serializable]
    public class PlayerProgressionSetup
    {
        [field: SerializeField] public long NeededExperience { get; private set; } = 0;
    }
}