using MyFirstVisualNovel.Runtime.Core.UI;

using Templates.FSM;

namespace MyFirstVisualNovel.Runtime.Core.GameFSM
{
    public class GameFSMState : FSMState
    {
        protected AssetStorage.AssetStorage _assetStorage;
        protected GameFSM _fsm;
        protected GameStateModel _model;
        protected UIRoot _uiRoot;
        protected ActionMap _actionMap;

        public GameFSMState(GameFSM fsm, GameStateModel model, UIRoot uiRoot, ActionMap actionMap, AssetStorage.AssetStorage assetStorage)
        {
            _fsm = fsm;
            _model = model;
            _uiRoot = uiRoot;
            _actionMap = actionMap;
            _assetStorage = assetStorage;
        }
    }
}