using Pamisu.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pamisu.TopDownShooter.UI
{
    public class UIMenu : MonoBehaviour
    {
        public TMP_Dropdown QualityDropdown;
        public Toggle FullScreenToggle;
        public Slider MusicSlider;
        public Slider SoundSlider;
        public Button ResumeButton;
        public Button QuitButton;

        private Settings Settings => GameManager.Instance.Settings;
        
        private void Start()
        {
            ResumeButton.onClick.AddListener(OnResumeButtonClicked);
            QuitButton.onClick.AddListener(OnQuitButtonClicked);

            QualityDropdown.onValueChanged.AddListener(OnQualityDropdownValueChanged);
            FullScreenToggle.onValueChanged.AddListener(OnFullScreenToggleValueChanged);
            MusicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
            SoundSlider.onValueChanged.AddListener(OnSoundSliderValueChanged);
        }

        private void Refresh()
        {
            QualityDropdown.SetValueWithoutNotify(Settings.Data.QualityLevel);
            FullScreenToggle.SetIsOnWithoutNotify(Settings.Data.IsFullScreen);
            MusicSlider.SetValueWithoutNotify(Settings.Data.MusicVolume);
            SoundSlider.SetValueWithoutNotify(Settings.Data.SoundVolume);
        }

        public void Toggle(bool isShow)
        {
            if (isShow)
                Refresh();
            else
                GameManager.Instance.Settings.SaveSettings();
            gameObject.SetActive(isShow);
        }

        private void OnResumeButtonClicked()
        {
            GameManager.Instance.Resume();
        }

        private void OnQuitButtonClicked()
        {
            GameManager.Instance.Quit();
        }

        private void OnQualityDropdownValueChanged(int value)
        {
            Settings.SetQualityLevel(value);
        }

        private void OnFullScreenToggleValueChanged(bool isOn)
        {
            Settings.ToggleFullScreen(isOn);
        }

        private void OnMusicSliderValueChanged(float value)
        {
            Settings.SetMusicVolume(value);
        }
        
        private void OnSoundSliderValueChanged(float value)
        {
            Settings.SetSoundVolume(value);
        }
        
    }
}