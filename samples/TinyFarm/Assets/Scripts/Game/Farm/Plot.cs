using Game.Configs;
using Game.Events;
using Game.Farm.Models;
using Game.Framework;
using Game.Inventory.Models;
using Game.Worker.Models;
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
        public override bool IsActive => !IsPendingDestroy
                                         && Data != null
                                         && Data.HasCrop 
                                         && !Data.Crop.IsRipe
                                         && Data.IsWatered
                                         && Go.activeInHierarchy;

        protected override void OnCreate()
        {
            base.OnCreate();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        public void Init(PlotData data)
        {
            Data = data;
            if (string.IsNullOrEmpty(Data.Id))
                Data.Id = GenerateId();
            Id = Data.Id;
        }

        public void OnUpdate(float deltaTime)
        {
            if (Crop.AddGrowthTime(deltaTime))
            {
                Data.IsWatered = false;
                if (Crop.Data.IsRipe)
                {
                    Emit(new ReqAddWorkerTask
                    {
                        Target = this,
                        Type = WorkerTaskType.Harvesting
                    });
                }
                else
                {
                    Emit(new ReqAddWorkerTask
                    {
                        Target = this,
                        Type = WorkerTaskType.Watering
                    });
                }
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

            Crop = GetDirector<GameDirector>().Pooler.Spawn<Crop>(_cropPrefab);
            Crop.Setup(Region);
            Crop.Trans.SetParent(Trans);
            Crop.Trans.localPosition = Vector3.zero;
            Crop.SetData(Data.Crop = new CropData(seedConfig));
            Data.IsWatered = false;
            
            Emit(new ReqAddWorkerTask
            {
                Target = this,
                Type = WorkerTaskType.Watering
            });
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
                Data.Crop = null;
            }
            return false;
        }
        
    }
}