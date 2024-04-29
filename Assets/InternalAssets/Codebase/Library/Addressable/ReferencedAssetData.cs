using System;
using InternalAssets.Codebase.Library.Addressable;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Codebase.Library.Addressable
{
    public class ReferencedAssetData<T> : IDisposable
    {
        public T Asset { get; private set; }
        public AsyncOperationHandle<T> InstantiateHandle { get; private set; }
        public IComponentReferenceInstance AssetInstance { get; private set; }

        public ReferencedAssetData(AsyncOperationHandle<T> instantiateHandle)
        {
            InstantiateHandle = instantiateHandle;
            Asset = InstantiateHandle.Result;
            AssetInstance = Asset as IComponentReferenceInstance;
        }

        public void Dispose()
        {
            if (AssetInstance == null || AssetInstance.GameObject == null) return;
            
            Object.Destroy(AssetInstance.GameObject);
                
            if(InstantiateHandle.IsValid())
                Addressables.Release(InstantiateHandle);
        }
    }
}