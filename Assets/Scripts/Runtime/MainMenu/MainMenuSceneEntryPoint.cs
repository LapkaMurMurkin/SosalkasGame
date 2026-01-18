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

        public MainMenuSceneEntryPoint(AssetStorage assetStorage, SceneLoader sceneLoader, GameFSM gameState, UIRoot uiRoot, MainMenuUI mainMenuUI)
        {
            _assetStorage = assetStorage;
            _sceneLoader = sceneLoader;
            _gameState = gameState;
            _uiRoot = uiRoot;
            _mainMenuUI = mainMenuUI;
        }

        public void Initialize()
        {
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

