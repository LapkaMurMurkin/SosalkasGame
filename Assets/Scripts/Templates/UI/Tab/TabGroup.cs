using System.Collections.Generic;
using System.Linq;

using Extensions;

using Template.UI;

using UnityEngine;

namespace TowerDefencePC.Template.UI.Tab
{
    public class TabGroup : UIElement
    {
        private List<TabButton> _buttons;
        [SerializeField] private Sprite _activeButtonImage;
        [SerializeField] private Sprite _inactiveButtonImage;

        public override void Initialize()
        {
            base.Initialize();

            _buttons = GetComponentsInChildren<TabButton>(true).ToList();
            foreach (TabButton button in _buttons.GetEmptyIfNull())
            {
                button.Initialize();
                button.OnClick += (uiElement) => ActivateButton((TabButton)uiElement);
            }
            ActivateButton(_buttons[0]);
        }

        private void ActivateButton(TabButton button)
        {
            ResetGroup();
            button.Image.sprite = _activeButtonImage;
            button.Page?.Show();
        }

        private void DeactivateButton(TabButton button)
        {
            button.Image.sprite = _inactiveButtonImage;
            button.Page?.Hide();
        }

        private void ResetGroup()
        {
            foreach (TabButton button in _buttons.GetEmptyIfNull())
            {
                DeactivateButton(button);
            }
        }
    }
}