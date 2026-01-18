using System;
using System.Collections.Generic;
using System.Linq;
using Alchemy.Serialization;
using Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

namespace SosalkasGame.Runtime.VNGameplay.VNInteractivity.Stars.Stars_Birth2
{
    [AlchemySerialize]
    public partial class StarsBirth2UI : InteractivityUI
    {
        private ActionMap _actionMap;
        private StarsBirth2Presenter _presenter;

        [SerializeField]
        private List<Image> _stars;
        [SerializeField]
        private LineRenderer _starPath;

        private Dictionary<Image, List<Image>> _correctAnswer;

        [Inject]
        public void Initialize(StarsBirth2Presenter presenter, ActionMap actionMap)
        {
            _presenter = presenter;
            _actionMap = actionMap;

            _correctAnswer = _presenter.BuildConnections(_stars, _starPath);

            _actionMap.MainInput.LeftClick.performed += AddStarToResultPath;
            _actionMap.MainInput.RightClick.performed += RemoveLastStarFromResultPath;
        }

        public override void Dispose()
        {
            _actionMap.MainInput.LeftClick.performed -= AddStarToResultPath;
            _actionMap.MainInput.RightClick.performed -= RemoveLastStarFromResultPath;

            base.Dispose();
        }

        private void Update()
        {
            DrawStarPath();
            DrawStarPathCursor();
        }

        private void AddStarToResultPath(InputAction.CallbackContext context)
        {
            Image star = TryGetStarUnderCursor();
            if (star is null)
                return;

            _presenter.AddStarToResultPath(star);
            //AddPointToLineRenderer(_starPath, star.rectTransform.position);
            if (IsCurrentAnswerCorrect(_correctAnswer, _presenter.ResultPathConnections))
                Debug.Log("End");
        }

        private void RemoveLastStarFromResultPath(InputAction.CallbackContext context)
        {
            _presenter.RemoveLastStarFromResultPath();
            //RemoveLastPointFromLineRenderer(_starPath);
        }

        private Image TryGetStarUnderCursor()
        {
            Vector2 mousePos = _actionMap.MainInput.MousePosition.ReadValue<Vector2>();

            PointerEventData eventData = new PointerEventData(_eventSystem);
            eventData.position = mousePos;

            List<RaycastResult> results = new();
            _graphicRaycaster.Raycast(eventData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.TryGetComponent<Image>(out Image image) is false)
                    continue;

                if (_stars.Contains(image) is false)
                    continue;

                return image;
            }

            return null;
        }

        private void DrawStarPath()
        {
            if (_starPath.positionCount == _presenter.ResultPath.Count + 1)
                return;

            _starPath.positionCount = _presenter.ResultPath.Count + 1;
            _starPath.SetPositions(_presenter.ResultPath.Select(image => image.rectTransform.localPosition).Reverse().ToArray());
        }

        private void DrawStarPathCursor()
        {
            if (_starPath.positionCount <= 1)
                return;

            Vector2 mouseScreenPos = _actionMap.MainInput.MousePosition.ReadValue<Vector2>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(this.RectTransform, mouseScreenPos, Camera.main, out Vector2 localPoint);
            _starPath.SetPosition(_starPath.positionCount - 1, localPoint);
        }

        private bool IsCurrentAnswerCorrect(Dictionary<Image, List<Image>> correctAnswer, Dictionary<Image, List<Image>> currentAnswer)
        {
            if (correctAnswer.Count != currentAnswer.Count)
                return false;

            foreach (Image star in correctAnswer.Keys)
            {
                if (!currentAnswer.TryGetValue(star, out List<Image> connections))
                    return false;

                HashSet<Image> aSet = new(correctAnswer[star]);
                HashSet<Image> bSet = new(connections);

                if (!aSet.SetEquals(bSet))
                    return false;
            }

            return true;
        }
    }
}