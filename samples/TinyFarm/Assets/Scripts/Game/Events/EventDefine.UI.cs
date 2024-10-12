namespace Game.Events
{
    public enum ToggleState
    {
        Inverse,
        On,
        Off
    }
    
    public struct ReqToggleShopView
    {
        public ToggleState NewState;
    }

    public struct ReqToggleInventoryView
    {
        public ToggleState NewState;
    }
    
}