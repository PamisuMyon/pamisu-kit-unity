using Cysharp.Threading.Tasks;
using PamisuKit.Common.Pool;
using PamisuKit.Common.Util;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.UI.Inventory
{
    public class ItemDragDummy : MonoEntity, IDragDummy, IPoolElement
    {
        [FormerlySerializedAs("_iconImage")]
        [SerializeField]
        protected Image IconImage;
        
        protected MonoPooler Pooler;

        public ItemSlot Slot { get; private set; }
        
        public void SetData(MonoPooler pooler, ItemSlot slot)
        {
            Pooler = pooler;
            Slot = slot;
            IconImage.LoadSprite(slot.Item.Config.IconRef).Forget();
        }

        public virtual void OnBeginDrag()
        {
        }

        public virtual void OnEndDrag()
        {
            Pooler.Release(this);
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