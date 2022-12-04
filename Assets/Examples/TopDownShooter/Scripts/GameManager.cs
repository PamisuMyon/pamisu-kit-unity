using Pamisu.Commons;
using Pamisu.TopDownShooter.Player;
using Pamisu.TopDownShooter.UI;
using UnityEngine;

namespace Pamisu.TopDownShooter
{
    public class GameManager : SingletonBehaviour<GameManager>
    {

        private UIHud hud;
        public PlayerController Player { get; private set; }
        
        public bool IsPaused { get; private set; }
        
        private void Start()
        {
            Player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            Player.Attributes.OnDied += OnPlayerDied;

            hud = FindObjectOfType<UIHud>();
        }

        private void Update()
        {
            if (Player.Input.Menu)
            {
                if (IsPaused)
                    Resume();
                else
                    Pause();
                Player.Input.Menu = false;
            }
        }

        public void Pause()
        {
            hud.ToggleMenu(true);
            IsPaused = true;
            // Time.timeScale = 0;
            Player.Input.Asset.Player.Disable();
            Player.Input.Asset.Menu.Enable();
        }

        public void Resume()
        {
            hud.ToggleMenu(false);
            IsPaused = false;
            // Time.timeScale = 1f;
            Player.Input.Asset.Player.Enable();
            Player.Input.Asset.Menu.Disable();
        }

        private void OnPlayerDied()
        {
            StartCoroutine(UnityUtil.Delay(3f, () =>
            {
                
                // TODO
            }));
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        
    }
}