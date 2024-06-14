using Cysharp.Threading.Tasks;
using Game.Combat;
using PamisuKit.Common.Pool;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Game.UI.Combat
{
    public class DroidsPanel : MonoBehaviour
    {
        [SerializeField]
        private AssetReferenceGameObject _droidItemRef;

        private MonoPool<DroidItem> _droidItemPool;
        private LayoutGroup _group;

        private void Awake()
        {
            _group = GetComponent<LayoutGroup>();
        }

        public async UniTaskVoid Init()
        {
            _droidItemPool = await MonoPool<DroidItem>.Create(_droidItemRef, _group.transform);
            
            var drone = CombatSystem.Instance.Bb.Player.Drone;
        }
    }
}
