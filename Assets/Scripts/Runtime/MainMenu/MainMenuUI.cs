using SosalkasGame.Runtime.MainMenu;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace SosalkasGame.Runtime.MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        public Button SettingsButton;
        public Button StartGameButton;
        public TMP_Dropdown ScenarioListDropdown;
        public SettingsWindowUI SettingsWindowUI;

        public void Initialize()
        {
            SettingsWindowUI.Initialize();
            SettingsButton.onClick.AddListener(SettingsWindowUI.Show);
        }

        protected void OnDestroy()
        {
            SettingsButton.onClick.RemoveAllListeners();
        }
    }
}