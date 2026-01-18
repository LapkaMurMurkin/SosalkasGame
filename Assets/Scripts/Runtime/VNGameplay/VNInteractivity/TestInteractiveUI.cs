using System;
using System.Collections.Generic;
using SosalkasGame.Runtime.Core.GameFSM;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace SosalkasGame.Runtime.VNGameplay.VNInteractivity
{
    public class TestInteractiveUI : MonoBehaviour
    {
        private VNInteractivityState _state;
        private TestInteractive _presenter;
        private ActionMap _actionMap;
        [SerializeField]
        private List<Image> _images;

        private int _crackCout;

        public void Initialize(VNInteractivityState state, TestInteractive presenter, ActionMap actionMap)
        {
            _state = state;
            _presenter = presenter;
            _actionMap = actionMap;

            foreach (Image image in _images)
                image.enabled = false;

            _actionMap.MainInput.LeftClick.performed += CheckIsImageClicked;
        }

        private void CheckIsImageClicked(InputAction.CallbackContext context)
        {
            Vector2 mousePos = _actionMap.MainInput.MousePosition.ReadValue<Vector2>();
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (_presenter.CheckIsImageClicked(hit.collider, out Image image) is false)
                return;

            if (_images.Remove(image) is false)
                return;

            _state.LoadNextFrame(false);
            if (_images.Count is 0)
                _presenter.EndInteraction();
        }

        private void OnDestroy()
        {
            _actionMap.MainInput.LeftClick.performed -= CheckIsImageClicked;
        }
    }
}