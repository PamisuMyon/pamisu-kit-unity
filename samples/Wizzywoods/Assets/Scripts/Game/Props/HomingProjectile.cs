using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Framework;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Combat.Props
{
    public class HomingProjectile : MonoEntity
    {

        protected IEntity Target;
        protected CancellationTokenSource CtsPerform;
        
        public event Action<HomingProjectile> Release;
        
        public UniTask Perform(Character instigator, IEntity target, Vector3 position)
        {
            CtsPerform?.Cancel();
            CtsPerform = new CancellationTokenSource();
            
            Go.SetActive(true);
            Trans.position = position;
            Target = target;
            
            return DoPerform(CtsPerform.Token);
        }

        private async UniTask DoPerform(CancellationToken cancellationToken)
        {
            Vector3 dir;
            do
            {
                var position = Trans.position;
                var targetPos = Target.Trans.position;
                targetPos.y = position.y;
                dir = targetPos - position;
                Trans.forward = dir;
                
                // TODO hard-coded
                var translate = Trans.forward * 10f * Time.deltaTime;
                Trans.Translate(translate, Space.World);
                await UniTask.Yield(cancellationToken);
            } while (dir.sqrMagnitude > .1f);
            HitTarget();
        }

        private void HitTarget()
        {
            Release?.Invoke(this);
        }

        public void OnRelease()
        {
            CtsPerform?.Cancel();
            Go.SetActive(false);
            Target = null;
        }
        
    }
}