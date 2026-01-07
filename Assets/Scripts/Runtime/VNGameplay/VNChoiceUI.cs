using System;
using System.Collections.Generic;
using Template.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SosalkasGame.Runtime.VNGameplay
{
    public class VNChoiceUI : UIElement
    {
        public VNChioceOptionUI OptionTemplate;
        public VerticalLayoutGroup OptionsPanel;
        public List<VNChioceOptionUI> Options;

        public Action<int> OptionSelected;

        public override void Initialize()
        {
            base.Initialize();
            OptionTemplate.Hide();
            this.Hide();
            Options = new List<VNChioceOptionUI>();
        }

        public void AddOption(string option)
        {
            VNChioceOptionUI newOption = Instantiate(OptionTemplate, OptionsPanel.transform);
            newOption.Initialize(option);
            newOption.OnClick += InvokeSelection;
            Options.Add(newOption);
            newOption.Show();
        }

        private void InvokeSelection(UIElement element)
        {
            int index = Options.FindIndex(option => option == element);
            OptionSelected.Invoke(index);
            this.Hide();
            ClearOptions();
        }

        public void AddOptions(string[][] options)
        {
            foreach (string[] option in options)
                AddOption(option[1]);

            this.Show();
        }

        public void ClearOptions()
        {
            foreach (VNChioceOptionUI option in Options)
                Destroy(option.gameObject);
            Options.Clear();
        }


    }
}