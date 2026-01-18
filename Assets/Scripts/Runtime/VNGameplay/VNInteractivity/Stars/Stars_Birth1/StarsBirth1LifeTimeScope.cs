using System;
using VContainer;
using VContainer.Unity;

namespace SosalkasGame.Runtime.VNGameplay.VNInteractivity.Stars.Stars_Birth1
{
    public class StarsBirth1LifeTimeScope : InteractivityLifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<StarsBirth1Presenter>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<StarsBirth1UI>();
        }
    }
}