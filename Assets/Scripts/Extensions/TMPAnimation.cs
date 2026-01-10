using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Extensions
{
    public class TMPAnimation
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

        private static Dictionary<string, Action<TMPAnimation, TMPAnimationTag>> _tagAnimation = new Dictionary<string, Action<TMPAnimation, TMPAnimationTag>>
        {
            {"wave", (anim, tag) => anim.AnimateWave(tag) },
            {"write", (anim, tag) => anim.AnimateWrite(tag) },
        };

        private TMP_Text _textMesh;
        private TMP_TextInfo _textInfo;
        private TMPAnimationTag[] _tags;
        private Mesh _meshCopy;
        private Vector3[] _verticesOrig;
        private Vector3[] _verticesCopy;

        private float _writerProgress;

        public TMPAnimation(TMP_Text tmpText)
        {
            _textMesh = tmpText;
            ForceTMPMeshUpdate();
        }

        public void ForceTMPMeshUpdate()
        {
            string rawText = _textMesh.text;
            _tags = ParseTags(rawText).ToArray();
            _textMesh.text = _tagRegex.Replace(rawText, match => match.Groups["content"].Value);

            _textMesh.ForceMeshUpdate();

            _textInfo = _textMesh.textInfo;
            _meshCopy = _textMesh.mesh;

            _verticesOrig = _meshCopy.vertices;
            _verticesCopy = new Vector3[_verticesOrig.Length];

            _writerProgress = 0;
        }

        public void Update()
        {
            _verticesOrig.CopyTo(_verticesCopy, 0);

            foreach (TMPAnimationTag tag in _tags)
                if (_tagAnimation.TryGetValue(tag.name, out var anim))
                    anim(this, tag);

            _meshCopy.vertices = _verticesCopy;
            _textMesh.canvasRenderer.SetMesh(_meshCopy);
        }

        private void AnimateWave(TMPAnimationTag tag)
        {
            float waveHeight = 10f;
            float waveSpeed = 2f;
            float waveLength = 0.5f;

            for (int i = tag.startIndex; i < tag.startIndex + tag.length; i++)
            {
                if (_textInfo.characterInfo[i].isVisible == false)
                    continue;
                int vertexIndex = _textInfo.characterInfo[i].vertexIndex;

                Vector3 offset = Vector3.up * Mathf.Sin(Time.time * waveSpeed + i * waveLength) * waveHeight;

                for (int j = 0; j < 4; j++)
                    _verticesCopy[vertexIndex + j] += offset;
                /*                 _verticesCopy[vertexIndex + 1] += offset;
                                _verticesCopy[vertexIndex + 2] += offset;
                                _verticesCopy[vertexIndex + 3] += offset; */
            }
        }

        private void AnimateWrite(TMPAnimationTag tag)
        {
            float writerSpeed = 10f;

            if (_writerProgress >= _textMesh.text.Length)
                return;

            _writerProgress += Time.deltaTime * writerSpeed;
            int visibleCount = (int)_writerProgress;

            for (int i = 0; i < _textMesh.text.Length; i++)
            {
                if (_textInfo.characterInfo[i].isVisible == false)
                    continue;
                int vertexIndex = _textInfo.characterInfo[i].vertexIndex;

                Color32[] colors = _meshCopy.colors32;
                byte alpha = (i < visibleCount) ? (byte)255 : (byte)0;

                for (int j = 0; j < 4; j++)
                    colors[vertexIndex + j].a = alpha;

                _meshCopy.colors32 = colors;
            }
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