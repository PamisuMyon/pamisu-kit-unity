using Pamisu.Commons;
using Pamisu.Gameplay;
using Pamisu.Inputs;
using UnityEngine;

namespace Pamisu.TopDownShooter
{
    public class GameManager : GameManagerBase<GameManager>
    {
        [SerializeField]
        private BasicPlayerInput input;
        
        protected override void Start()
        {
            base.Start();
            if (input == null)
                input = FindObjectOfType<BasicPlayerInput>();
        }

        private void Update()
        {
            if (input.Menu)
            {
                if (IsPaused)
                    Resume();
                else
                    Pause();
                input.Menu = false;
            }
        }

        public void OnPlayerDied()
        {
            StartCoroutine(UnityUtil.Delay(3f, () =>
            {
                
                // TODO
            }));
        }
        
    }
}