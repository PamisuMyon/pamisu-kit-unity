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

        protected Action<Pickup> OnPickupCompleteDelegate;
        
        private bool _isPicking;
        private Character _picker;
        private MonoPooler _pooler;

        public bool CanPickup => !_isPicking;

        public virtual void OnUpdate(float deltaTime)
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
                OnPickupComplete(_picker);
            }
            else
            {
                var moveOffset = _flySpeed * deltaTime * dir.normalized;
                selfPos += moveOffset;
                Trans.position = selfPos;
            }
        }

        public virtual void OnSpawnFromPool()
        {
            _isPicking = false;
        }

        public void OnReleaseToPool()
        {
            _isPicking = false;
            gameObject.SetActive(false);
        }

        public void Pick(Character picker, Action<Pickup> onComplete = null)
        {
            if (_isPicking)
                return;
            _isPicking = true;
            _picker = picker;
            OnPickupCompleteDelegate = onComplete;
        }

        protected virtual void OnPickupComplete(Character picker)
        {
            OnPickupCompleteDelegate?.Invoke(this);
            GetDirector<GameDirector>().Pooler.Release(this);
        }

    }
}
