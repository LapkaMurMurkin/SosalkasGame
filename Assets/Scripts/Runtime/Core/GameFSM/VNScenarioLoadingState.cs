using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;

using Extensions;

using MyFirstVisualNovel.Extensions;
using MyFirstVisualNovel.Runtime.Core.UI;
using MyFirstVisualNovel.Runtime.VNGameplay;

using UnityEngine;

namespace MyFirstVisualNovel.Runtime.Core.GameFSM
{
    public class VNScenarioLoadingState : GameFSMState
    {
        private UniTask<bool>.Awaiter _awaiter;

        public VNScenarioLoadingState(GameFSM fsm, GameStateModel model) : base(fsm, model)
        {
        }

        public override void Enter()
        {
            this._model.UIRoot.ShowLoadingScreen();
            _awaiter = LoadScenarioAsync(this._model.ScenarioConfigPath).GetAwaiter();
            Debug.LogWarning($"VNScenarioLoadingState: Enter");
        }

        public override void Exit()
        {
            this._model.UIRoot.HideLoadingScreen();
            Debug.LogWarning($"VNScenarioLoadingState: Exit");
        }

        public override void Update()
        {
            if (_awaiter.IsCompleted)
            {
                bool result = _awaiter.GetResult();
                ConvertScenarioIntoFrames(this._model.ScenarioTable);
                this._fsm.SwitchStateTo<VNReadingState>();
            }
        }

        private async UniTask<bool> LoadScenarioAsync(string scenarioConfigPath)
        {
            string variablesPath;
            string scenarioPath;
            string[] config = File.ReadAllLines(scenarioConfigPath);

            foreach (string line in config)
            {
                string trimmed = line.Trim();

                if (string.IsNullOrEmpty(trimmed))
                    continue; // пустая строка

                if (trimmed.StartsWith("//"))
                    continue; // комментарий

                // Пример: "Scenario https://..."
                if (trimmed.StartsWith("Variables ", System.StringComparison.OrdinalIgnoreCase))
                {
                    variablesPath = trimmed.Substring("Variables ".Length).Trim();
                    this._model.VariablesTable = await TryLoadCSVAsync(variablesPath);
                }
                else if (trimmed.StartsWith("Scenario ", System.StringComparison.OrdinalIgnoreCase))
                {
                    scenarioPath = trimmed.Substring("Scenario ".Length).Trim();
                    this._model.ScenarioTable = await TryLoadCSVAsync(scenarioPath);
                }
            }

            if (this._model.VariablesTable.IsNullOrEmpty() || this._model.ScenarioTable.IsNullOrEmpty())
                return false;

            return true;
        }

        private async UniTask<string[][]> TryLoadCSVAsync(string csvPathOrURL)
        {
            if (csvPathOrURL.StartsWith("http"))
                return await CSVLoader.DownloadGoogleSheetAsync(csvPathOrURL);
            else
                return CSVLoader.LoadCSVFile(csvPathOrURL);
        }

        private void ConvertScenarioIntoFrames(string[][] csv)
        {
            List<VNFrame> frames = new List<VNFrame>();
            Dictionary<string, int> anchors = new Dictionary<string, int>();
            VNFrame newFrame = new VNFrame();

            Dictionary<string, Action<string>> commandHandlers = new Dictionary<string, Action<string>>
            {
                ["BACKGROUND"] = (string value) => newFrame.BackgroundImageID = value,
                ["PORTRET"] = (string value) => newFrame.CharacterImageID = value,
                ["NAME"] = (string value) => newFrame.CharacterNameID = value,
                ["CHOICE"] = (string value) => newFrame.Choice = ParseChoice(value),
                ["ANCHOR"] = (string value) =>
                {
                    newFrame.AnchorID = value;
                    anchors.Add(newFrame.AnchorID, frames.Count); // "frames.Count" it's frame index+1, before adding new frame
                },
                ["JUMP"] = (string value) => newFrame.JumpToAnchorID = value,
                ["INTERACTIVE"] = (string value) => newFrame.Interactive = value.Split("\n"),
                [">"] = (string value) =>
                {
                    newFrame.MainText = value;
                    frames.Add(newFrame.Copy());
                    newFrame.Choice = null;
                    newFrame.JumpToAnchorID = null;
                    newFrame.Interactive = null;
                }
            };

            foreach (string[] row in csv)
            {
                if (row.Length < 3)
                    continue;

                string command = row[0];
                string additional = row[1];
                string value = row[2];

                if (command.IsNullOrEmpty() || value.IsNullOrEmpty())
                {
                    Debug.LogWarning($"Отсутствует команда или контент: {string.Join("|", row)}");
                    continue;
                }

                if (commandHandlers.TryGetValue(command, out var handler))
                    handler(value);
                else
                    Debug.LogWarning($"Неизвестная команда: [{command}]");
            }

            this._model.Frames = frames.ToArray();
            this._model.Anchors = anchors;
            this._model.FramesHistory = new List<int>();
        }

        private string[][] ParseChoice(string choiceContent)
        {
            List<string[]> result = new List<string[]>();
            string[] choiceOptions = choiceContent.Split('\n');
            foreach (string option in choiceOptions)
            {
                if (option.IsNullOrWhitespace())
                    continue;

                int firstSpace = option.IndexOf(' ');
                if (firstSpace == -1)
                {
                    Debug.LogWarning($"Неверный формат выбора: {option}");
                    continue;
                }

                string anchor = option.Substring(0, firstSpace).Trim();
                string text = option.Substring(firstSpace + 1).Trim();
                result.Add(new[] { anchor, text });
            }

            return result.ToArray();
        }
    }
}
