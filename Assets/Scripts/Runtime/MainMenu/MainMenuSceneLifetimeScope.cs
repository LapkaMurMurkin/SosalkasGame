using VContainer;
using VContainer.Unity;

namespace SosalkasGame.Runtime.MainMenu
{
    public class MainMenuSceneLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<MainMenuSceneEntryPoint>();
        }
    }
}
