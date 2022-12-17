using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Pamisu.Gameplay
{
    public class Settings
    {
        public const string KeySettings = "settings";

        public SettingsData Data;

        public Settings()
        {
            Data = new SettingsData();
        }

        public void InitSettings()
        {
            ReadSettings();
            if (!Data.Initialized)
            {
                Data.Initialized = true;

                Data.QualityLevel = QualitySettings.GetQualityLevel();
                
                Data.FullScreenMode = Screen.fullScreenMode;
                Data.IsFullScreen = Screen.fullScreen;
                var currentResolution = Screen.currentResolution;
                Data.WindowedResolution = new Vector2Int(currentResolution.width, currentResolution.height);
                var fullResolution = Screen.resolutions[^1];
                Data.FullScreenResolution = new Vector2Int(fullResolution.width, fullResolution.height);

                var audioManager = AudioManager.Instance;
                Data.MusicVolume = audioManager.GetVolume(audioManager.ParamMusicVolume, true);
                Data.SoundVolume = audioManager.GetVolume(audioManager.ParamSoundVolume, true);

                SaveSettings();
            }

            ApplyAllSettings();
        }

        public void ReadSettings()
        {
            var json = PlayerPrefs.GetString(KeySettings);
            if (!string.IsNullOrEmpty(json))
                Data = JsonConvert.DeserializeObject<SettingsData>(json);
        }

        public void SaveSettings()
        {
            PlayerPrefs.SetString(KeySettings, JsonConvert.SerializeObject(Data));
        }

        public void ApplyAllSettings()
        {
            ToggleFullScreen(Data.IsFullScreen);
            SetMusicVolume(Data.MusicVolume);
            SetSoundVolume(Data.SoundVolume);
        }

        public void SetQualityLevel(int level)
        {
            QualitySettings.SetQualityLevel(level);
        }
        
        public Resolution[] GetResolutions()
        {
            return Screen.resolutions;
        }

        public void SetResolution(int width, int height)
        {
            if (Data.IsFullScreen)
                Data.FullScreenResolution = new Vector2Int(width, height);
            else
                Data.WindowedResolution = new Vector2Int(width, height);
            Screen.SetResolution(width, height, Data.FullScreenMode);
        }

        public void ToggleFullScreen(bool isFullScreen)
        {
            Data.IsFullScreen = isFullScreen;
            if (isFullScreen)
            {
                Data.FullScreenMode = FullScreenMode.FullScreenWindow;
                Screen.SetResolution(Data.FullScreenResolution.x, Data.FullScreenResolution.y, Data.FullScreenMode);
            }
            else
            {
                Data.FullScreenMode = FullScreenMode.Windowed;
                if (Data.WindowedResolution != Vector2.zero)
                    Screen.SetResolution(Data.WindowedResolution.x, Data.WindowedResolution.y, Data.FullScreenMode);
                else
                    Screen.fullScreenMode = Data.FullScreenMode;
            }
        }

        public void SetMusicVolume(float value)
        {
            AudioManager.Instance.SetVolume(AudioManager.Instance.ParamMusicVolume, value, true);
            Data.MusicVolume = value;
        }

        public void SetSoundVolume(float value)
        {
            AudioManager.Instance.SetVolume(AudioManager.Instance.ParamSoundVolume, value, true);
            Data.SoundVolume = value;
        }
        
    }

    [Serializable]
    public struct SettingsData
    {
        public bool Initialized;
        public int QualityLevel;
        public Vector2Int WindowedResolution;
        public Vector2Int FullScreenResolution;
        public FullScreenMode FullScreenMode;
        public bool IsFullScreen;
        public float MusicVolume;
        public float SoundVolume;
        
    }
}