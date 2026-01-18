using System;
using VContainer;
using VContainer.Unity;

namespace SosalkasGame.Runtime.VNGameplay.VNInteractivity.Stars.Stars_Birth2
{
    public class StarsBirth2LifeTimeScope : InteractivityLifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<StarsBirth2Presenter>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<StarsBirth2UI>();
        }
    }
}