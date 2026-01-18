using System;
using SosalkasGame.Runtime.Core.GameFSM;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace SosalkasGame.Runtime.VNGameplay.VNInteractivity.Stars.Stars_Birth1
{
    public class StarsBirth1UI : InteractivityUI
    {
        private StarsBirth1Presenter _presenter;
        private ActionMap _actionMap;

        public Image SpaceBackground;
        public Image Star;

        [Inject]
        public void Initialize(StarsBirth1Presenter presenter, ActionMap actionMap)
        {
            _presenter = presenter;
            _actionMap = actionMap;
            _actionMap.MainInput.LeftClick.performed += CheckIsImageClicked;
        }

        public override void Dispose()
        {
            _actionMap.MainInput.LeftClick.performed -= CheckIsImageClicked;
            base.Dispose();
        }

        private void CheckIsImageClicked(InputAction.CallbackContext context)
        {
            Debug.Log("StarInteraction");
            _presenter.EndInteraction();
        }
    }
}