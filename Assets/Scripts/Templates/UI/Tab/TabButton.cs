using Template.UI;

using UnityEngine;
using UnityEngine.UI;

namespace TowerDefencePC.Template.UI.Tab
{
    [RequireComponent(typeof(Image))]
    public class TabButton : UIElement
    {
        public Image Image { get; private set; }
        [field: SerializeField] public UIElement Page { get; private set; }

        public override void Initialize()
        {
            base.Initialize();

            Image = GetComponent<Image>();
            Page?.Initialize();
        }
    }
}