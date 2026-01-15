using System;
using System.Collections.Generic;
using Extensions;
using SosalkasGame.Runtime.Core.GameFSM;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static ActionMap;

namespace SosalkasGame.Runtime.VNGameplay.VNInteractive
{
    public class TestInteractive
    {
        private TestInteractiveEntryPoint _testInteractiveEntryPoint;

        public TestInteractive(TestInteractiveEntryPoint testInteractiveEntryPoint)
        {
            _testInteractiveEntryPoint = testInteractiveEntryPoint;
        }

        public bool CheckIsImageClicked(Collider2D collider, out Image image)
        {
            image = null;

            if (collider is null)
                return false;

            if (collider.TryGetComponent<Image>(out image) is false)
                return false;

            image.enabled = true;
            collider.enabled = false;
            return true;
        }

        public void EndInteraction()
        {
            _testInteractiveEntryPoint.EndInteraction();
        }
    }
}