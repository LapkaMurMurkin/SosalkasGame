using SosalkasGame.Runtime.Core.GameFSM;
using SosalkasGame.Runtime.Core;

using UnityEngine;

using VContainer;
using VContainer.Unity;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SosalkasGame.Runtime.Core.GameEntryPoint
{
    public class GameLifetimeScope : LifetimeScope
    {
        /*         [SerializeField]
                private UIRoot _uiRoot; */

        protected override void Configure(IContainerBuilder builder)
        {
            //GameObject.DontDestroyOnLoad(this.gameObject);

            //Core
            builder.RegisterEntryPoint<GameEntryPoint>();
            builder.Register<GameStateModel>(Lifetime.Singleton);
            builder.Register<GameFSM.GameFSM>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

            builder.Register<AssetStorage.AssetStorage>(Lifetime.Singleton);
            builder.Register<SceneLoader.SceneLoader>(Lifetime.Singleton);
            builder.Register<ActionMap>(Lifetime.Singleton);

            builder.RegisterComponentInHierarchy<UIRoot>();
            builder.RegisterComponentInHierarchy<GraphicRaycaster>();
            builder.RegisterComponentInHierarchy<EventSystem>();
        }
    }
}


