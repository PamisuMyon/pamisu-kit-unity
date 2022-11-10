namespace Pamisu.Commons.FSM
{
    public interface IState
    {
        string StateName { get; set; }
        
        IStateMachine StateMachine { get; set; }

        void OnAddToMachine();

        void OnEnter();

        void OnLeave();

        void OnProcess();

        void OnPhysicsProcess();
        
    }
}