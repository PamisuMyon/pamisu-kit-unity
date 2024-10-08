using UnityEngine;

namespace Game.UI
{
    public interface IDragDummy
    {
        Transform Trans { get; }

        void OnEndDrag();
    }
}