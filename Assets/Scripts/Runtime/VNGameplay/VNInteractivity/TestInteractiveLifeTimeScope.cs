using System;
using VContainer;
using VContainer.Unity;

namespace SosalkasGame.Runtime.VNGameplay.VNInteractivity
{
    public class TestInteractiveLifeTimeScope : LifetimeScope
    {
        public Action InteractionEnded;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<TestInteractiveEntryPoint>();
        }
    }
}