using System.Linq;
using Extensions;
using SosalkasGame.Runtime.Core;
using SosalkasGame.Runtime.VNGameplay;
using SosalkasGame.Runtime.Core.GameFSM;
using UnityEngine;
using UnityEngine.InputSystem;

using static ActionMap;

namespace SosalkasGame.Runtime.Core.GameFSM
{
    public class VNReadingState : VNGameplayState
    {
        private VNGameplayUI _vnGameplayUI;
        private MainInputActions _mainInput;
        private InputAction _leftClick;
        private InputAction _rightClick;

        private string _defaultFontSettings;

        public VNReadingState(GameFSM fsm, GameStateModel model) : base(fsm, model)
        {
            _mainInput = this._fsm.ActionMap.MainInput;
            _leftClick = this._fsm.ActionMap.MainInput.LeftClick;
            _rightClick = this._fsm.ActionMap.MainInput.RightClick;
            //_defaultFontSettings = "<color=blue>";
        }

        public override void Enter()
        {
            _vnGameplayUI = MonoBehaviour.FindFirstObjectByType<VNGameplayUI>();
            LoadFrame(this._model.FrameIndex, true);

            _leftClick.performed += LoadNextFrame;
            _rightClick.performed += LoadPreviousFrame;

            _mainInput.Enable();
            Debug.LogWarning($"VNGameplayState: Enter");
        }

        public override void Exit()
        {
            _leftClick.performed -= LoadNextFrame;
            _rightClick.performed -= LoadPreviousFrame;
            Debug.LogWarning($"VNGameplayState: Exit");
        }

        public override void Update() { }


    }
}