using System;
using Extensions;
using SosalkasGame.Runtime.Core.GameFSM;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SosalkasGame.Runtime.VNGameplay.VNInteractive
{
    public class TestInteractive : MonoBehaviour
    {
        private VNInteractiveState _state;
        public Button TestButton;

        private int _crackCout;

        public Action InteractionEnded;

        public void Initialize(VNInteractiveState state)
        {
            _state = state;
            TestButton.onClick.AddListener(CrackStep);
        }

        public void OnDestroy()
        {
            TestButton.onClick.RemoveAllListeners();
        }

        private void CrackStep()
        {
            DOTweenAnimator.ShakePosition(TestButton.transform, 1f, 20);
            _state.LoadNextFrame(false);
            _crackCout++;
            if (_crackCout >= 3)
                InteractionEnded.Invoke();
        }
    }
}