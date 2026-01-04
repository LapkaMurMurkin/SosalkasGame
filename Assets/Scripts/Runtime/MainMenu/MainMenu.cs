using System.Collections.Generic;
using System.IO;
using System.Linq;

using MyFirstVisualNovel.Runtime.Core.GameFSM;
using MyFirstVisualNovel.Runtime.Core.SceneLoader;

using UnityEngine;

namespace MyFirstVisualNovel.Runtime.MainMenu
{
    public class MainMenu
    {
        private MainMenuUI _ui;
        private SceneLoader _sceneLoader;
        private GameFSM _gameState;
        private readonly string _scenariosDirectory = Application.dataPath + "/Scenarios";
        private string[] _scenarioPaths;

        public MainMenu(MainMenuUI mainMenuUI, SceneLoader sceneLoader, GameFSM gameState)
        {
            _ui = mainMenuUI;
            _sceneLoader = sceneLoader;
            _gameState = gameState;
        }

        public void Initialize()
        {
            PopulateScenarioList();
            _ui.StartGameButton.onClick.AddListener(LoadGameplayScene);
        }

        public void Dispose()
        {
            _ui.StartGameButton.onClick.RemoveAllListeners();
        }

        private async void LoadGameplayScene()
        {
            _gameState.Model.CurrentScenarioConfigPath = _scenarioPaths[_ui.ScenarioListDropdown.value];
            await _sceneLoader.LoadScene(SceneID.VN_GAMEPLAY);
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
