using Game.Configs;
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

        private bool _isWatered;
        private bool _hasCrop;
        
        public Crop Crop { get; private set; }
        public override bool IsActive => !IsPendingDestroy && _hasCrop && _isWatered && Go.activeInHierarchy;
        
        public void OnUpdate(float deltaTime)
        {
            if (Crop.AddGrowthTime(deltaTime))
            {
                _isWatered = false;
            }
        }
        
        public bool CanPlant()
        {
            return !_hasCrop;
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
            Crop.SetData(seedConfig);
            _hasCrop = true;
            
            // TODO Temp
            _isWatered = true;
        }

        public void Water()
        {
        }

        public void Harvest()
        {
        }

        public void RemoveCrop()
        {
        }

    }
}