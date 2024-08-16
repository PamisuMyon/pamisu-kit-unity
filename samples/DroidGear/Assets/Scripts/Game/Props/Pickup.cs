using System;
using Game.Framework;
using PamisuKit.Common.Pool;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Props
{
    public class Pickup : MonoEntity, IUpdatable, IPoolElement
    {
        [SerializeField]
        private float _flySpeed = 10;

        private bool _isPicking;
        private Character _picker;
        private Action<Pickup> _onPickComplete;
        private MonoPooler _pooler;


        public void OnUpdate(float deltaTime)
        {
            if (!_isPicking)
                return;
            var targetPos = _picker.Trans.position;
            targetPos.y += _picker.Model.VisualHeight * .5f;
            var selfPos = Trans.position;
            var dir = targetPos - selfPos;
            if (dir.sqrMagnitude < .1f)
            {
                _isPicking = false;
                _onPickComplete?.Invoke(this);
                _pooler?.Release(this);
                return;
            }
            else
            {
                var moveOffset = _flySpeed * deltaTime * dir.normalized;
                selfPos += moveOffset;
                Trans.position = selfPos;
            }
        }

        public void OnSpawnFromPool()
        {
            _isPicking = false;
        }

        public void OnReleaseToPool()
        {
            Go.SetActive(false);
        }

        public void Pick(Character picker, Action<Pickup> onComplete)
        {
            if (_isPicking)
                return;
            _isPicking = true;
            _picker = picker;
            _pooler = picker.GetDirector<GameDirector>().Pooler;
            _onPickComplete = onComplete;
        }

    }
}
