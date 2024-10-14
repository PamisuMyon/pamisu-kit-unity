using Cysharp.Threading.Tasks;
using Game.Configs;
using PamisuKit.Common.Pool;
using PamisuKit.Common.Util;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Farm
{
    public class Crop : MonoEntity, IPoolElement
    {

        private SpriteRenderer _spriteRenderer;
        private float _growthCounter;
        
        public SeedConfig Config { get; private set; } 
        public int PhaseIndex { get; private set; }
        public GrowthPhase Phase => Config.Phases[PhaseIndex];
        public bool IsMaxPhase => PhaseIndex >= Config.Phases.Length - 1;
        
        protected override void OnCreate()
        {
            base.OnCreate();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetData(SeedConfig config)
        {
            Config = config;
            PhaseIndex = 0;
            Refresh();
        }

        private void Refresh()
        {
            _spriteRenderer.LoadSprite(Phase.SpriteRef).Forget();
        }

        public bool AddGrowthTime(float delta)
        {
            _growthCounter += delta;
            if (_growthCounter >= Phase.Duration)
            {
                AddPhase();
                _growthCounter = 0;
                return true;
            }
            return false;
        }

        public void AddPhase()
        {
            if (IsMaxPhase)
                return;
            PhaseIndex++;
            Refresh();
        }

        public void OnSpawnFromPool()
        {
            gameObject.SetActive(true);
        }

        public void OnReleaseToPool()
        {
            Config = null;
            PhaseIndex = 0;
            gameObject.SetActive(false);
        }
    }
}