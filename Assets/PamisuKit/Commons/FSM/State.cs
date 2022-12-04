namespace Pamisu.Commons.FSM
{
    public class State : IState
    {
        public string StateName { get; set; }
        
        public IStateMachine Machine { get; set; }

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

        public virtual void OnExit()
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