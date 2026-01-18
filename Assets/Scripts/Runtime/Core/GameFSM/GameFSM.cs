using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SosalkasGame.Runtime.Core;
using SosalkasGame.Runtime.Core.GameEntryPoint;
using SosalkasGame.Runtime.Core.GameFSM;
using SosalkasGame.Runtime.Core.SceneLoader;
using Templates.FSM;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace SosalkasGame.Runtime.Core.GameFSM
{
    public class GameFSM : FSM, ITickable
    {
        public readonly GameLifetimeScope GameLifetimeScope;
        public readonly SceneLoader.SceneLoader SceneLoader;
        public readonly AssetStorage.AssetStorage AssetStorage;
        public readonly ActionMap ActionMap;

        private readonly GameStateModel _model;

        public GameFSM(GameLifetimeScope gameLifetimeScope, GameStateModel model, ActionMap actionMap, AssetStorage.AssetStorage assetStorage, SceneLoader.SceneLoader sceneLoader)
        {
            _model = model;
            GameLifetimeScope = gameLifetimeScope;
            SceneLoader = sceneLoader;
            AssetStorage = assetStorage;
            ActionMap = actionMap;

            this.InitializeState(new MainMenuState(this, _model));
            this.InitializeState(new VNScenarioLoadingState(this, _model));
            this.InitializeState(new VNReadingState(this, _model));
            this.InitializeState(new VNChoiceState(this, _model));
            this.InitializeState(new VNInteractivityState(this, _model));
        }

        public void Tick()
        {
            Update();
            //Debug.Log($"GameFSM Update");
        }
    }
}