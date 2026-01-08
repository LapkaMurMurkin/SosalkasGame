using MyFirstVisualNovel.Runtime.Core.GameFSM;
using MyFirstVisualNovel.Runtime.Core.UI;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace MyFirstVisualNovel.Runtime.Core.GameEntryPoint
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] UIRoot _uiRoot;

        protected override void Configure(IContainerBuilder builder)
        {
            GameObject.DontDestroyOnLoad(this.gameObject);

            //Core
            builder.RegisterEntryPoint<GameEntryPoint>();
            builder.Register<GameStateModel>(Lifetime.Singleton);
            builder.Register<GameFSM.GameFSM>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            /*             builder.Register<MainMenuState>(Lifetime.Singleton);
                        builder.Register<VNScenarioLoadingState>(Lifetime.Singleton);
                        builder.Register<VNGameplayState>(Lifetime.Singleton); */


            builder.Register<AssetStorage.AssetStorage>(Lifetime.Singleton);
            builder.Register<SceneLoader.SceneLoader>(Lifetime.Singleton);
            builder.Register<ActionMap>(Lifetime.Singleton);

            builder.RegisterComponentInNewPrefab<UIRoot>(_uiRoot, Lifetime.Singleton).DontDestroyOnLoad();
        }
    }
}


