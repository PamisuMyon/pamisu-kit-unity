namespace Pamisu.Commons.FSM
{
    public interface IState
    {
        string StateName { get; set; }
        
        IStateMachine Machine { get; set; }

        void OnAddToMachine();

        void OnEnter();

        void OnExit();

        void OnProcess();

        void OnPhysicsProcess();
        
    }
}