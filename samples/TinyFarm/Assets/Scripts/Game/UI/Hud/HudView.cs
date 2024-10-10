using PamisuKit.Framework;
using UnityEngine;

namespace Game.UI.Hud
{
    public class HudView : MonoEntity
    {
        [SerializeField]
        private Toolbar _toolbar;

        protected override void OnCreate()
        {
            base.OnCreate();
            _toolbar.Setup(Region);
        }
    }
}