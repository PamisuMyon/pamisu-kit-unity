using System;
using System.Collections;
using Pamisu.Commons;
using Pamisu.Commons.Pool;
using UnityEngine;

namespace Pamisu.TopDownShooter.Enemies
{
    public class RotateShootAction : EnemyAction
    {

        [SerializeField]
        protected float rotateSpeed = 90f;
        [SerializeField]
        protected float cycles = 3;
        [SerializeField]
        protected float shootPreDelay = .1f;
        [SerializeField]
        protected float shootPostDelay = .1f;
        [SerializeField]
        protected float shootInterval = .5f;
        [SerializeField]
        protected GameObject projectilePrefab;

        protected Coroutine performCoroutine;
        protected bool isPerforming;

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
            isPerforming = true;
            Owner.StartCoroutine(DoRotation());

            while (isPerforming)
            {
                Owner.Animator.SetTrigger(AnimID.Shoot);
                yield return new WaitForSeconds(shootPreDelay);
                DoShoot();
                yield return new WaitForSeconds(shootPostDelay);
                yield return new WaitForSeconds(shootInterval);
            }

            Owner.Blackboard.ActionBlockCounter = BlockTime;
            CooldownCounter = Cooldown;

            onCompleted.Invoke();
        }

        protected IEnumerator DoRotation()
        {
            var duration = cycles * 360f / rotateSpeed;
            var counter = duration;
            while (counter > 0)
            {
                Owner.transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
                counter -= Time.deltaTime;
                yield return null;
            }
            isPerforming = false;
        }

        protected virtual void DoShoot()
        {
            var go = GameObjectPooler.Spawn(projectilePrefab);
            var projectile = go.GetComponent<Projectile>();
            projectile.Spawn(Owner.FirePoints.RandomItem().position, Owner.transform.rotation, Owner.gameObject.layer);
        }
        
    }
}