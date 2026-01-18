using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SosalkasGame.Runtime.VNGameplay
{
    public class VNGameplaySceneLifeTimeScope : LifetimeScope
    {
        [SerializeField]
        private VNGameplayUI _vnGameplayUI;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<VNGameplaySceneEntryPoint>();
            builder.RegisterComponentInNewPrefab<VNGameplayUI>(_vnGameplayUI, Lifetime.Singleton);
        }
    }
}