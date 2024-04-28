using System;
using Cysharp.Threading.Tasks;
using InternalAssets.Codebase.Services.Scenes;
using InternalAssets.Codebase.Services.Scenes.Data;
using InternalAssets.Codebase.Services.Scenes.Enum;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InternalAssets.Codebase.Gameplay.SceneLoader
{
    public class EndlessFireSceneLoader : ISceneLoader, IDisposable
    {
        public event Action SceneLoaded;
        
        public EndlessFireSceneLoader() { }

        public void Dispose() { }

        public async UniTask<AsyncOperation> LoadScene(SceneType sceneType)
        {
            SceneConfig sceneConfig = await SceneConfig.GetInstanceAsync();
            SceneConfigData data = sceneConfig.Get(sceneType);
            
            return SceneManager.LoadSceneAsync(data.SceneBuildIndex);
        }
    }
}