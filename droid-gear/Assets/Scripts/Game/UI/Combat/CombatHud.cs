using Game.Combat.States;
using Game.Events;
using PamisuKit.Common;
using UnityEngine;

namespace Game.UI.Combat
{
    public class CombatHud : MonoBehaviour
    {
        [SerializeField]
        private HeroPanel _heroPanel;

        [SerializeField]
        private DroidsPanel _droidsPanel;

        private void Awake()
        {
            Init();
        }

        private void Init() 
        {
            EventBus.OnRaw<CombatStateChanged>(OnCombatStateChanged, EventBus.SubscribeOptions.Once);
            _heroPanel.gameObject.SetActive(false);
            _droidsPanel.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            EventBus.Off<CombatStateChanged>(OnCombatStateChanged);
        }

        private void OnCombatStateChanged(CombatStateChanged e)
        {
            Debug.Log("CombatHud OnCombatStateChanged");
            if (e.StateType == typeof(CombatStates.Battle))
            {
                _heroPanel.gameObject.SetActive(true);
                _heroPanel.Init();
                _droidsPanel.gameObject.SetActive(true);
                _droidsPanel.Init().Forget();
            }
        }
    }
}
