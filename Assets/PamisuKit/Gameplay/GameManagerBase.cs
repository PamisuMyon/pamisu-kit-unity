using System;
using System.Collections;
using Pamisu.Commons;
using UnityEngine;

namespace Pamisu.Gameplay
{
    public class GameManagerBase<T> : SingletonBehaviour<T> where T : GameManagerBase<T>
    {
        public Settings Settings { get; protected set; }
        public bool IsPaused { get; protected set; }
        
        public event Action OnPause;
        public event Action OnResume;

        protected virtual void Start()
        {
            StartCoroutine(LateStart());
        }

        protected virtual IEnumerator LateStart()
        {
            yield return null;
            Settings = new Settings();
            Settings.InitSettings();
        }

        public virtual void Pause()
        {
            IsPaused = true;
            Time.timeScale = 0;
            OnPause?.Invoke();
        }

        public virtual void Resume()
        {
            IsPaused = false;
            Time.timeScale = 1f;
            OnResume?.Invoke();
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