using System;
using Game.Configs;
using Game.Framework;
using PamisuKit.Common.Util;
using UnityEngine;

namespace Game.Props
{
    public class Spray : MonoBehaviour
    {

        [SerializeField]
        private ParticleSystem _mainParticle;

        [SerializeField]
        private Collider _hitBox;
        
        public EffectConfig EffectConfig { get; protected set; }
        // public bool IsActive { get; protected set; }
        public event Action<Character> AreaEnter;
        public event Action<Character> AreaExit;

        private void Awake()
        {
            _hitBox.enabled = false;
        }

        public void SetData(int layer)
        {
            _hitBox.gameObject.layer = layer;
        }

        public void Activate()
        {
            _mainParticle.Play();
            _hitBox.enabled = true;
        }

        public void Cancel()
        {
            _mainParticle.Stop();
            _hitBox.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponentInDirectParent<Character>(out var character))
            {
                AreaEnter?.Invoke(character);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponentInDirectParent<Character>(out var character))
            {
                AreaExit?.Invoke(character);
            }
        }
        
    }
}