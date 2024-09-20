using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.Events;
using PamisuKit.Common.Util;
using PamisuKit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Combat
{
    public class GearItem : MonoEntity
    {
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private RectTransform _upgradeMark;

        [SerializeField]
        private TMP_Text _levelText;

        private CharacterConfig _config;

        protected override void OnCreate()
        {
            base.OnCreate();
            _upgradeMark.gameObject.SetActive(false);
        }

        public void Init(CharacterConfig config)
        {
            _config = config;
            _icon.LoadSprite(config.IconRef).Forget();
            
            On<GearUpgraded>(OnGearUpgraded);
        }
        
        private void OnGearUpgraded(GearUpgraded e)
        {
            if (e.Config != _config)
                return;
            if (e.Level > 0)
            {
                _upgradeMark.gameObject.SetActive(true);
                _levelText.text = e.Level.ToString();
            }
        }
        
    }
}