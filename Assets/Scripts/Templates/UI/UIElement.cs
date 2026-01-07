using System;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Template.UI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    public class UIElement : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Canvas ParentCanvas { get; private set; }
        public RectTransform RectTransform { get; private set; }
        public CanvasGroup CanvasGroup { get; private set; }

        public virtual Action<UIElement> OnClick { get; set; }
        public virtual Action<UIElement> OnEnter { get; set; }
        public virtual Action<UIElement> OnExit { get; set; }

        public virtual void Initialize()
        {
            ParentCanvas = GetComponentInParent<Canvas>();
            RectTransform = GetComponent<RectTransform>();
            CanvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Show() => this.gameObject?.SetActive(true);
        public virtual void Hide() => this.gameObject?.SetActive(false);

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(this);
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            OnEnter?.Invoke(this);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            OnExit?.Invoke(this);
        }

        protected virtual void OnDestroy()
        {
            
        }
    }
}