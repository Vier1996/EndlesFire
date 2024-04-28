using System;
using Cysharp.Threading.Tasks;
using InternalAssets.Codebase.Services.Scenes.Enum;
using UnityEngine;

namespace InternalAssets.Codebase.Services.Scenes
{
    public interface ISceneLoader
    {
        public event Action SceneLoaded;
        
        public UniTask<AsyncOperation> LoadScene(SceneType sceneType);
    }
}