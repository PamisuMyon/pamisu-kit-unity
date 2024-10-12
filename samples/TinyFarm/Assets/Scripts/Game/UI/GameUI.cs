using Game.UI.Hud;
using Game.UI.Inventory;
using Game.UI.Shop;
using PamisuKit.Common.Pool;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.UI
{
    public class GameUI : MonoEntity
    {
        [SerializeField]
        private ClickDragHelper _dragHelper;
        
        [SerializeField]
        private HudView _hud;
        
        [SerializeField]
        private ShopView _shopView;

        [SerializeField]
        private InventoryView _inventoryView;

        private Canvas _canvas;
        
        public Camera UICam;
        public MonoPooler Pooler { get; private set; }

        protected override void OnCreate()
        {
            base.OnCreate();
            _canvas = GetComponent<Canvas>();
            if (UICam == null)
                UICam = _canvas.worldCamera;
            Pooler = new MonoPooler(Trans);

            RegisterService(this);
            
            _dragHelper.Setup(Region);
            _hud.Setup(Region);
            
            _inventoryView.Setup(Region);
            _shopView.Setup(Region);
        }

        protected override void OnSelfDestroy()
        {
            base.OnSelfDestroy();
            RemoveService(this);
        }
        
    }
}