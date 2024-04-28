using System;
using System.Collections.Generic;
using System.Linq;
using Codebase.Library.Extension.ScriptableObject;
using InternalAssets.Codebase.Services.Scenes.Enum;
using UnityEngine;

namespace InternalAssets.Codebase.Services.Scenes.Data
{
    [CreateAssetMenu(fileName = nameof(SceneConfig), menuName = "App/Configs/Scenes/" + nameof(SceneConfig))]
    public class SceneConfig : LoadableScriptableObject<SceneConfig>
    {
        [SerializeField] private List<SceneConfigData> _datas = new();

        public SceneConfigData Get(SceneType sceneType)
        {
            SceneConfigData data = _datas.FirstOrDefault(sd => sd.SceneType == sceneType);

            if (data == default)
                throw new ArgumentException($"Can not get SceneData with type:[{sceneType}]");

            return data;
        }
    }

    [Serializable]
    public class SceneConfigData
    {
        [field: SerializeField] public SceneType SceneType { get; private set; } = SceneType.none;
        [field: SerializeField] public int SceneBuildIndex { get; private set; } = -1;
        [field: SerializeField] public string SceneName { get; private set; } = "";
    }
}