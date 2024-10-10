using PamisuKit.Common.Util;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Hud
{
    public class Toolbar : MonoEntity
    {
        [SerializeField]
        private Button _shovelButton;

        [SerializeField]
        private Button _plantButton;

        [SerializeField]
        private PlantBar _plantBar;
        
        protected override void OnCreate()
        {
            base.OnCreate();
            
            _plantButton.SetOnClickListener(() =>
            {
                
            });
        }

        public void Show()
        {
            
        }

        public void Hide()
        {
            
        }
        
    }
}