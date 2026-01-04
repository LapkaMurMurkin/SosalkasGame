using System.Collections.Generic;
using System.IO;

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

        public VNScenarioLoadingState(GameFSM fsm, GameStateModel model, UIRoot uiRoot, ActionMap actionMap, AssetStorage.AssetStorage assetStorage) : base(fsm, model, uiRoot, actionMap, assetStorage)
        {
        }

        public override void Enter()
        {
            _uiRoot.ShowLoadingScreen();
            _awaiter = LoadScenarioAsync(this._model.CurrentScenarioConfigPath).GetAwaiter();
            Debug.LogWarning($"VNScenarioLoadingState: Enter");
        }

        public override void Exit()
        {
            _uiRoot.HideLoadingScreen();
            Debug.LogWarning($"VNScenarioLoadingState: Exit");
        }

        public override void Update()
        {
            if (_awaiter.IsCompleted)
            {
                bool result = _awaiter.GetResult();
                this._model.Frames = ConvertScenarioIntoFrames(this._model.ScenarioTable);
                this._fsm.SwitchStateTo<VNGameplayState>();
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

        private VNFrame[] ConvertScenarioIntoFrames(string[][] csv)
        {
            List<VNFrame> frames = new List<VNFrame>();

            string currentBackgroundID = null;
            string currentCharacterImageID = null;
            string currentCharacterName = null;

            foreach (string[] row in csv)
            {
                if (row.Length < 3)
                    continue;

                string command = row[0];
                string additional = row[1];
                string value = row[2];

                if (command.IsNullOrEmpty() || value.IsNullOrEmpty())
                {
                    Debug.LogWarning($"Неизвестная строка: {string.Join("|", row)}");
                    continue;
                }

                switch (command)
                {
                    case "BACKGROUND":
                        currentBackgroundID = value;
                        break;

                    case "PORTRET":
                        currentCharacterImageID = value;
                        break;

                    case "NAME":
                        currentCharacterName = value;
                        break;

                    case ">":
                        frames.Add(new VNFrame
                        {
                            BackgroundImageID = currentBackgroundID,
                            CharacterImageID = currentCharacterImageID,
                            CharacterNameID = currentCharacterName,
                            MainText = value
                        });
                        break;

                    default:
                        Debug.LogWarning($"Неизвестная команда: [{command}]");
                        break;
                }
            }

            return frames.ToArray();
        }
    }
}
