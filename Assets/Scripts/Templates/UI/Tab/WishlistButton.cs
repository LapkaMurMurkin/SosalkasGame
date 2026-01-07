using UnityEngine;
using UnityEngine.UI;

namespace TowerDefencePC.Template.UI.Tab
{
    public class WishlistButton : Button
    {
        protected override void Awake()
        {
            base.Awake();
            this.onClick.AddListener(OpenSteamGamePage);
        }

        protected override void OnDestroy()
        {
            this.onClick.RemoveAllListeners();
            base.OnDestroy();
        }

        public void OpenSteamGamePage()
        {
            //Debug.Log("asd");
/*             if (SteamManager.Initialized)
                Application.OpenURL("steam://openurl/https://store.steampowered.com/app/3887080/Stufft_TD/");
            else
                Application.OpenURL("https://store.steampowered.com/app/3887080/Stufft_TD/"); */
        }
    }
}