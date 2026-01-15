using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Extensions;
using TMPro;
using UnityEngine;

namespace SosalkasGame.Extensions
{
    public class DOTweenTMPAnimator : IDisposable
    {
        private readonly TextMeshProUGUI _textElement;
        private TMP_TextInfo _textInfo;
        private Vector3[][] _verticesOrig;
        private int _visibleCharacters;

        private bool _isVerticesDirty;
        private bool _isColorsDirty;

        private List<Tween> _tweens;

        private readonly struct CharContext
        {
            public readonly int MaterialIndex;
            public readonly int VertexIndex;

            public CharContext(int materialIndex, int vertexIndex)
            {
                MaterialIndex = materialIndex;
                VertexIndex = vertexIndex;
            }
        }

        public DOTweenTMPAnimator(TextMeshProUGUI textElement)
        {
            _textElement = textElement;
            _tweens = new List<Tween>();
            _textElement.OnPreRenderText += UpdateTMPMeshData;
            Canvas.willRenderCanvases += ApplyChanges;
        }

        public void Dispose()
        {
            Canvas.willRenderCanvases -= ApplyChanges;
            _textElement.OnPreRenderText -= UpdateTMPMeshData;
            KillAll();
        }

        private void UpdateTMPMeshData(TMP_TextInfo info)
        {
            if (_textElement == null) return;

            _textInfo = _textElement.textInfo;
            CacheOriginalVertices();
        }

        private void ApplyChanges()
        {
            if (_textElement == null || _verticesOrig.IsNullOrEmpty()) return;

            if (_isVerticesDirty)
                _textElement.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);

            if (_isColorsDirty)
                _textElement.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            _isVerticesDirty = false;
            _isColorsDirty = false;
        }

        private void CacheOriginalVertices()
        {
            TMP_MeshInfo[] meshInfoCopy = _textInfo.CopyMeshInfoVertexData();
            _verticesOrig = new Vector3[meshInfoCopy.Length][];

            for (int i = 0; i < _verticesOrig.Length; i++)
                _verticesOrig[i] = meshInfoCopy[i].vertices;
        }

        #region Tween Operations

        private T RegisterTween<T>(T tween) where T : Tween
        {
            _tweens.Add(tween);
            // Когда твин убивается (завершился или вызвали Kill вручную)
            tween.OnKill(() =>
            {
                _tweens.Remove(tween);
            });

            return tween;
        }

        public void KillAll(bool complete = false)
        {
            // Делаем копию списка, так как OnKill будет модифицировать оригинал при удалении
            List<Tween> activeTweens = _tweens.ToList();
            foreach (Tween tween in activeTweens)
                if (tween != null && tween.IsActive())
                    tween.Kill(complete);

            _tweens.Clear();
        }

        public void PauseAll() => _tweens.ForEach(t => t.Pause());
        public void ResumeAll() => _tweens.ForEach(t => t.Play());

        #endregion


        #region Single Char Setter Getter

        private bool TryGetCharInfo(int charIndex, out CharContext context)
        {
            context = default;

            if (_textInfo == null || charIndex < 0 || charIndex >= _textInfo.characterCount)
                return false;

            TMP_CharacterInfo info = _textInfo.characterInfo[charIndex];
            if (info.isVisible is false)
                return false;

            context = new CharContext(info.materialReferenceIndex, info.vertexIndex);
            return true;
        }

        public void SetCharOffset(int charIndex, Vector3 offset)
        {
            if (TryGetCharInfo(charIndex, out var ctx) is false)
                return;

            Vector3[] vertices = _textInfo.meshInfo[ctx.MaterialIndex].vertices;
            Vector3[] verticesOrig = _verticesOrig[ctx.MaterialIndex];

            for (int i = 0; i < 4; i++)
                vertices[ctx.VertexIndex + i] = verticesOrig[ctx.VertexIndex + i] + offset;

            _isVerticesDirty = true;
        }

        public Color GetCharColor(int charIndex)
        {
            if (TryGetCharInfo(charIndex, out var ctx) is false)
                return default;

            Color32[] colors = _textInfo.meshInfo[ctx.MaterialIndex].colors32;

            return colors[ctx.VertexIndex];
        }

        public void SetCharColor(int charIndex, Color color)
        {
            if (TryGetCharInfo(charIndex, out var ctx) is false)
                return;

            Color32[] colors = _textInfo.meshInfo[ctx.MaterialIndex].colors32;

            for (int i = 0; i < 4; i++)
                colors[ctx.VertexIndex + i] = color;

            _isColorsDirty = true;
        }

        #endregion

        #region Single Char operation

        public Tween DOOffsetChar(int charIndex, Vector3 offset, float duration)
        {
            Vector3 start = Vector3.zero;
            Vector3 end = offset;

            return DOTween.To(() => start, value =>
            {
                start = value;
                SetCharOffset(charIndex, value);
            }, end, duration)
            .SetLink(_textElement.gameObject);
        }

        #endregion

        #region Full text animations

        public Tween DOWave(float amplitude = 10f, float wavelength = 0.5f, float speed = 2f)
        {
            return RegisterTween(DOTween.To(() => 0, _ =>
            {
                for (int i = 0; i < _textInfo.characterCount; i++)
                {
                    Vector3 offset = new Vector3(0, Mathf.Sin(Time.time * speed + i * wavelength) * amplitude, 0);
                    SetCharOffset(i, offset); // метод, который обновляет вершины буквы
                }

            }, 0, 0)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart)
                .SetLink(_textElement.gameObject));
        }

        public Tween DOTypewriter()
        {
            _visibleCharacters = 0;
            //int characterCount = _textInfo.characterCount;
            return RegisterTween(DOTween.To(() => _visibleCharacters,
                value =>
                {
                    _visibleCharacters = value;
                    for (int i = 0; i < _textInfo.characterCount; i++)
                    {
                        Color color = GetCharColor(i);
                        color.a = i < _visibleCharacters ? 1 : 0;
                        SetCharColor(i, color);
                    }
                },
                _textInfo.characterCount,
                10
            )
            .SetEase(Ease.Linear)
            .SetLink(_textElement.gameObject));
        }

        //Никогда, бля, не используй ебучий -> "_textElement.maxVisibleCharacters" эта говнина пересобирает меш текста и обнуляет вообще все изменения
        /*         public Tween DOTypewriter()
                {
                    _textElement.maxVisibleCharacters = 3;
                    return DOTween.To(() => _textElement.maxVisibleCharacters,
                        x => _textElement.maxVisibleCharacters = x,
                        _textInfo.characterCount,
                        10
                    )
                    .SetEase(Ease.Linear)
                    .SetLink(_textElement.gameObject);
                } */

        #endregion
    }
}