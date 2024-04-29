using System;
using UnityEngine;

namespace InternalAssets.Codebase.Library.Logging.Configs
{
    [Serializable]
    public class LogConfigData
    {
        [field: SerializeField] public LogType LogType { get; private set; } = LogType.Log;
        [field: SerializeField] public Color LogColor { get; private set; } = Color.white;
    }
}