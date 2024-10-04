using Game.Events;
using PamisuKit.Common;
using PamisuKit.Common.Util;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Combat
{
    // TODO TEMP
    public class UI_Combat : MonoBehaviour
    {

        [SerializeField]
        private Button _skillButton1;

        [SerializeField]
        private Button _endTurnButton;

        private void Start()
        {
            // TODO hard-coded
            _skillButton1.SetOnClickListener(() =>
            {
                EventBus.Emit(new RequestActivatePlayerAbility { Id = "abi_mage_strike"});
            });
            
            _endTurnButton.SetOnClickListener(() =>
            {
                EventBus.Emit(new RequestEndPlayerTurn());
            });
        }
    }
}