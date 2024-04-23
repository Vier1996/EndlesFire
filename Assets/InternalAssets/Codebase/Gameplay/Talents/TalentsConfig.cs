using System;
using System.Collections.Generic;
using System.Linq;
using Codebase.Library.Extension.ScriptableObject;
using InternalAssets.Codebase.Gameplay.Enums;
using InternalAssets.Codebase.Library.ProjectAssets;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Talents
{
    [CreateAssetMenu(fileName = nameof(TalentsConfig), menuName = "App/Configs/Talents/" + nameof(TalentsConfig))]
    public class TalentsConfig : LoadableScriptableObject<TalentsConfig>
    {
        [SerializeField] private List<TalentSetup> _talentSetups = new();

        public TalentSetup Get(TalentType talentType)
        {
            TalentSetup setup = _talentSetups.FirstOrDefault(tt => tt.TalentType == talentType);

            if (setup == default)
                throw new ArgumentException($"Can not get setup with type:[{talentType}]");
            
            return setup;
        }

        public List<TalentType> GetAllTypes() => _talentSetups.Select(ts => ts.TalentType).ToList();

#if UNITY_EDITOR
        [Button]
        private void GetAllConfigs()
        {
            _talentSetups.Clear();

            if (AssetsCollector.TryGetAssets(out List<TalentSetup> list))
                _talentSetups = list;
        }
#endif
    }
}