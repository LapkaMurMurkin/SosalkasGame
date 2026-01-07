using System;

using MyFirstVisualNovel.Runtime.Core.AssetStorage;
using MyFirstVisualNovel.Runtime.Core.GameFSM;
using MyFirstVisualNovel.Runtime.Core.SceneLoader;
using MyFirstVisualNovel.Runtime.Core.UI;

using UnityEngine;

using VContainer.Unity;

namespace MyFirstVisualNovel.Runtime.MainMenu
{
    public class MainMenuSceneEntryPoint : IInitializable, IDisposable
    {
        private AssetStorage _assetStorage;
        private SceneLoader _sceneLoader;
        private GameFSM _gameState;
        private UIRoot _uiRoot;
        private MainMenuUI _mainMenuUI;

        public MainMenuSceneEntryPoint(AssetStorage assetStorage, SceneLoader sceneLoader, GameFSM gameState, UIRoot uiRoot)
        {
            _assetStorage = assetStorage;
            _sceneLoader = sceneLoader;
            _gameState = gameState;
            _uiRoot = uiRoot;
        }

        public void Initialize()
        {
            _mainMenuUI = _assetStorage.InstantiateAsset<MainMenuUI>(AssetID.MAIN_MENU_UI);
            _mainMenuUI.Initialize();
            _uiRoot.AddScreen(_mainMenuUI.gameObject);

            _gameState.SwitchStateTo<MainMenuState>();
        }

        public void Dispose()
        {
            MonoBehaviour.Destroy(_mainMenuUI.gameObject);
            Debug.Log("MainMenuScene - Dispose");
        }
    }
}

