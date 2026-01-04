using System;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace MyFirstVisualNovel.Runtime.Core.AssetStorage
{
    public class AssetStorage : IDisposable
    {
        private readonly Dictionary<string, AsyncOperationHandle> _assetRefCache;

        public AssetStorage()
        {
            _assetRefCache = new Dictionary<string, AsyncOperationHandle>();
        }

        public T InstantiateAsset<T>(string assetID, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion(), Transform parent = null) where T : Component
        {
            T assetRef = GetAssetRef<T>(assetID);
            return GameObject.Instantiate(assetRef, position, rotation, parent);
        }

        public T GetAssetRef<T>(string assetID)
        {
            if (assetID is null)
            {
                Debug.LogError("Null ID");
                return default;
            }

            if (_assetRefCache.TryGetValue(assetID, out AsyncOperationHandle handle))
                return ExtractAssetRef<T>(handle);

            //throw new Exception("asset not loaded");
            Debug.LogError("Asset ID not found. It's not loadedd or check name string, perhaps?");
            return default;
        }

        public async UniTask<List<IResourceLocation>> LoadAssetsByLable(string lable)
        {
            AsyncOperationHandle<IList<IResourceLocation>> mainHandle = Addressables.LoadResourceLocationsAsync(lable);
            await mainHandle;

            List<IResourceLocation> resourceLocations = new List<IResourceLocation>(mainHandle.Result);
            mainHandle.Release();

            foreach (IResourceLocation resourceLocation in resourceLocations)
                await LoadAssetRefAsync(resourceLocation.PrimaryKey);

            return resourceLocations;
        }

        public async UniTask<T> LoadAndInstantiateAssetAsync<T>(string assetID, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion(), Transform parent = null) where T : Component
        {
            T assetRef = await LoadAssetRefAsync<T>(assetID);
            return GameObject.Instantiate(assetRef, position, rotation, parent);
        }

        public async UniTask<T> LoadAssetRefAsync<T>(string assetID)
        {
            AsyncOperationHandle handle = await LoadAssetRefAsync(assetID);
            return ExtractAssetRef<T>(handle);
        }

        private async UniTask<AsyncOperationHandle> LoadAssetRefAsync(string assetID)
        {
            if (_assetRefCache.ContainsKey(assetID) && _assetRefCache[assetID].IsValid())
                return _assetRefCache[assetID];

            AsyncOperationHandle handle = Addressables.LoadAssetAsync<object>(assetID);
            await handle;

            _assetRefCache[assetID] = handle;

            return handle;
        }

        private T ExtractAssetRef<T>(AsyncOperationHandle handle)
        {
            if (handle.IsValid() is false || handle.Status != AsyncOperationStatus.Succeeded)
                throw new Exception("handle status is invalid");

            if (typeof(T).IsSubclassOf(typeof(Component)))
            {
                T component = (handle.Result as GameObject).GetComponent<T>();
                if (component is null)
                    Debug.LogError("Wrong component type?");
                return component;
            }

            return (T)handle.Result;
        }

        public void Dispose()
        {
            foreach (AsyncOperationHandle handle in _assetRefCache.Values)
                handle.Release();

            Debug.Log("AssetStorage - Dispose");
        }
    }
}
