using Game.UI.Inventory;
using PamisuKit.Common.Pool;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.UI
{
    public class GameUI : MonoEntity
    {
        [SerializeField]
        private RectTransform _windowsPanel;

        [SerializeField]
        private ItemContainer[] _containers;

        [SerializeField]
        private DragHelper _dragHelper;

        private Canvas _canvas;
        
        public Camera UICam;
        public MonoPooler Pooler { get; private set; }

        protected override void OnCreate()
        {
            base.OnCreate();
            _canvas = GetComponent<Canvas>();
            UICam = _canvas.worldCamera;

            Pooler = new MonoPooler(Trans);
            
            _dragHelper.Setup(Region);
            
            // TODO TEMP
            for (int i = 0; i < _containers.Length; i++)
            {
                _containers[i].Setup(Region);
            }
        }
        
    }
}