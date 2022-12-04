using UnityEngine;
using UnityEngine.UI;

namespace Pamisu.TopDownShooter.UI
{
    public class UIMenu : MonoBehaviour
    {
        public Toggle FullScreenToggle;
        public Slider MusicSlider;
        public Slider SoundSlider;
        public Button ResumeButton;
        public Button QuitButton;


        private void Start()
        {
            ResumeButton.onClick.AddListener(OnResumeButtonClicked);
            QuitButton.onClick.AddListener(OnQuitButtonClicked);
        }

        public void OnResumeButtonClicked()
        {
            GameManager.Instance.Resume();
        }

        public void OnQuitButtonClicked()
        {
            GameManager.Instance.Quit();
        }
        
    }
}