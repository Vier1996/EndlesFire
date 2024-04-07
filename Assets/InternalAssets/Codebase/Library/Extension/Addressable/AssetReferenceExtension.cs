using Codebase.Library.Addressable;
using Codebase.Library.Extension.Native;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Codebase.Library.Extension.Addressable
{
    public static class AssetReferenceExtension
    {
        public static async UniTask<ReferencedAssetData<T>> InstantiateReference<T>(this AssetComponentReference<T> asset, 
            Transform parent = null, Vector3 localPosition = default, Vector3 localRotation = default, Vector3 localScale = default) where T : IComponentReferenceInstance
        {
            AsyncOperationHandle<T> instantiateHandle = asset.InstantiateAsync();

            await instantiateHandle.Task;

            ReferencedAssetData<T> referencedAssetData = new ReferencedAssetData<T>(instantiateHandle);
            
            referencedAssetData.AssetInstance
                .GetObject().transform
                .With(tr => tr.SetParent(parent))
                .With(tr => tr.localScale = (localScale == default ? Vector3.one : localScale))
                .With(tr => tr.localEulerAngles = localRotation)
                .With(tr => tr.localPosition = localPosition);

            return referencedAssetData;
        }
    }
}