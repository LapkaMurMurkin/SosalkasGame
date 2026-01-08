using MyFirstVisualNovel.Runtime.Core.UI;
using SosalkasGame.Runtime.Core.GameFSM;
using Templates.FSM;
using UnityEngine;
using VContainer.Unity;

namespace MyFirstVisualNovel.Runtime.Core.GameFSM
{
    public class GameFSM : FSM, ITickable
    {
        private readonly ActionMap _actionMap;
        private readonly GameStateModel _model;

        public GameFSM(GameStateModel model, ActionMap actionMap, UIRoot uiRoot, AssetStorage.AssetStorage assetStorage, SceneLoader.SceneLoader sceneLoader)
        {
            _actionMap = actionMap;
            _model = model;
            _model.AssetStorage = assetStorage;
            _model.ActionMap = actionMap;
            _model.SceneLoader = sceneLoader;
            _model.UIRoot = uiRoot;

            this.InitializeState(new MainMenuState(this, _model));
            this.InitializeState(new VNScenarioLoadingState(this, _model));
            this.InitializeState(new VNReadingState(this, _model));
            this.InitializeState(new VNChoiceState(this, _model));
            this.InitializeState(new VNInteractiveState(this, _model));
        }

        public void Tick()
        {
            Update();
            //Debug.LogWarning($"GameFSM Update");
        }
    }
}