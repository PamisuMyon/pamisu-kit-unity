namespace Pamisu.Commons.FSM
{
    public class State : IState
    {
        public string StateName { get; set; }
        
        public IStateMachine StateMachine { get; set; }

        public State()
        {
            StateName = GetType().ToString();
        }
        
        public virtual void OnAddToMachine()
        {
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnLeave()
        {
        }

        public virtual void OnProcess()
        {
        }

        public virtual void OnPhysicsProcess()
        {
        }
        
    }
}