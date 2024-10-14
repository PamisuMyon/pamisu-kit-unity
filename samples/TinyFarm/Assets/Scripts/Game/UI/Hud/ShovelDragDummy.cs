using Game.Events;
using Game.Framework;
using PamisuKit.Framework;

namespace Game.UI.Hud
{
    public class ShovelDragDummy : MonoEntity, IDragDummy
    {
        public void OnBeginDrag()
        {
            Go.SetActive(true);
            Emit(new ReqChangePlayerControlState
            {
                NewState = PlayerControlState.Shovel
            });
        }

        public void OnEndDrag()
        {
            Emit(new ReqPlayerControlStateReset());
            Go.SetActive(false);
        }
        
    }
}