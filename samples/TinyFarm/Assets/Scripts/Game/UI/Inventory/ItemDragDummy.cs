using PamisuKit.Common.Pool;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Inventory
{
    public class ItemDragDummy : MonoEntity, IDragDummy
    {
        [SerializeField]
        private Image _iconImage;
        
        private MonoPooler _pooler;

        public ItemSlot Slot { get; private set; }
        
        public void SetData(MonoPooler pooler, ItemSlot slot)
        {
            _pooler = pooler;
            Slot = slot;
        }
        
        public void OnEndDrag()
        {
            _pooler.Release(this);
        }
    }
}