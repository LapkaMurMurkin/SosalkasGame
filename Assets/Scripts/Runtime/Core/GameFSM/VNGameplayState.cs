using MyFirstVisualNovel.Runtime.Core.UI;
using MyFirstVisualNovel.Runtime.VNGameplay;

using UnityEngine;
using UnityEngine.InputSystem;

using static ActionMap;

namespace MyFirstVisualNovel.Runtime.Core.GameFSM
{
    public class VNGameplayState : GameFSMState
    {
        private VNGameplayUI _vnGameplayUI;
        private MainInputActions _mainInput;
        private InputAction _leftClick;
        private InputAction _rightClick;

        public VNGameplayState(GameFSM fsm, GameStateModel model, UIRoot uiRoot, ActionMap actionMap, AssetStorage.AssetStorage assetStorage) : base(fsm, model, uiRoot, actionMap, assetStorage)
        {
            _mainInput = this._actionMap.MainInput;
            _leftClick = this._actionMap.MainInput.LeftClick;
            _rightClick = this._actionMap.MainInput.RightClick;
            //_vnGameplayUI = MonoBehaviour.FindFirstObjectByType<VNGameplayUI>();
        }

        public override void Enter()
        {
            _vnGameplayUI = MonoBehaviour.FindFirstObjectByType<VNGameplayUI>();
            LoadFrame(this._model.CurrentFrameIndex);

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
            VNFrame frame = LoadFrame(this._model.CurrentFrameIndex - 1);
            if (frame is not null)
                this._model.CurrentFrameIndex--;
        }

        private void NextFrame(InputAction.CallbackContext context)
        {
            VNFrame frame = LoadFrame(this._model.CurrentFrameIndex + 1);
            if (frame is not null)
                this._model.CurrentFrameIndex++;
        }

        private VNFrame LoadFrame(int frameIndex)
        {
            if (frameIndex > this._model.Frames.Length - 1 || frameIndex < 0)
                return null;

            VNFrame frame = this._model.Frames[frameIndex];
            _vnGameplayUI.BackgroundImage.texture = this._assetStorage.GetAssetRef<Texture2D>(frame.BackgroundImageID);
            _vnGameplayUI.CharacterImage.texture = this._assetStorage.GetAssetRef<Texture2D>(frame.CharacterImageID);
            _vnGameplayUI.CharacterName.text = frame.CharacterNameID;
            _vnGameplayUI.MainText.text = frame.MainText;

            return frame;
        }
    }
}