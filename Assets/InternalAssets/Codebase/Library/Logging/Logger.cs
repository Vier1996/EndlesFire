using InternalAssets.Codebase.Library.Extension.Colors;
using InternalAssets.Codebase.Library.Logging.Configs;
using UnityEngine;
using Color = UnityEngine.Color;

namespace InternalAssets.Codebase.Library.Logging
{
    public static class Logger
    {
        private static readonly LogConfig Config;

        static Logger()
        {
            Config = LogConfig.GetInstance();
            
            if(Application.platform is not RuntimePlatform.WindowsEditor and RuntimePlatform.OSXEditor && Config.OnlyInEditor)
                Config.Release();
        }
        
        public static void Log(string message, LogType logType = LogType.Log)
        {
            if(Config == null) return;

            Color color = Config.Get(logType).LogColor;
            
            Debug.Log($"<color={color.ToHtml()}>{Config.Prefix}</color>{message}");
        }
    }
}
