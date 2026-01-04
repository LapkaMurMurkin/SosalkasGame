using System;

using MyFirstVisualNovel.Runtime.Core.AssetStorage;
using MyFirstVisualNovel.Runtime.Core.SceneLoader;
using MyFirstVisualNovel.Runtime.Core.UI;

using UnityEngine;

using VContainer.Unity;

namespace MyFirstVisualNovel.Runtime.Core.GameEntryPoint
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
            await _sceneLoader.LoadScene(SceneID.MAIN_MENU);
        }
    }
}