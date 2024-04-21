using System;
using System.Collections.Generic;
using System.Linq;
using Codebase.Library.Extension.ScriptableObject;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Talents
{
    [CreateAssetMenu(fileName = nameof(TalentsConfig), menuName = "App/Configs/Talents/" + nameof(TalentsConfig))]
    public class TalentsConfig : LoadableScriptableObject<TalentsConfig>
    {
        [SerializeField] private List<ITalentSetup> _talentSetups = new();

        public T Get<T>() where T : class, ITalentSetup
        {
            ITalentSetup setup = _talentSetups.FirstOrDefault(tt => tt.GetType() == typeof(T));

            if (setup == default)
                throw new ArgumentException($"Can not get setup with type:[{typeof(T).Name}]");
            
            return setup as T;
        }
    }
}