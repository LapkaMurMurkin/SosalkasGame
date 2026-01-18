using System.Linq;
using Extensions;
using SosalkasGame.Runtime.Core.GameFSM;
using SosalkasGame.Runtime.VNGameplay;
using UnityEngine.InputSystem;

namespace SosalkasGame.Runtime.Core.GameFSM
{
    public abstract class VNGameplayState : GameFSMState
    {
        protected VNGameplayState(SosalkasGame.Runtime.Core.GameFSM.GameFSM fsm, GameStateModel model) : base(fsm, model)
        {
        }

        protected void LoadPreviousFrame(InputAction.CallbackContext context)
        {
            this._model.FramesHistory.Remove(this._model.FramesHistory.LastOrDefault());
            LoadFrame(this._model.FramesHistory.LastOrDefault(), false);
        }

        protected void LoadNextFrame(InputAction.CallbackContext context) => LoadNextFrame(true);
        public void LoadNextFrame(bool saveHistory)
        {
            VNFrame frame;
            if (this._model.CurrentFrame.JumpToAnchorID.IsNullOrEmpty())
                frame = LoadFrame(this._model.FrameIndex + 1, saveHistory);
            else
                frame = LoadFrame(this._model.Anchors[this._model.CurrentFrame.JumpToAnchorID], saveHistory);
        }

        protected VNFrame LoadFrame(int frameIndex, bool saveHistory)
        {
            if (frameIndex > this._model.Frames.Length - 1 || frameIndex < 0)
                return null;

            VNFrame frame = this._model.Frames[frameIndex];

            this._model.FrameIndex = frameIndex;
            if (saveHistory)
                this._model.FramesHistory.Add(this._model.FrameIndex);
            if (frame.Choice is not null)
                this._fsm.SwitchStateTo<VNChoiceState>();
            if (frame.Interactive is not null)
                this._fsm.SwitchStateTo<VNInteractivityState>();

            return frame;
        }
    }
}