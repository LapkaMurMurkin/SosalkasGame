using System;

using SosalkasGame.Runtime.Core.AssetStorage;
using SosalkasGame.Runtime.Core.GameFSM;
using SosalkasGame.Runtime.Core.SceneLoader;
using SosalkasGame.Runtime.Core;

using UnityEngine;

using VContainer.Unity;

namespace SosalkasGame.Runtime.MainMenu
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

