using UnityEngine;

namespace Game.UI
{
    public interface IDragDummy
    {
        Transform Trans { get; }

        void OnBeginDrag();
        
        void OnEndDrag();
    }
}