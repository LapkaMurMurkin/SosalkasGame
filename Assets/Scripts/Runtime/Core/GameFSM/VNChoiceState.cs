using System;
using System.Linq;
using MyFirstVisualNovel.Runtime.Core.GameFSM;

namespace SosalkasGame.Runtime.Core.GameFSM
{
    public class VNChoiceState : GameFSMState
    {
        public Action<string[][]> ChoiceOptionsLoaded;

        public VNChoiceState(MyFirstVisualNovel.Runtime.Core.GameFSM.GameFSM fsm, GameStateModel model) : base(fsm, model)
        {
        }

        public override void Enter()
        {
            ChoiceOptionsLoaded.Invoke(this._model.CurrentFrame.Choice);
        }

        public override void Exit()
        {

        }

        public override void Update() { }


        public void MakeChoice(int optionIndex)
        {
            this._model.FrameIndex = this._model.Anchors[this._model.CurrentFrame.Choice.ElementAt(optionIndex)[0]];
            this._fsm.SwitchStateTo<VNReadingState>();
        }
    }
}