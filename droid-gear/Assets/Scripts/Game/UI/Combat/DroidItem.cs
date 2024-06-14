using Game.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Combat
{
    public class DroidItem : MonoBehaviour
    {
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private RectTransform _upgrade;

        [SerializeField]
        private TMP_Text _levelText;

        public void Bind(CharacterConfig config)
        {
            
        }

    }
}