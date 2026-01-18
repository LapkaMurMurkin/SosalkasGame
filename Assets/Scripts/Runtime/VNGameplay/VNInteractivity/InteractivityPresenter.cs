using VContainer;

namespace SosalkasGame.Runtime.VNGameplay.VNInteractivity
{
    public abstract class InteractivityPresenter
    {
        private InteractivityLifetimeScope _lifetimeScope;

        public InteractivityPresenter(IObjectResolver resolver)
        {
            _lifetimeScope = (InteractivityLifetimeScope)resolver.ApplicationOrigin;
        }

        public void EndInteraction() => _lifetimeScope.EndInteraction();
    }
}