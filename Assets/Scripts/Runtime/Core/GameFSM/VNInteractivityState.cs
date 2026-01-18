
using System;
using SosalkasGame.Runtime.Core.GameFSM;
using SosalkasGame.Runtime.Core;
using SosalkasGame.Runtime.VNGameplay.VNInteractivity;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using VContainer.Unity;
using SosalkasGame.Runtime.VNGameplay;

namespace SosalkasGame.Runtime.Core.GameFSM
{
    public class VNInteractivityState : VNGameplayState
    {
        private InteractivityLifetimeScope _interactivityLifetimeScope;
        private int _exitFrameIndex;
        //public Action<TestInteractive> InteractiveLoaded;

        public VNInteractivityState(SosalkasGame.Runtime.Core.GameFSM.GameFSM fsm, GameStateModel model) : base(fsm, model)
        {

        }

        public override void Enter()
        {
            VNFrame frame = this._model.CurrentFrame;
            
            _interactivityLifetimeScope = this._fsm.AssetStorage.InstantiateAsset<InteractivityLifetimeScope>(frame.Interactive[0]);

            if (frame.Interactive.Length > 1)
                _exitFrameIndex = this._model.Anchors[frame.Interactive[1]];
            else
                _exitFrameIndex = this._model.FrameIndex + 1;

            _interactivityLifetimeScope.InteractionEnded += EndInteraction;
        }

        public override void Exit()
        {
            _interactivityLifetimeScope.InteractionEnded -= EndInteraction;
            this._model.FrameIndex = _exitFrameIndex;
        }

        public override void Update() { }

        private void EndInteraction()
        {
            _interactivityLifetimeScope.Dispose();
            this._fsm.SwitchStateTo<VNReadingState>();
        }
    }
}