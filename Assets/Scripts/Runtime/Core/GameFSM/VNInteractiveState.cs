
using System;
using MyFirstVisualNovel.Runtime.Core.GameFSM;
using MyFirstVisualNovel.Runtime.Core.UI;
using SosalkasGame.Runtime.VNGameplay.VNInteractive;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace SosalkasGame.Runtime.Core.GameFSM
{
    public class VNInteractiveState : VNGameplayState
    {
        public TestInteractive Interactive { get; private set; }
        public Action<TestInteractive> InteractiveLoaded;

        public VNInteractiveState(MyFirstVisualNovel.Runtime.Core.GameFSM.GameFSM fsm, GameStateModel model) : base(fsm, model)
        {

        }

        public override void Enter()
        {
            Interactive = this._model.AssetStorage.InstantiateAsset<TestInteractive>(this._model.CurrentFrame.Interactive[0]);
            Interactive.Initialize(this);
            Interactive.InteractionEnded += EndInteraction;
            InteractiveLoaded.Invoke(Interactive);
        }

        public override void Exit()
        {
            Interactive.InteractionEnded -= EndInteraction;
            GameObject.Destroy(Interactive.gameObject);
        }

        public override void Update() { }

        private void EndInteraction()
        {
            this._fsm.SwitchStateTo<VNReadingState>();
        }
    }
}