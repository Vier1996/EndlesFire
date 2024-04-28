using System;
using ACS.Core.ServicesContainer;
using Codebase.Library.Extension.Rx;
using InternalAssets.Codebase.Interfaces;
using InternalAssets.Codebase.Library.CustomSliders;
using InternalAssets.Codebase.Services.Scenes;
using InternalAssets.Codebase.Services.Scenes.Enum;
using UniRx;
using UnityEngine;

namespace InternalAssets.Codebase.Bootstrappers
{
    public class LoadingSceneBootstrapper : ServiceContainerLocal
    {
        [SerializeField] private OffsetSlider _loadingProgressSlider;

        private IDisposable _operationDisposable;
        private ISceneLoader _sceneLoader;
        private IShutter _shutter;
        private AsyncOperation _loadOperation;
        
        protected override void Bootstrap()
        {
            Container.AsScene();
            
            ServiceContainer.Global.Get(out _sceneLoader);
            ServiceContainer.Global.Get(out _shutter);
        }

        private void Start()
        {
            UpdateSliderProgress(0);

            _operationDisposable = RX.Delay(1f, LoadGameScene);
        }

        private async void LoadGameScene()
        {
            _loadOperation = await _sceneLoader.LoadScene(SceneType.game_scene);
            _loadOperation.allowSceneActivation = false;

            _operationDisposable?.Dispose();
            _operationDisposable = Observable.EveryUpdate().Subscribe(_ => LoadFrame());
        }

        private void LoadFrame()
        {
            UpdateSliderProgress(_loadOperation.progress);
            
            if (_loadOperation.progress >= 0.75f) 
                OnSceneLoaded();
        }

        private void OnSceneLoaded()
        {
            _operationDisposable?.Dispose();
            
            _shutter.DisplayShutter();

            _operationDisposable = RX.Delay(1f, AllowSceneChanging);
        }

        private void AllowSceneChanging()
        {
            _operationDisposable?.Dispose();
            
            _loadOperation.allowSceneActivation = true;
        }

        private void UpdateSliderProgress(float normalizedProgress) => 
            _loadingProgressSlider.SetNormalizedProgress(normalizedProgress);
    }
}