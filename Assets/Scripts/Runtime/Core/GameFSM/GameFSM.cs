using MyFirstVisualNovel.Runtime.Core.UI;

using Templates.FSM;
using UnityEngine;
using VContainer.Unity;

namespace MyFirstVisualNovel.Runtime.Core.GameFSM
{
    public class GameFSM : FSM, ITickable
    {
        private readonly ActionMap _actionMap;
        public GameStateModel Model;

        public GameFSM(ActionMap actionMap, UIRoot uiRoot, AssetStorage.AssetStorage assetStorage)
        {
            _actionMap = actionMap;
            Model = new GameStateModel();
            this.InitializeState(new MainMenuState());
            this.InitializeState(new VNScenarioLoadingState(this, Model, uiRoot, _actionMap, assetStorage));
            this.InitializeState(new VNGameplayState(this, Model, uiRoot, _actionMap, assetStorage));
        }

        public void Tick()
        {
            Update();
            //Debug.LogWarning($"GameFSM Update");
        }
    }
}