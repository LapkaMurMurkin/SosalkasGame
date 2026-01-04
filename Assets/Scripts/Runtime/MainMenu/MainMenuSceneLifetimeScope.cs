using VContainer;
using VContainer.Unity;

namespace MyFirstVisualNovel.Runtime.MainMenu
{
    public class MainMenuSceneLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<MainMenuSceneEntryPoint>();
        }
    }
}
