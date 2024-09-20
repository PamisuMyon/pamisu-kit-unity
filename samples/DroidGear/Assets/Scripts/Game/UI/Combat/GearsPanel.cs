using Game.Combat;
using Game.Events;
using PamisuKit.Common.Pool;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Combat
{
    public class GearsPanel : MonoEntity
    {
        [SerializeField]
        private GameObject _gearItemPrefab;

        private MonoPool<GearItem> _gearItemPool;
        private LayoutGroup _group;

        protected override void OnCreate()
        {
            base.OnCreate();
            _group = GetComponent<LayoutGroup>();
        }

        public void Init()
        {
            _gearItemPool = MonoPool<GearItem>.Create(_gearItemPrefab, _group.transform);
            
            var drone = GetSystem<CombatSystem>().Bb.Player.Drone;
            var droneItem = _gearItemPool.Spawn();
            droneItem.Setup(Region);
            droneItem.Init(drone.Config);

            On<GearAdded>(OnGearAdded);
        }

        private void OnGearAdded(GearAdded e)
        {
            var item = _gearItemPool.Spawn();
            item.Setup(Region);
            item.Init(e.Config);
        }
    }
}
