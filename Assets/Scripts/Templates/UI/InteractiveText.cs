using System;
using System.Text.RegularExpressions;

using TMPro;

using UnityEngine;

namespace TowerDefencePC.Template.UI
{
    public class InteractiveText : TextMeshProUGUI
    {
        public int CurrentLinkIndex { get; private set; } = -1;
        public Action<string, Vector3> TagEnter;
        public Action<string, Vector3> TagExit;

        public void Initialize()
        {
/*             string pattern = @"\b(" + string.Join("|", StaticData.InteractiveTextTags.Keys) + @")\b";
            MatchCollection matches = Regex.Matches(this.text, pattern);

            this.text = Regex.Replace(this.text, pattern, match =>
            {
                string tag = match.Value;
                string tagName = StaticData.InteractiveTextTags[tag].Split("\n")[0];
                string tagReplace = $"<color=blue><link=\"{tag}\">{tagName}</link></color>";
                return tagReplace;
            }); */
        }

        private void InvokeTagEnter(int newLinkIndex)
        {
            if (CurrentLinkIndex == newLinkIndex)
                return;

            InvokeTagExit();

            if (newLinkIndex >= 0)
            {
                TagEnter?.Invoke(this.textInfo.linkInfo[newLinkIndex].GetLinkID(), Input.mousePosition);
                Debug.Log("Enter");
            }

            CurrentLinkIndex = newLinkIndex;
        }

        private void InvokeTagExit()
        {
            if (CurrentLinkIndex >= 0)
            {
                TagExit?.Invoke(this.textInfo.linkInfo[CurrentLinkIndex].GetLinkID(), Input.mousePosition);
                Debug.Log("Exit");
            }

            CurrentLinkIndex = -1;
        }

        private void Update()
        {
            CheckForTextTagUnderPointer();
        }

        private void CheckForTextTagUnderPointer()
        {
            bool isIntersectingRectTransform = TMP_TextUtilities.IsIntersectingRectTransform(rectTransform, Input.mousePosition, null);
            if (isIntersectingRectTransform is false)
            {
                InvokeTagExit();
                return;
            }

            int linkIndex = TMP_TextUtilities.FindIntersectingLink(this, Input.mousePosition, null);
            if (linkIndex is -1)
            {
                InvokeTagExit();
                return;
            }

            InvokeTagEnter(linkIndex);
        }
    }
}