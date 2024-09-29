using PamisuKit.Framework;

namespace Game.Framework
{
    public enum IndicatorState
    {
        None,
        Hover,
        Selected,
    }
    
    public class Indicator : MonoEntity
    {
        public Unit AttachedUnit;
        
        public IndicatorState State { get; private set; }

        public void Hover(Unit unit)
        {
            
        }

        public void Attach(Unit unit)
        {
            
        }
        
        public void Detach()
        {
            
        }
    }
}