namespace PamisuKit.Common.FSM
{
    public interface IState
    {
        string StateName { get; set; }
        
        IStateMachine Machine { get; set; }

        void OnAddToMachine();

        void OnEnter();

        void OnExit();

        void OnUpdate(float deltaTime);

        void OnFixedUpdate(float deltaTime);

        void OnLateUpdate(float deltaTime);

    }
}