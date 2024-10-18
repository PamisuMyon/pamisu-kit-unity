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
        
        public void OnSpawnFromPool()
        {
            gameObject.SetActive(true);
        }

        public void OnReleaseToPool()
        {
            Data = null;
            gameObject.SetActive(false);
        }

        private void Refresh()
        {
            _spriteRenderer.LoadSprite(Phase.SpriteRef).Forget();
        }

        public bool AddGrowthTime(float delta)
        {
            Data.GrowthTimeCounter += delta;
            if (Data.GrowthTimeCounter >= Phase.Duration)
            {
                AddPhase();
                Data.GrowthTimeCounter = 0;
                return true;
            }
            return false;
        }

        public void AddPhase()
        {
            if (Data.IsRipe)
                return;
            Data.PhaseIndex++;
            Refresh();
        }

        public bool CanRegrowth() => Data.RegrowthTimes < Data.Config.RegrowthTimes;

        public void Regrowth()
        {
            Data.RegrowthTimes++;
            Data.GrowthTimeCounter = 0;
            Data.PhaseIndex = 0;
            Refresh();
        }

    }
}