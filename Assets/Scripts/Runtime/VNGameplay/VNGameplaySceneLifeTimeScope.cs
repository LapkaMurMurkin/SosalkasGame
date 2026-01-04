using VContainer;
using VContainer.Unity;

namespace MyFirstVisualNovel.Runtime.VNGameplay
{
    public class VNGameplaySceneLifeTimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<VNGameplaySceneEntryPoint>();
        }
    }
}