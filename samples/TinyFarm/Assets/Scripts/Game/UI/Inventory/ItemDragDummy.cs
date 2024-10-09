using Cysharp.Threading.Tasks;
using PamisuKit.Common.Pool;
using PamisuKit.Common.Util;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Inventory
{
    public class ItemDragDummy : MonoEntity, IDragDummy, IPoolElement
    {
        [SerializeField]
        private Image _iconImage;
        
        private MonoPooler _pooler;

        public ItemSlot Slot { get; private set; }
        
        public void SetData(MonoPooler pooler, ItemSlot slot)
        {
            _pooler = pooler;
            Slot = slot;
            _iconImage.LoadSprite(slot.Item.Config.IconRef).Forget();
        }
        
        public void OnEndDrag()
        {
            _pooler.Release(this);
        }

        public void OnSpawnFromPool()
        {
            gameObject.SetActive(true);
        }

        public void OnReleaseToPool()
        {
            gameObject.SetActive(false);
        }
    }
}