using Game.Configs;
using Game.Farm.Models;
using Game.Framework;
using Game.Inventory.Models;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Farm
{
    public class Plot : Unit, IUpdatable
    {
        [SerializeField]
        private GameObject _cropPrefab;

        [SerializeField]
        private Sprite _normalSprite;

        [SerializeField]
        private Sprite _wateredSprite;

        private SpriteRenderer _spriteRenderer;
        
        public PlotData Data { get; private set; }
        public Crop Crop { get; private set; }
        public override bool IsActive => !IsPendingDestroy && Data.HasCrop && Data.IsWatered && Go.activeInHierarchy;

        protected override void OnCreate()
        {
            base.OnCreate();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        public void Init(PlotData data)
        {
            Data = data;
        }

        public void OnUpdate(float deltaTime)
        {
            if (Crop.AddGrowthTime(deltaTime))
            {
                Data.IsWatered = false;
            }
        }
        
        public bool CanPlant()
        {
            return !Data.HasCrop;
        }

        public void Plant(Item plantItem)
        {
            if (plantItem.Config is not SeedConfig seedConfig)
            {
                Debug.LogError($"Plot plant item SeedConfig expected, got {plantItem.Config.GetType()}", Go);
                return;
            }

            plantItem.ChangeAmount(-1);

            Data.Crop = new CropData(seedConfig);
            Crop = GetDirector<GameDirector>().Pooler.Spawn<Crop>(_cropPrefab);
            Crop.Setup(Region);
            Crop.Trans.SetParent(Trans);
            Crop.Trans.localPosition = Vector3.zero;
            Crop.SetData(Data.Crop);
            Data.HasCrop = true;
            Data.IsWatered = false;
            
            // TODO Temp
            Data.IsWatered = true;
        }

        private void Refresh()
        {
            
        }

        public void Water()
        {
        }

        public void Harvest()
        {
        }

        public bool RemoveCrop()
        {
            if (Data.HasCrop)
            {
                GetDirector<GameDirector>().Pooler.Release(Crop);
                Crop = null;
                Data.HasCrop = false;
                Data.Crop = null;
            }
            return false;
        }
        
    }
}