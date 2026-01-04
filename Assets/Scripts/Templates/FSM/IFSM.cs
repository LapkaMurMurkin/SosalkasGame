namespace Templates.FSM
{
    public interface IFSM
    {
        public abstract void InitializeState(IFSMState state);

        public abstract void SwitchStateTo<T>() where T : IFSMState;

        public abstract void Update();
    }
}
