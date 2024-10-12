using Game.UI.Inventory;
using PamisuKit.Common.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Hud
{
    public class PlantBar : ItemContainer
    {
        [Header("PlantBar")]
        [SerializeField]
        private Button _closeButton;

        public ToggleableView Toggleable;

        protected override void OnCreate()
        {
            base.OnCreate();
            if (Toggleable == null)
                Toggleable = GetComponent<ToggleableView>();
            Toggleable.Setup(Region);
            
            _closeButton.SetOnClickListener(() =>
            {
                Toggleable.Hide();
            });

            Toggleable.Hide(false, true);
        }
        
    }
}