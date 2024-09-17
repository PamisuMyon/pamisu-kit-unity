using Game.Combat;
using PamisuKit.Common.Pool;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Combat
{
    public class DroidsPanel : MonoEntity
    {
        [SerializeField]
        private GameObject _droidItemPrefab;

        private MonoPool<DroidItem> _droidItemPool;
        private LayoutGroup _group;

        protected override void OnCreate()
        {
            base.OnCreate();
            _group = GetComponent<LayoutGroup>();
        }

        public void Init()
        {
            _droidItemPool = MonoPool<DroidItem>.Create(_droidItemPrefab, _group.transform);
            
            var drone = GetSystem<CombatSystem>().Bb.Player.Drone;
            var droneItem = _droidItemPool.Spawn();
            droneItem.Setup(Region);
            droneItem.Init(drone.Config);
        }
    }
}
