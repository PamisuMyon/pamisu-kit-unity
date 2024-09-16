using Game.Combat;
using Game.Events;
using Game.Framework;
using PamisuKit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Combat
{
    public class HeroPanel : MonoEntity
    {
        [SerializeField]
        private Slider _healthSlider;

        [SerializeField]
        private Slider _expSlider;

        [SerializeField]
        private TMP_Text _levelText;

        public void Init()
        {
            var bb = App.Instance.GetSystem<CombatSystem>().Bb;
            var player = bb.Player.Chara;
            _healthSlider.value = player.AttrComp[AttributeType.Health].Value / player.AttrComp[AttributeType.MaxHealth].Value;
            player.AttrComp.HealthChanged += OnPlayerHealthChanged;

            _expSlider.value = bb.Experience / bb.NextLevelExperience;
            _levelText.text = $"Lv.{bb.PlayerLevel}";
            On<PlayerExpChanged>(OnPlayerExpChanged);
        }

        private void OnPlayerHealthChanged(AttributeComponent attrComp, float delta, float newHealth)
        {
            _healthSlider.value = newHealth / attrComp[AttributeType.MaxHealth].Value;
        }

        private void OnPlayerExpChanged(PlayerExpChanged e)
        {
            if (e.LevelUpDelta != 0)
                _levelText.text = $"Lv.{e.NewLevel}";
            if (e.ExpDelta != 0)
                _expSlider.value = e.NewExp / e.NextLevelExp;
        }

    }
}
