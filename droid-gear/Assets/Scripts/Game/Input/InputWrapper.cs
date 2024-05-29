using PamisuKit.Framework;

namespace Game.Input
{
    public class InputWrapper : System<InputWrapper>
    {
        
        private GameInputActions _actions;
        
        public static GameInputActions Actions => Instance?._actions;

        public void Init()
        {
            _actions = new GameInputActions();
            _actions.Combat.Enable();
        }

    }
}