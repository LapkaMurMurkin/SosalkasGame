using System;
using SosalkasGame.Templates.UI;
using Template.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace SosalkasGame.Runtime.MainMenu
{
    public class SettingsWindowUI : UIWindow
    {
        public TextMeshProUGUI FontSizeValue;
        public TextMeshProUGUI FontPreview;
        public Slider FontSizeSlider;

        public override void Initialize()
        {
            base.Initialize();
            FontSizeSlider.onValueChanged.AddListener(UpdateFontPreview);
            UpdateFontPreview(FontSizeSlider.value);
        }

        private void UpdateFontPreview(float fontSize)
        {
            FontPreview.fontSize = fontSize;
            FontSizeValue.text = fontSize.ToString();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            FontSizeSlider.onValueChanged.RemoveAllListeners();
        }
    }
}