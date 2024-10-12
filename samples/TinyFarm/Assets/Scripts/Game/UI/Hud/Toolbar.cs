using LitMotion;
using LitMotion.Extensions;
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
        private Button _plantButton;

        [SerializeField]
        private PlantBar _plantBar;
        
        protected override void OnCreate()
        {
            base.OnCreate();
            
            _plantBar.Setup(Region);
            _plantBar.Toggleable.Toggling += b =>
            {
                if (b)
                    Hide();
                else
                    Show();
            };
            
            _plantButton.SetOnClickListener(() =>
            {
                _plantBar.Toggleable.Show();
            });
            
            Show(false);
        }
        
    }
}