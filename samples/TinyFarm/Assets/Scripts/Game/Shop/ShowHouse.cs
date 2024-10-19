using Game.Events;
using Game.Framework;

namespace Game.Shop
{
    public class ShopHouse : Unit
    {
        public override void OnSelected()
        {
            base.OnSelected();
            Emit(new ReqToggleShopView { NewState = ToggleState.On });
        }
    }
}