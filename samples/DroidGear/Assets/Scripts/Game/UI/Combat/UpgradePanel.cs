using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Events;
using Game.Upgrades;
using PamisuKit.Common.Pool;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.UI.Combat
{
    public class UpgradePanel : MonoEntity
    {
        [SerializeField]
        private float _itemAnimDuration = .4f;

        [SerializeField]
        private float _itemAnimInterval = .1f;
        
        [SerializeField]
        private GameObject _upgradeItemPrfab;

        [SerializeField]
        private RectTransform _panel;

        [SerializeField]
        private RectTransform _container;
        
        private MonoPool<UpgradeItemView> _upgradeItemPool;
        private readonly List<UpgradeItemView> _items = new();
        
        protected override void OnCreate()
        {
            base.OnCreate();
            _upgradeItemPool = MonoPool<UpgradeItemView>.Create(_upgradeItemPrfab, _container);

            for (int i = 0; i < 3; i++)
            {
                var item = _upgradeItemPool.Spawn();
                item.Setup(Region);
                item.Go.SetActive(false);
                _upgradeItemPool.Release(item);
            }
            
            _panel.gameObject.SetActive(false);

            On<ReqShowUpgradeItems>(OnReqShowUpgradeItems);
        }

        private void OnReqShowUpgradeItems(ReqShowUpgradeItems e)
        {
            Show(e.Items).Forget();
        }

        private async UniTaskVoid Show(List<UpgradeItem> items)
        {
            _panel.gameObject.SetActive(true);
            _items.Clear();
            for (int i = 0; i < items.Count; i++)
            {
                var item = _upgradeItemPool.Spawn();
                item.Setup(Region);
                item.Go.SetActive(true);
                item.SetData(items[i]);
                item.ItemClicked = OnItemClicked;
                _items.Add(item);
            }

            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].Show(_itemAnimDuration).Forget();
                await Region.Ticker.Delay(_itemAnimInterval, destroyCancellationToken);
            }

        }

        private async UniTaskVoid Hide()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].Hide(_itemAnimDuration).Forget();
                if (i != _items.Count - 1)
                    await Region.Ticker.Delay(_itemAnimInterval, destroyCancellationToken);
            }
            await Region.Ticker.Delay(_itemAnimDuration);
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].gameObject.SetActive(false);
                _upgradeItemPool.Release(_items[i]);
            }
            _panel.gameObject.SetActive(false);
        }

        private void OnItemClicked(UpgradeItem item)
        {
            Emit(new ReqSelectUpgradeItem { Item = item });
            Hide().Forget();
        }
        
    }
}