using System;
using Game.UI.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Hud
{
    public class PlantBar : ItemContainer
    {
        [Space]
        [SerializeField]
        private Button _closeButton;

        public event Action<bool> Toggling;
        
        

    }
}