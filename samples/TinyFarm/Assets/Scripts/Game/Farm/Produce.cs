using Cysharp.Threading.Tasks;
using Game.Configs;
using PamisuKit.Common.Pool;
using PamisuKit.Common.Util;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Farm
{
    public class Produce : MonoEntity, IPoolElement
    {
        
        [SerializeField]
        private SpriteRenderer _itemSpriteRenderer;
        
        public ItemConfig Config { get; private set; }
        
        public void SetData(ItemConfig config)
        {
            Config = config;
            _itemSpriteRenderer.LoadSprite(Config.WorldSpriteRef).Forget();
        }

        public void OnSpawnFromPool()
        {
            gameObject.SetActive(true);
        }

        public void OnReleaseToPool()
        {
            Config = null;
            _itemSpriteRenderer.sprite = null;
            gameObject.SetActive(false);
        }
    }
}