namespace PamisuKit.Common.FSM
{
    public class State : IState
    {
        public string StateName { get; set; }
        
        public IStateMachine Machine { get; set; }

        public State()
        {
            // StateName = GetType().ToString();
            StateName = GetType().Name;
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

        public virtual void OnUpdate(float deltaTime)
        {
        }

        public virtual void OnFixedUpdate(float deltaTime)
        {
        }
        
    }
}