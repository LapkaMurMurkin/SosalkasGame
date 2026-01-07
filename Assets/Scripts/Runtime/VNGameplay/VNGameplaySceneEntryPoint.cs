using System;

using MyFirstVisualNovel.Runtime.Core.AssetStorage;
using MyFirstVisualNovel.Runtime.Core.GameFSM;
using MyFirstVisualNovel.Runtime.Core.SceneLoader;
using MyFirstVisualNovel.Runtime.Core.UI;
using SosalkasGame.Runtime.Core.GameFSM;

using UnityEngine;

using VContainer.Unity;

namespace MyFirstVisualNovel.Runtime.VNGameplay
{
    public class VNGameplaySceneEntryPoint : IInitializable, IDisposable
    {
        private AssetStorage _assetStorage;
        private SceneLoader _sceneLoader;
        private GameFSM _gameState;
        private UIRoot _uiRoot;
        private VNGameplayUI _vnGameplayUI;

        public VNGameplaySceneEntryPoint(AssetStorage assetStorage, SceneLoader sceneLoader, GameFSM gameState, UIRoot uiRoot)
        {
            _assetStorage = assetStorage;
            _sceneLoader = sceneLoader;
            _gameState = gameState;
            _uiRoot = uiRoot;
        }

        public void Initialize()
        {
            _vnGameplayUI = _assetStorage.InstantiateAsset<VNGameplayUI>(AssetID.VN_GAMEPLAY_UI);
            _vnGameplayUI.Initialize();
            _uiRoot.AddScreen(_vnGameplayUI.gameObject);

            _gameState.GetState<VNChoiceState>().ChoiceOptionsLoaded += _vnGameplayUI.ChoiceMenu.AddOptions;
            _vnGameplayUI.ChoiceMenu.OptionSelected += _gameState.GetState<VNChoiceState>().MakeChoice;

            _gameState.SwitchStateTo<VNScenarioLoadingState>();
        }

        public void Dispose()
        {
            MonoBehaviour.Destroy(_vnGameplayUI.gameObject);

            _gameState.GetState<VNChoiceState>().ChoiceOptionsLoaded -= _vnGameplayUI.ChoiceMenu.AddOptions;
            _vnGameplayUI.ChoiceMenu.OptionSelected -= _gameState.GetState<VNChoiceState>().MakeChoice;
        }
    }
}


