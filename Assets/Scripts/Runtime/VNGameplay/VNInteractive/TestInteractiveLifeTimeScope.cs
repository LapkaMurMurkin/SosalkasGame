using System;
using VContainer;
using VContainer.Unity;

namespace SosalkasGame.Runtime.VNGameplay.VNInteractive
{
    public class TestInteractiveLifeTimeScope : LifetimeScope
    {
        public Action InteractionEnded;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<TestInteractiveEntryPoint>();
            //builder.RegisterEntryPoint<TestInteractiveUI>();

            //Core
            /*             builder.RegisterEntryPoint<GameEntryPoint>();
                        builder.Register<GameStateModel>(Lifetime.Singleton);
                        builder.Register<GameFSM.GameFSM>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
                        /*             builder.Register<MainMenuState>(Lifetime.Singleton);
                                    builder.Register<VNScenarioLoadingState>(Lifetime.Singleton);
                                    builder.Register<VNGameplayState>(Lifetime.Singleton); */


            /*             builder.Register<AssetStorage.AssetStorage>(Lifetime.Singleton);
                        builder.Register<SceneLoader.SceneLoader>(Lifetime.Singleton);
                        builder.Register<ActionMap>(Lifetime.Singleton);

                        builder.RegisterComponentInNewPrefab<UIRoot>(_uiRoot, Lifetime.Singleton).DontDestroyOnLoad();  */
        }
    }
}