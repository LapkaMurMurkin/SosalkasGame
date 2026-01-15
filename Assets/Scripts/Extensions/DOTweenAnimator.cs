using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Extensions
{
    public static class DOTweenAnimator
    {
        public static Tween FadeInBounce(Transform transform, float duration, float randomDelay = 0)
        {
            transform.localScale = Vector3.zero;
            return transform.DOScale(1, duration)
                .SetEase(Ease.OutBounce)
                .SetDelay(UnityEngine.Random.Range(0, randomDelay))
                .SetLink(transform.gameObject);
        }

        public static Tween ShakePosition(Transform transform, float duration, float strength = 10)
        {
            return transform.DOShakePosition(duration, strength)
                .SetEase(Ease.InBounce)
                .SetLink(transform.gameObject);
        }

        public static Tween FadeInUI(CanvasGroup canvasGroup, float duration = 1)
        {
            return canvasGroup.DOFade(1, duration)
                .SetLink(canvasGroup.gameObject);
        }

        public static Tween FadeInUI(Image image, float duration = 1)
        {
            return image.DOFade(1, duration)
                .SetLink(image.gameObject);
        }

        public static Tween FadeInUI(TextMeshProUGUI text, float duration = 1)
        {
            return text.DOFade(1, duration)
                .SetLink(text.gameObject);
        }
    }
}