using System;

namespace Templates.FSM
{
    public abstract class FSMState : IFSMState
    {
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }
    }
}
