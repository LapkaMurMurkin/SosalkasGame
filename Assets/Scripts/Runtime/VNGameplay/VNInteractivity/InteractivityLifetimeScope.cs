using System;
using VContainer.Unity;

namespace SosalkasGame.Runtime.VNGameplay.VNInteractivity
{
    public abstract class InteractivityLifetimeScope : LifetimeScope
    {
        public event Action InteractionEnded;

        public void EndInteraction()
        {
            InteractionEnded.Invoke();
        }
    }
}