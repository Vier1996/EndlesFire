using System;
using System.Collections.Generic;
using System.Linq;
using Codebase.Library.Extension.ScriptableObject;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InternalAssets.Codebase.Library.Logging.Configs
{
    [CreateAssetMenu(fileName = nameof(LogConfig), menuName = "App/Logger/" + nameof(LogConfig))]
    public class LogConfig : LoadableScriptableObject<LogConfig>
    {
        [field: SerializeField, BoxGroup("Params")] public string Prefix { get; private set; } = "[Logger] => ";
        [field: SerializeField, BoxGroup("Params")] public bool OnlyInEditor { get; private set; } = true;
        
        [BoxGroup("Params"), SerializeField] private List<LogConfigData> _logConfigData = new();

        public LogConfigData Get(LogType logType)
        {
            LogConfigData data = _logConfigData.FirstOrDefault(lcd => lcd.LogType == logType);

            if (data == default)
                throw new ArgumentException($"Can not get config with LogType:[{logType}]");

            return data;
        }
    }
}