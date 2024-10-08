using Game.UI.Inventory;
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

        protected override void OnCreate()
        {
            base.OnCreate();
            
            // TODO TEMP
            for (int i = 0; i < _containers.Length; i++)
            {
                _containers[i].Setup(Region);
            }
        }
        
    }
}