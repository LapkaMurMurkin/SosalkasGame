namespace Templates.FSM
{
    public interface IFSMState
    {
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }
    }
}
