
using System;
using SosalkasGame.Runtime.Core.GameFSM;
using SosalkasGame.Runtime.Core;
using SosalkasGame.Runtime.VNGameplay.VNInteractive;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using VContainer.Unity;

namespace SosalkasGame.Runtime.Core.GameFSM
{
    public class VNInteractiveState : VNGameplayState
    {
        private TestInteractiveLifeTimeScope _testInteractiveLifeTimeScope;
        //public Action<TestInteractive> InteractiveLoaded;

        public VNInteractiveState(SosalkasGame.Runtime.Core.GameFSM.GameFSM fsm, GameStateModel model) : base(fsm, model)
        {

        }

        public override void Enter()
        {
            //Interactive = this._fsm.AssetStorage.InstantiateAsset<TestInteractive>(this._model.CurrentFrame.Interactive[0]);
            _testInteractiveLifeTimeScope = this._fsm.GameLifetimeScope.CreateChild<TestInteractiveLifeTimeScope>();
            _testInteractiveLifeTimeScope.InteractionEnded += EndInteraction;
        }

        public override void Exit()
        {
            _testInteractiveLifeTimeScope.InteractionEnded -= EndInteraction;
        }

        public override void Update() { }

        private void EndInteraction()
        {
            _testInteractiveLifeTimeScope.Dispose();
            this._fsm.SwitchStateTo<VNReadingState>();
        }
    }
}