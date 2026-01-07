using SosalkasGame.Runtime.VNGameplay;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace MyFirstVisualNovel.Runtime.VNGameplay
{
    public class VNGameplayUI : MonoBehaviour
    {
        public RawImage BackgroundImage;
        public RawImage CharacterImage;
        public TextMeshProUGUI CharacterName;
        public TextMeshProUGUI MainText;
        public TMP_FontAsset CurrentFont;

        public VNChoiceUI ChoiceMenu;

        public void Initialize()
        {
            MainText.font = CurrentFont;
            ChoiceMenu.Initialize();
        }
    }
}