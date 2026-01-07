using System.Linq;
using Extensions;
using MyFirstVisualNovel.Runtime.Core.UI;
using MyFirstVisualNovel.Runtime.VNGameplay;
using SosalkasGame.Runtime.Core.GameFSM;
using UnityEngine;
using UnityEngine.InputSystem;

using static ActionMap;

namespace MyFirstVisualNovel.Runtime.Core.GameFSM
{
    public class VNReadingState : GameFSMState
    {
        private VNGameplayUI _vnGameplayUI;
        private MainInputActions _mainInput;
        private InputAction _leftClick;
        private InputAction _rightClick;

        private string _defaultFontSettings;

        public VNReadingState(GameFSM fsm, GameStateModel model) : base(fsm, model)
        {
            _mainInput = this._model.ActionMap.MainInput;
            _leftClick = this._model.ActionMap.MainInput.LeftClick;
            _rightClick = this._model.ActionMap.MainInput.RightClick;
            _defaultFontSettings = "<color=blue>";
        }

        public override void Enter()
        {
            _vnGameplayUI = MonoBehaviour.FindFirstObjectByType<VNGameplayUI>();
            LoadFrame(this._model.FrameIndex);

            _leftClick.performed += NextFrame;
            _rightClick.performed += PreviousFrame;

            _mainInput.Enable();
            Debug.LogWarning($"VNGameplayState: Enter");
        }

        public override void Exit()
        {
            _leftClick.performed -= NextFrame;
            _rightClick.performed -= PreviousFrame;
            Debug.LogWarning($"VNGameplayState: Exit");
        }

        public override void Update() { }

        private void PreviousFrame(InputAction.CallbackContext context)
        {
            this._model.FramesHistory.Remove(this._model.FramesHistory.LastOrDefault());
            LoadFrame(this._model.FramesHistory.LastOrDefault(), false);
        }

        private void NextFrame(InputAction.CallbackContext context)
        {
            VNFrame frame;
            if (this._model.CurrentFrame.JumpToAnchorID.IsNullOrEmpty())
                frame = LoadFrame(this._model.FrameIndex + 1);
            else
                frame = LoadFrame(this._model.Anchors[this._model.CurrentFrame.JumpToAnchorID]);
        }

        private VNFrame LoadFrame(int frameIndex, bool saveHistory = true)
        {
            if (frameIndex > this._model.Frames.Length - 1 || frameIndex < 0)
                return null;

            VNFrame frame = this._model.Frames[frameIndex];
            _vnGameplayUI.BackgroundImage.texture = this._model.AssetStorage.GetAssetRef<Texture2D>(frame.BackgroundImageID);
            _vnGameplayUI.CharacterImage.texture = this._model.AssetStorage.GetAssetRef<Texture2D>(frame.CharacterImageID);
            _vnGameplayUI.CharacterName.text = frame.CharacterNameID;
            _vnGameplayUI.MainText.text = _defaultFontSettings + frame.MainText;

            this._model.FrameIndex = frameIndex;
            if (saveHistory)
                this._model.FramesHistory.Add(this._model.FrameIndex);
            if (frame.Choice is not null)
                this._fsm.SwitchStateTo<VNChoiceState>();

            return frame;
        }
    }
}