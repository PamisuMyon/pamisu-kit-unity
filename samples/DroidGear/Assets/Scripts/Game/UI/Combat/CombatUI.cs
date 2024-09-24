using Game.UI.Common;
using PamisuKit.Framework;

namespace Game.UI.Combat
{
    public class CombatUI : MonoEntity
    {
        public CombatOverlay Overlay;
        public CombatHud Hud;
        public InGameMenuView MenuView;
        
        protected override bool AutoSetupOverride => false;

        protected override void OnCreate()
        {
            base.OnCreate();
            
            Overlay.Setup(Region);
            Hud.Setup(Region);
            MenuView.Setup(Region);
        }
    }
}