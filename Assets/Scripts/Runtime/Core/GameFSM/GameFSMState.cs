using MyFirstVisualNovel.Runtime.Core.UI;

using Templates.FSM;

namespace MyFirstVisualNovel.Runtime.Core.GameFSM
{
    public abstract class GameFSMState : FSMState
    {
        protected GameFSM _fsm;
        protected GameStateModel _model;

        public GameFSMState(GameFSM fsm, GameStateModel model)
        {
            _fsm = fsm;
            _model = model;
        }
    }
}