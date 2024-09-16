using Game.Combat.States;
using Game.Events;
using PamisuKit.Common;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.UI.Combat
{
    public class CombatHud : MonoEntity
    {
        [SerializeField]
        private HeroPanel _heroPanel;

        [SerializeField]
        private DroidsPanel _droidsPanel;

        protected override void OnCreate()
        {
            base.OnCreate();
            _heroPanel.Setup(Region);
            _droidsPanel.Setup(Region);
            _heroPanel.Go.SetActive(false);
            _droidsPanel.Go.SetActive(false);
            
            On<CombatStateChanged>(OnCombatStateChanged);
        }

        private void OnCombatStateChanged(CombatStateChanged e)
        {
            if (e.StateType == typeof(CombatStates.Battle))
            {
                _heroPanel.Go.SetActive(true);
                _heroPanel.Init();
                _droidsPanel.Go.SetActive(true);
                _droidsPanel.Init();
            }
        }
        
    }
}
