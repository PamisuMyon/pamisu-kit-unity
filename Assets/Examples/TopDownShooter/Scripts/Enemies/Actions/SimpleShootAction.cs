using System;
using System.Collections;
using Pamisu.Commons;
using Pamisu.Commons.Pool;
using UnityEngine;

namespace Pamisu.TopDownShooter.Enemies
{
    public class SimpleShootAction : EnemyAction
    {

        [SerializeField]
        protected float turnSpeed = 1080f;
        [SerializeField]
        protected int shotCount = 1;
        [SerializeField]
        protected float shootPreDelay = .1f;
        [SerializeField]
        protected float shootPostDelay = .1f;
        [SerializeField]
        protected float shootInterval = .5f;
        [SerializeField]
        protected GameObject projectilePrefab;

        protected Coroutine performCoroutine;

        public override void Perform(Action onCompleted)
        {
            performCoroutine = StartCoroutine(DoPerform(onCompleted));
        }

        public override void Stop()
        {
            StopCoroutine(performCoroutine);
        }
        
        protected virtual IEnumerator DoPerform(Action onCompleted)
        {
            var dir = Owner.Blackboard.TrackTarget.transform.position - Owner.transform.position;
            dir.y = 0;
            
            // Look at target
            while (!Owner.RotateTowards(dir, turnSpeed))
            {
                yield return null;
            }
            
            for (var i = 0; i < shotCount; i++)
            {
                Owner.Animator.SetTrigger(AnimID.Shoot);
                yield return new WaitForSeconds(shootPreDelay);
                var go = GameObjectPooler.Spawn(projectilePrefab);
                var projectile = go.GetComponent<Projectile>();
                projectile.Spawn(Owner.FirePoints.RandomItem().position, Owner.transform.rotation, Owner.gameObject.layer);
                yield return new WaitForSeconds(shootPostDelay);
                yield return new WaitForSeconds(shootInterval);
            }
            
            Owner.Blackboard.ActionBlockCounter = BlockTime;
            CooldownCounter = Cooldown;

            onCompleted.Invoke();
        }
        
    }
}