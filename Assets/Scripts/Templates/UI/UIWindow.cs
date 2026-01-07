using Template.UI;
using UnityEngine.UI;

namespace SosalkasGame.Templates.UI
{
    public class UIWindow : UIElement
    {
        public Button CloseButton;

        public override void Initialize()
        {
            base.Initialize();
            CloseButton.onClick.AddListener(this.Hide);
        }

        protected override void OnDestroy()
        {
            CloseButton.onClick.RemoveAllListeners();
        }
    }
}