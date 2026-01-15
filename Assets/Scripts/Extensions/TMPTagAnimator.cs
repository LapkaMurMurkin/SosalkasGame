using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SosalkasGame.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Extensions
{
    public class TMPTagAnimator : IDisposable
    {
        private struct TMPAnimationTag
        {
            public string name;
            public string parameters;
            public int startIndex;
            public int length;
        }

        private static readonly HashSet<string> _tagNames = new HashSet<string>
        {
            "wave",
            "write",
            "redText",
            "blueText"
        };

        // Regex для тегов:
        // - <tag>...</tag>  (содержимое внутри)
        // - <tag>            (одиночный тег)
        private static readonly Regex _tagRegex = new Regex(
            @"<(?<tag>" + string.Join("|", _tagNames) + @")(?<params>[^>]*)>(?<content>.*?)</\k<tag>>|<(?<single>" + string.Join("|", _tagNames) + @")(?<sparams>[^>]*)>",
            RegexOptions.Singleline
        );

        private static Dictionary<string, Func<DOTweenTMPAnimator, TMPAnimationTag, Tween>> _tagAnimation = new()
        {
            {"wave", (anim, tag) => anim.DOWave() },
            {"write", (anim, tag) => anim.DOTypewriter() },
        };

        private TextMeshProUGUI _textElement;
        private List<TMPAnimationTag> _tags;
        private DOTweenTMPAnimator _doTweenTMPAnimator;

        public TMPTagAnimator(TextMeshProUGUI textElement)
        {
            _textElement = textElement;
            _doTweenTMPAnimator = new DOTweenTMPAnimator(_textElement);

            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ReParseTags);

            WaitAndDispose(textElement.GetCancellationTokenOnDestroy()).Forget();
        }

        public void Dispose()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ReParseTags);
            _doTweenTMPAnimator.Dispose();
        }

        private async UniTaskVoid WaitAndDispose(System.Threading.CancellationToken token)
        {
            await UniTask.WaitUntilCanceled(token);
            Dispose();
        }

        private void ReParseTags(UnityEngine.Object obj)
        {
            if (_textElement == null || obj != _textElement)
                return;

            _tags = ParseTags(_textElement.text);                 // парсинг тегов
            ReApplayAnimations();
        }

        public void ReApplayAnimations()
        {
            _doTweenTMPAnimator.KillAll(true);
            foreach (TMPAnimationTag tag in _tags)
                if (_tagAnimation.TryGetValue(tag.name, out var doAnim))
                    doAnim(_doTweenTMPAnimator, tag);
        }


        private List<TMPAnimationTag> ParseTags(string rawText)
        {
            List<TMPAnimationTag> tags = new List<TMPAnimationTag>();
            int cleanTextIndex = 0; // индекс в чистом тексте
            int rawTextIndex = 0;

            MatchCollection matchCollection = _tagRegex.Matches(rawText);
            foreach (var collectionElement in matchCollection.LoopIndex())
            {
                Match match = collectionElement.item;
                int inCollectionIndex = collectionElement.index;

                // Текст между тегами
                cleanTextIndex += match.Index - rawTextIndex;

                if (match.Groups["tag"].Success)
                {
                    // Тег с закрывающим
                    string tagName = match.Groups["tag"].Value.Trim();
                    string content = match.Groups["content"].Value;

                    tags.Add(new TMPAnimationTag
                    {
                        name = tagName,
                        startIndex = cleanTextIndex,
                        length = content.Length
                    });

                    cleanTextIndex += content.Length; // двигаем индекс чистого текста
                }
                else if (match.Groups["single"].Success)
                {
                    // Одиночный тег <tag>
                    string tagName = match.Groups["single"].Value.Trim();

                    // Ищем позицию конца эффекта
                    int tagContentStartIndex = match.Index + match.Length;
                    int tagContentEndIndex = rawText.Length;

                    // если есть следующий тег — до него
                    if (inCollectionIndex + 1 < matchCollection.Count)
                        tagContentEndIndex = matchCollection[inCollectionIndex + 1].Index;

                    int tagLength = tagContentEndIndex - tagContentStartIndex;

                    tags.Add(new TMPAnimationTag
                    {
                        name = tagName,
                        startIndex = cleanTextIndex,
                        length = tagLength
                    });
                }

                rawTextIndex = match.Index + match.Length;
            }

            return tags;
        }
    }
}