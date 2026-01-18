using System;
using Template.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace SosalkasGame.Runtime.VNGameplay.VNInteractivity
{
    public abstract class InteractivityUI : UIElement, IDisposable
    {
        protected GraphicRaycaster _graphicRaycaster;
        protected EventSystem _eventSystem;
        protected VNGameplayUI _vnGameplayUI;

        [Inject]
        public void Initialize(GraphicRaycaster graphicRaycaster, EventSystem eventSystem, VNGameplayUI vnGameplayUI)
        {
            this.Initialize();
            _graphicRaycaster = graphicRaycaster;
            _eventSystem = eventSystem;
            _vnGameplayUI = vnGameplayUI;

            this.transform.SetParent(_vnGameplayUI.InteractivityLayer.transform, false);
        }

        public virtual void Dispose()
        {
            Destroy(this.gameObject);
        }
    }
}