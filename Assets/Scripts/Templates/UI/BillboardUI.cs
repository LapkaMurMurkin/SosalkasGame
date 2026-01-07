
using UnityEngine;

namespace Template.UI
{
    public class BillboardUI : UIElement
    {
        protected Transform _playerCameraTransform;

        public override void Initialize()
        {
            base.Initialize();
            //_playerCameraTransform = GameScene.GetService<PlayerCamera>().transform;
        }

        private void LateUpdate()
        {
            RotateToCamera();
        }

        private void RotateToCamera()
        {
            transform.LookAt(transform.position + _playerCameraTransform.forward);
        }
    }
}