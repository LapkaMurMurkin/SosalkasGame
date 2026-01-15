using System;

using Cysharp.Threading.Tasks;

using SosalkasGame.Runtime.Core.GameEntryPoint;

using UnityEngine;
using UnityEngine.SceneManagement;

using VContainer.Unity;

namespace SosalkasGame.Runtime.Core.SceneLoader
{
    public class SceneLoader
    {
        private readonly GameLifetimeScope _gameLifetimeScope;
        public Action LoadStart;
        public Action LoadEnd;

        public SceneLoader(GameLifetimeScope lifetimeScope)
        {
            _gameLifetimeScope = lifetimeScope;
        }

        public async UniTask<Scene> LoadSceneAsync(string sceneName, LifetimeScope reparentLifetimeScope = null)
        {
            LoadStart.Invoke();

            await SceneManager.LoadSceneAsync(SceneID.EMPTY);
            using (LifetimeScope.EnqueueParent(reparentLifetimeScope is null ? _gameLifetimeScope : reparentLifetimeScope))
                await SceneManager.LoadSceneAsync(sceneName);

            LoadEnd.Invoke();
            Debug.Log($"{sceneName} scene loaded");

            return SceneManager.GetSceneByName(sceneName);
        }
    }
}