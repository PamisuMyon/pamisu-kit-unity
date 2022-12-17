using Pamisu.Commons;
using UnityEngine.Audio;

namespace Pamisu.Gameplay
{
    public class AudioManager : SingletonBehaviour<AudioManager>
    {
        public AudioMixer Mixer;
        public string ParamMusicVolume = "MusicVolume";
        public string ParamSoundVolume = "SoundVolume";

        public void SetVolume(string param, float value, bool inPercent = false)
        {
            if (inPercent)
                value = CommonUtil.RemapFrom01(value, -80f, 0);
            Mixer.SetFloat(param, value);
        }

        public float GetVolume(string param, bool inPercent = false)
        {
            Mixer.GetFloat(param, out var value);
            if (inPercent)
                value = CommonUtil.RemapTo01(value, -80f, 0);
            return value;
        }
        
    }
}