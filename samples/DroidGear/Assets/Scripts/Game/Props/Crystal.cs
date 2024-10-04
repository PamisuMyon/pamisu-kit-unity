using Game.Combat;
using Game.Framework;
using UnityEngine;

namespace Game.Props
{
    public class Crystal : Pickup
    {
        [SerializeField]
        private float _experience = 2f;

        [SerializeField]
        private float _rotationSpeed = 360f;

        private Vector3 _rotationAxis;

        public override void OnUpdate(float deltaTime)
        {
            Trans.Rotate(_rotationAxis, _rotationSpeed * deltaTime);
            base.OnUpdate(deltaTime);
        }

        public override void OnSpawnFromPool()
        {
            base.OnSpawnFromPool();
            transform.rotation = Random.rotation;
            _rotationAxis = Random.insideUnitSphere;
        }

        protected override void OnPickupComplete(Character picker)
        {
            OnPickupCompleteDelegate?.Invoke(this);
            GetDirector<GameDirector>().Pooler.Release(this);
            GetSystem<CombatSystem>().AddPlayerExp(_experience);
        }
    }
}