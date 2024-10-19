using System;
using System.Collections.Generic;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Framework
{
    public class SpriteAnimator : MonoBehaviour, IUpdatable
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private bool _playOnAwake;
        
        [SerializeField]
        private SpriteAnimationClip[] _clips;

        private Dictionary<string, SpriteAnimationClip> _clipDict; 
        private SpriteAnimationClip _currentClip;
        private LoopMode _loopMode;
        private float _frameDuration;
        private float _frameTimeCounter;
        private int _currentFrame;
        
        public bool IsPlaying { get; private set; }
        public bool IsActive => gameObject.activeInHierarchy;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public event Action PlaybackComplete;
        public event Action LoopEnd; 

        private void Awake()
        {
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();
            
            _clipDict = new Dictionary<string, SpriteAnimationClip>();
            for (int i = 0; i < _clips.Length; i++)
            {
                _clipDict[_clips[i].Name] = _clips[i];
            }
            
            if (_playOnAwake && _clips.Length > 0)
                Play(_clips[0].Name);
        }

        public void OnUpdate(float deltaTime)
        {
            if (!IsPlaying)
                return;
            
            _frameTimeCounter -= deltaTime;
            if (_frameTimeCounter <= 0)
            {
                _currentFrame++;
                _frameTimeCounter += _frameDuration;
                if (_currentFrame >= _currentClip.Length)
                {
                    if (_loopMode == LoopMode.OneShot
                        || (_loopMode == LoopMode.Default && !_currentClip.IsLoop))
                    {
                        Stop();
                        PlaybackComplete?.Invoke();
                        return;
                    }
                    else
                    {
                        _currentFrame %= _currentClip.Length;
                        LoopEnd?.Invoke();
                    }
                }
                _spriteRenderer.sprite = _currentClip[_currentFrame];
            }
        }

        public void Play(string clipName, LoopMode loopMode = LoopMode.Default)
        {
            IsPlaying = true;
            if (_currentClip != null && _currentClip.Name == clipName && _currentClip.IsLoop)
                return;
            _currentClip = _clipDict[clipName];
            _frameDuration = 1f / _currentClip.SampleRate;
            _frameTimeCounter = _frameDuration;
            _currentFrame = 0;
            _spriteRenderer.sprite = _currentClip[_currentFrame];
        }

        public void Stop()
        {
            IsPlaying = false;
            _currentFrame = 0;
        }
        
        public enum LoopMode
        {
            Default,
            OneShot,
        }
        
    }

    [Serializable]
    public class SpriteAnimationClip
    {
        public string Name;
        public bool IsLoop = true;
        public int SampleRate = 60;
        public Sprite[] Frames;

        public Sprite this[int index] => Frames[index];
        public int Length => Frames.Length;
    }
}