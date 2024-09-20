using Game.Combat.States;
using Game.Events;
using PamisuKit.Common;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.UI.Combat
{
    public class CombatHud : MonoEntity
    {
        [SerializeField]
        private HeroPanel _heroPanel;

        [FormerlySerializedAs("_droidsPanel")]
        [SerializeField]
        private GearsPanel _gearsPanel;

        protected override void OnCreate()
        {
            base.OnCreate();
            _heroPanel.Setup(Region);
            _gearsPanel.Setup(Region);
            _heroPanel.Go.SetActive(false);
            _gearsPanel.Go.SetActive(false);
            
            On<CombatStateChanged>(OnCombatStateChanged);
        }

        private void OnCombatStateChanged(CombatStateChanged e)
        {
            if (e.StateType == typeof(CombatStates.Battle))
            {
                _heroPanel.Go.SetActive(true);
                _heroPanel.Init();
                _gearsPanel.Go.SetActive(true);
                _gearsPanel.Init();
            }
        }
        
    }
}
