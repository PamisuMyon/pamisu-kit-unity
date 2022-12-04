using System;
using System.Collections;
using Pamisu.Commons;
using Pamisu.Commons.Pool;
using UnityEngine;

namespace Pamisu.TopDownShooter.Enemies
{
    public class RotateScatterShootAction : RotateShootAction
    {
        [Space]
        [SerializeField]
        private int branches = 3;
        [SerializeField]
        private float deltaAngle = 15f;
        
        private float totalAngle;
        private Quaternion rotDelta;
        
        protected override void Start()
        {
            base.Start();
            totalAngle = deltaAngle * ((branches - 1) / 2f);
            rotDelta = Quaternion.AngleAxis(deltaAngle, Vector3.up);
        }
        
        protected override IEnumerator DoPerform(Action onCompleted)
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

        protected override void DoShoot()
        {
            var dir = Owner.transform.forward;
            var startDir = Quaternion.AngleAxis(-totalAngle, Vector3.up) * dir;
            dir = startDir;
            for (var j = 0; j < branches; j++)
            {
                var go = GameObjectPooler.Spawn(projectilePrefab);
                var projectile = go.GetComponent<Projectile>();
                projectile.Spawn(Owner.FirePoints.RandomItem().position, Owner.transform.rotation, Owner.gameObject.layer);
                projectile.transform.forward = dir;

                dir = rotDelta * dir;
            }            
        }
        
    }
}