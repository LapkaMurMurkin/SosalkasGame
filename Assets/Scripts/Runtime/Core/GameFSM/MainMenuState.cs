using System.Collections.Generic;
using System.IO;
using System.Linq;
using MyFirstVisualNovel.Runtime.Core.SceneLoader;
using MyFirstVisualNovel.Runtime.Core.UI;
using MyFirstVisualNovel.Runtime.MainMenu;
using Templates.FSM;
using UnityEngine;

namespace MyFirstVisualNovel.Runtime.Core.GameFSM
{
    public class MainMenuState : GameFSMState
    {
        private MainMenuUI _ui;
        private readonly string _scenariosDirectory = Application.dataPath + "/Scenarios";
        private string[] _scenarioPaths;

        public MainMenuState(GameFSM fsm, GameStateModel model) : base(fsm, model)
        {
        }

        public override void Enter()
        {
            _ui = MonoBehaviour.FindFirstObjectByType<MainMenuUI>();
            PopulateScenarioList();
            _ui.StartGameButton.onClick.AddListener(LoadGameplayScene);
        }

        public override void Exit()
        {
            _ui.StartGameButton.onClick.RemoveAllListeners();
        }

        public override void Update() { }

        private async void LoadGameplayScene()
        {
            this._model.ScenarioConfigPath = _scenarioPaths[_ui.ScenarioListDropdown.value];
            await this._model.SceneLoader.LoadScene(SceneID.VN_GAMEPLAY);
        }

        private void PopulateScenarioList()
        {
            if (!Directory.Exists(_scenariosDirectory))
            {
                Debug.LogError($"Папка {_scenariosDirectory} не найдена!");
                return;
            }

            _scenarioPaths = Directory.GetFiles(_scenariosDirectory, "*.cfg"); // например, только CSV

            List<string> options = _scenarioPaths.Select(path => Path.GetFileName(path)).ToList();
            _ui.ScenarioListDropdown.ClearOptions();
            _ui.ScenarioListDropdown.AddOptions(options);
        }
    }
}