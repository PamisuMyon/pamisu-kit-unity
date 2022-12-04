using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Pamisu.TopDownShooter
{
    public class Settings
    {
        public static readonly string KeySettings = "settings";

        public static SettingsData Data { get; protected set; }

        public void ToggleFullScreen(bool isFullScreen)
        {
            
        }

        protected void ReadSettings()
        {
            PlayerPrefs.GetString(KeySettings);
        }

        protected void SaveSettings()
        {
            PlayerPrefs.SetString(KeySettings, JsonConvert.SerializeObject(Data));
        }
    }

    [Serializable]
    public struct SettingsData
    {
        public float ScreenWidth;
        public float ScreenHeight;
        public FullScreenMode FullScreenMode;
    }
}