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

        [SerializeField]
        private Sprite _normalSprite;

        [SerializeField]
        private Sprite _wateredSprite;

        private SpriteRenderer _spriteRenderer;
        private bool _isWatered;
        private bool _hasCrop;
        
        public Crop Crop { get; private set; }
        public override bool IsActive => !IsPendingDestroy && _hasCrop && _isWatered && Go.activeInHierarchy;

        protected override void OnCreate()
        {
            base.OnCreate();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

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
            _isWatered = false;
            
            // TODO Temp
            _isWatered = true;
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
            if (_hasCrop)
            {
                GetDirector<GameDirector>().Pooler.Release(Crop);
                Crop = null;
                _hasCrop = false;
            }
            return false;
        }

    }
}