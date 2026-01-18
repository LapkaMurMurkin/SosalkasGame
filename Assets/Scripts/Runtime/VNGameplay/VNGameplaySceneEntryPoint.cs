using System;

using SosalkasGame.Runtime.Core.AssetStorage;
using SosalkasGame.Runtime.Core.GameFSM;
using SosalkasGame.Runtime.Core.SceneLoader;
using SosalkasGame.Runtime.Core;
using SosalkasGame.Runtime.VNGameplay.VNInteractivity;

using UnityEngine;

using VContainer.Unity;

namespace SosalkasGame.Runtime.VNGameplay
{
    public class VNGameplaySceneEntryPoint : IInitializable, IDisposable
    {
        private readonly GameStateModel _model;
        private readonly AssetStorage _assetStorage;
        private readonly SceneLoader _sceneLoader;
        private readonly GameFSM _gameState;
        private readonly UIRoot _uiRoot;
        private readonly VNGameplayUI _vnGameplayUI;

        public VNGameplaySceneEntryPoint(GameStateModel model, AssetStorage assetStorage, SceneLoader sceneLoader, GameFSM gameState, UIRoot uiRoot, VNGameplayUI vnGameplayUI)
        {
            _model = model;
            _assetStorage = assetStorage;
            _sceneLoader = sceneLoader;
            _gameState = gameState;
            _uiRoot = uiRoot;
            _vnGameplayUI = vnGameplayUI;
        }

        public void Initialize()
        {
            _vnGameplayUI.Initialize(_model, _assetStorage);
            _uiRoot.AddScreen(_vnGameplayUI.gameObject);

            _gameState.GetState<VNChoiceState>().ChoiceOptionsLoaded += _vnGameplayUI.ChoiceMenu.AddOptions;
            //_gameState.GetState<VNInteractiveState>().InteractiveLoaded += AddInteractiveScreen;
            _vnGameplayUI.ChoiceMenu.OptionSelected += _gameState.GetState<VNChoiceState>().MakeChoice;



            _gameState.SwitchStateTo<VNScenarioLoadingState>();
        }

        public void Dispose()
        {
            MonoBehaviour.Destroy(_vnGameplayUI.gameObject);

            _gameState.GetState<VNChoiceState>().ChoiceOptionsLoaded -= _vnGameplayUI.ChoiceMenu.AddOptions;
            //_gameState.GetState<VNInteractiveState>().InteractiveLoaded -= AddInteractiveScreen;
            _vnGameplayUI.ChoiceMenu.OptionSelected -= _gameState.GetState<VNChoiceState>().MakeChoice;
        }

        private void AddInteractiveScreen(TestInteractive testInteractive)
        {
            //_uiRoot.AddScreen(testInteractive.gameObject);
        }
    }
}


