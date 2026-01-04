using Extensions;

using MyFirstVisualNovel.Runtime.Core.SceneLoader;

using UnityEditor;
using UnityEditor.SceneManagement;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyFirstVisualNovel.Editor
{
    [InitializeOnLoad]
    public class AutoBoot
    {
        private const string _bootSceneName = SceneID.BOOT;
        private const string _workScenePathPrefsKey = "AutoBootWorkScenePath";

        static AutoBoot()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                // Перед входом в Play Mode
                case PlayModeStateChange.ExitingEditMode:
                    OpenBootScene();
                    break;

                // После выхода из Play Mode
                case PlayModeStateChange.EnteredEditMode:
                    OpenWorkScene();
                    break;
            }
        }

        private static void OpenBootScene()
        {
            Scene currentScene = EditorSceneManager.GetActiveScene();
            if (currentScene.name == _bootSceneName)
                return;

            EditorPrefs.SetString(_workScenePathPrefsKey, currentScene.path);
            string bootScenePath = GetScenePathByName(_bootSceneName);
            if (bootScenePath.IsNullOrWhitespace())
            {
                Debug.LogError($"[AutoBoot] Scene '{_bootSceneName}' not found in Build Settings.");
                return;
            }

            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                Debug.Log($"[AutoBoot] Opening scene '{_bootSceneName}' before entering Play Mode.");
                EditorSceneManager.OpenScene(bootScenePath);
            }
        }

        private static void OpenWorkScene()
        {
            string workScenePath = EditorPrefs.GetString(_workScenePathPrefsKey, string.Empty);
            if (workScenePath.IsNullOrWhitespace())
                return;

            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                Debug.Log($"[AutoBoot] Restoring work scene: {workScenePath}");
                EditorSceneManager.OpenScene(workScenePath);
            }

            EditorPrefs.DeleteKey(_workScenePathPrefsKey);
        }

        private static string GetScenePathByName(string sceneName)
        {
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
                if (scene.enabled && System.IO.Path.GetFileNameWithoutExtension(scene.path) == sceneName)
                    return scene.path;
            return null;
        }
    }
}
