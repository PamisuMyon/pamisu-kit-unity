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
            _upgradeItemPool = MonoPool<UpgradeItemView>.Create(_upgradeItemPrfab, _panel);

            for (int i = 0; i < 3; i++)
            {
                var item = _upgradeItemPool.Spawn();
                item.Setup(Region);
                item.Go.SetActive(false);
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
                item.Show(_itemAnimDuration).Forget();
                _items.Add(item);
                await Region.Ticker.Delay(_itemAnimInterval, destroyCancellationToken);
            }
        }

        private async UniTaskVoid Hide()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                HideAndReleaseItem(_items[i]).Forget();
                if (i != _items.Count - 1)
                    await Region.Ticker.Delay(_itemAnimInterval, destroyCancellationToken);
            }
            await Region.Ticker.Delay(_itemAnimDuration);
            _panel.gameObject.SetActive(false);
        }

        private async UniTaskVoid HideAndReleaseItem(UpgradeItemView itemView)
        {
            await itemView.Hide(_itemAnimDuration);
            itemView.gameObject.SetActive(false);
            _upgradeItemPool.Release(itemView);
        }

        private void OnItemClicked(UpgradeItem item)
        {
            Emit(new ReqSelectUpgradeItem { Item = item });
            Hide().Forget();
        }
        
    }
}