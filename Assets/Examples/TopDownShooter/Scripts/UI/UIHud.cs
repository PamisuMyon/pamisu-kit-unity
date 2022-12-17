using System.Collections;
using Pamisu.TopDownShooter.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Pamisu.TopDownShooter.UI
{
    public class UIHud : MonoBehaviour
    {

        public Slider HPSlider;
        public UISkill Skill1;
        public UIMenu Menu;

        private PlayerController pc;

        private void Start()
        {
            pc = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            pc.Attributes.OnHealthChanged += OnPlayerHealthChanged;
            StartCoroutine(UpdateSkill1());
            Menu.gameObject.SetActive(false);

            GameManager.Instance.OnPause += () =>
            {
                ToggleMenu(true);
            };
            GameManager.Instance.OnResume += () =>
            {
                ToggleMenu(false);
            };
        }

        private void OnPlayerHealthChanged(float delta, float health, float maxHealth)
        {
            HPSlider.value = health / maxHealth;
        }
        
        public void ToggleMenu(bool isShow)
        {
            Menu.Toggle(isShow);
        }

        private IEnumerator UpdateSkill1()
        {
            while (true)
            {
                Skill1.SetProgress(pc.CannonCooldownCounter / pc.CannonCooldown);
                yield return new WaitForSeconds(0.2f);
            }
            // ReSharper disable once IteratorNeverReturns
        }
        
    }
}