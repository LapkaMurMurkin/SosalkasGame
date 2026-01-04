using System;

using Cysharp.Threading.Tasks;

using MyFirstVisualNovel.Runtime.Core.GameEntryPoint;

using UnityEngine;
using UnityEngine.SceneManagement;

using VContainer.Unity;

namespace MyFirstVisualNovel.Runtime.Core.SceneLoader
{
    public class SceneLoader
    {
        private GameLifetimeScope _gameLifetimeScope;
        public Action LoadStart;
        public Action LoadEnd;

        public SceneLoader(GameLifetimeScope lifetimeScope)
        {
            _gameLifetimeScope = lifetimeScope;
        }

        public async UniTask<Scene> LoadScene(string sceneName, LifetimeScope parentLifetimeScope = null)
        {
            LoadStart.Invoke();

            await SceneManager.LoadSceneAsync(SceneID.EMPTY);
            using (LifetimeScope.EnqueueParent(parentLifetimeScope is null ? _gameLifetimeScope : parentLifetimeScope))
                await SceneManager.LoadSceneAsync(sceneName);

            LoadEnd.Invoke();
            Debug.Log($"{sceneName} scene loaded");

            return SceneManager.GetSceneByName(sceneName);
        }
    }
}