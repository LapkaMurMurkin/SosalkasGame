using System;

using SosalkasGame.Runtime.Core.AssetStorage;
using SosalkasGame.Runtime.Core.SceneLoader;
using SosalkasGame.Runtime.Core;

using UnityEngine;

using VContainer.Unity;

namespace SosalkasGame.Runtime.Core.GameEntryPoint
{
    public class GameEntryPoint : IInitializable, IDisposable
    {
        private AssetStorage.AssetStorage _assetStorage;
        private SceneLoader.SceneLoader _sceneLoader;
        private UIRoot _uiRoot;

        public GameEntryPoint(AssetStorage.AssetStorage assetStorage, SceneLoader.SceneLoader sceneLoader, UIRoot uiRoot)
        {
            _assetStorage = assetStorage;
            _sceneLoader = sceneLoader;
            _uiRoot = uiRoot;
        }

        public void Initialize()
        {
            _sceneLoader.LoadStart += _uiRoot.ShowLoadingScreen;
            _sceneLoader.LoadEnd += _uiRoot.HideLoadingScreen;
            LoadGame();

            Debug.Log("GameEntryPoint - Initialize");
        }

        public void Dispose()
        {
            _sceneLoader.LoadStart -= _uiRoot.ShowLoadingScreen;
            _sceneLoader.LoadEnd -= _uiRoot.HideLoadingScreen;

            Debug.Log("GameEntryPoint - Dispose");
        }

        private async void LoadGame()
        {
            await _assetStorage.LoadAssetsByLable(AssetID.GROUP_ROOT);
            await _sceneLoader.LoadSceneAsync(SceneID.MAIN_MENU);
        }
    }
}