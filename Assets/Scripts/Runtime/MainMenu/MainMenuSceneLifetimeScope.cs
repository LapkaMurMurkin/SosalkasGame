using SosalkasGame.Runtime.Core;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SosalkasGame.Runtime.MainMenu
{
    public class MainMenuSceneLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private MainMenuUI _mainMenuUI;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<MainMenuSceneEntryPoint>();
            builder.RegisterComponentInNewPrefab<MainMenuUI>(_mainMenuUI, Lifetime.Singleton);
        }
    }
}
