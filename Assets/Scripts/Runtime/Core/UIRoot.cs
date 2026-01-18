using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SosalkasGame.Runtime.Core
{
    public class UIRoot : MonoBehaviour
    {
        [field: SerializeField] public GraphicRaycaster GraphicRaycaster { get; private set;}
        [field: SerializeField] public EventSystem EventSystem { get; private set;}

        [field: SerializeField] public GameObject Screens { get; private set;}
        [field: SerializeField] public GameObject PopUps { get; private set;}
        [SerializeField] private GameObject _loadingScreen;

        public void AddScreen(GameObject screen, bool enabled = true)
        {
            screen.transform.SetParent(Screens.transform, false);
            screen.gameObject.SetActive(enabled);
        }

        public void ClearScreens()
        {
            if (Screens)
                foreach (Transform transform in Screens.transform)
                    Destroy(transform.gameObject);
        }

        public void AddPopUp(GameObject popUp, bool enabled = true)
        {
            popUp.transform.SetParent(PopUps.transform, false);
            popUp.gameObject.SetActive(enabled);
        }

        public void ClearPopUps()
        {
            if (PopUps)
                foreach (Transform transform in PopUps.transform)
                    Destroy(transform.gameObject);
        }

        public void ShowLoadingScreen()
        {
            _loadingScreen.SetActive(true);
        }

        public void HideLoadingScreen()
        {
            _loadingScreen.SetActive(false);
        }
    }
}