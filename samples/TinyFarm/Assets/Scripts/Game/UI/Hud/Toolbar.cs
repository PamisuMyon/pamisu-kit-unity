using Game.Events;
using Game.Framework;
using PamisuKit.Common.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Hud
{
    public class Toolbar : ToggleableView
    {
        [Header("Toolbar")]
        [SerializeField]
        private Button _shovelButton;

        [SerializeField]
        private ShovelDragDummy _shovelDragDummy;

        [SerializeField]
        private Button _plantButton;

        [SerializeField]
        private PlantBar _plantBar;
        
        protected override void OnCreate()
        {
            base.OnCreate();
            _plantBar.Setup(Region);
            _plantButton.SetOnClickListener(() =>
            {
                _plantBar.Toggleable.Show();
            });
            
            _shovelDragDummy.Setup(Region);
            _shovelDragDummy.Go.SetActive(false);
            _shovelButton.SetOnClickListener(TryEnableShovel);

            On<PlayerControlStateChanged>(OnPlayerControlStateChanged);
            
            Show(false);
        }

        private void OnPlayerControlStateChanged(PlayerControlStateChanged e)
        {
            if (e.NewState == PlayerControlState.Normal)
            {
                Show();
            }
        }

        private void TryEnableShovel()
        {
            var dragHelper = GetService<ClickDragHelper>();
            if (dragHelper.IsDragging)
                return;
            dragHelper.BeginDrag(_shovelDragDummy);
        }
        
    }
}