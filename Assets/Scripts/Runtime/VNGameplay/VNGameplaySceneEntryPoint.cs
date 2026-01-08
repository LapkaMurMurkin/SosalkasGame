using System;

using MyFirstVisualNovel.Runtime.Core.AssetStorage;
using MyFirstVisualNovel.Runtime.Core.GameFSM;
using MyFirstVisualNovel.Runtime.Core.SceneLoader;
using MyFirstVisualNovel.Runtime.Core.UI;
using SosalkasGame.Runtime.Core.GameFSM;
using SosalkasGame.Runtime.VNGameplay.VNInteractive;

using UnityEngine;

using VContainer.Unity;

namespace MyFirstVisualNovel.Runtime.VNGameplay
{
    public class VNGameplaySceneEntryPoint : IInitializable, IDisposable
    {
        private readonly GameStateModel _model;
        private AssetStorage _assetStorage;
        private SceneLoader _sceneLoader;
        private GameFSM _gameState;
        private UIRoot _uiRoot;
        private VNGameplayUI _vnGameplayUI;

        public VNGameplaySceneEntryPoint(GameStateModel model, AssetStorage assetStorage, SceneLoader sceneLoader, GameFSM gameState, UIRoot uiRoot)
        {
            _model = model;
            _assetStorage = assetStorage;
            _sceneLoader = sceneLoader;
            _gameState = gameState;
            _uiRoot = uiRoot;
        }

        public void Initialize()
        {
            _vnGameplayUI = _assetStorage.InstantiateAsset<VNGameplayUI>(AssetID.VN_GAMEPLAY_UI);
            _vnGameplayUI.Initialize(_model);
            _uiRoot.AddScreen(_vnGameplayUI.gameObject);

            _gameState.GetState<VNChoiceState>().ChoiceOptionsLoaded += _vnGameplayUI.ChoiceMenu.AddOptions;
            _gameState.GetState<VNInteractiveState>().InteractiveLoaded += AddInteractiveScreen;
            _vnGameplayUI.ChoiceMenu.OptionSelected += _gameState.GetState<VNChoiceState>().MakeChoice;



            _gameState.SwitchStateTo<VNScenarioLoadingState>();
        }

        public void Dispose()
        {
            MonoBehaviour.Destroy(_vnGameplayUI.gameObject);

            _gameState.GetState<VNChoiceState>().ChoiceOptionsLoaded -= _vnGameplayUI.ChoiceMenu.AddOptions;
            _gameState.GetState<VNInteractiveState>().InteractiveLoaded -= AddInteractiveScreen;
            _vnGameplayUI.ChoiceMenu.OptionSelected -= _gameState.GetState<VNChoiceState>().MakeChoice;
        }

        private void AddInteractiveScreen(TestInteractive testInteractive)
        {
            _uiRoot.AddScreen(testInteractive.gameObject);
        }
    }
}


