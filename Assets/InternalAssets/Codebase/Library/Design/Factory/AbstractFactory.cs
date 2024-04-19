using System;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;

namespace InternalAssets.Codebase.Library.Design.Factory
{
    public abstract class AbstractFactory<TFactoryType, TFactoryValue> : IDisposable where TFactoryValue : Component
    {
        private IFactory<TFactoryValue, TFactoryType> _factory;
        
        public TFactoryValue SpawnFactoryItem(TFactoryType type, Vector3 spawnPoint, Quaternion rotation = default, Transform parent = null)
        {
            TFactoryValue value = GetFactoryItem(type);
            
            return LeanPool.Spawn(value, spawnPoint, rotation == default ? Quaternion.identity : value.transform.rotation, parent);
        }
        
        public async UniTask<TFactoryValue> SpawnFactoryItemAsync(TFactoryType type, Vector3 spawnPoint, Quaternion rotation = default, Transform parent = null)
        {
            TFactoryValue value = await GetFactoryItemAsync(type);
            
            return LeanPool.Spawn(value, spawnPoint, rotation == default ? Quaternion.identity : value.transform.rotation, parent);
        }
        
        protected abstract IFactory<TFactoryValue, TFactoryType> GetSetup();
        protected abstract UniTask<IFactory<TFactoryValue, TFactoryType>> GetSetupAsync();
        protected abstract void ReleaseSetup();

        private TFactoryValue GetFactoryItem(TFactoryType type) => (_factory ??= GetSetup()).GetFactoryValue(type);
        
        private async UniTask<TFactoryValue> GetFactoryItemAsync(TFactoryType type) => (_factory ??= await GetSetupAsync()).GetFactoryValue(type);
        
        public void Dispose() => ReleaseSetup();
    }
}