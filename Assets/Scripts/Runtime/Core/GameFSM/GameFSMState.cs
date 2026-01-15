using SosalkasGame.Runtime.Core;

using Templates.FSM;

namespace SosalkasGame.Runtime.Core.GameFSM
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