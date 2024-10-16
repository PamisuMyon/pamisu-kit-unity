using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.Farm.Models;
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
        
        public CropData Data { get; private set; }
        public SeedConfig Config => Data.Config;
        public GrowthPhase Phase => Config.Phases[Data.PhaseIndex];
        
        protected override void OnCreate()
        {
            base.OnCreate();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetData(CropData data)
        {
            Data = data;
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
            if (Data.IsRipe)
                return;
            Data.PhaseIndex++;
            if (Data.PhaseIndex >= Config.Phases.Length - 1)
                Data.IsRipe = true;
            Refresh();
        }

        public void OnSpawnFromPool()
        {
            gameObject.SetActive(true);
        }

        public void OnReleaseToPool()
        {
            Data = null;
            gameObject.SetActive(false);
        }
    }
}