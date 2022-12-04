using System.Collections;
using UnityEngine;

namespace Pamisu.Commons.Pool
{

    public class RecycleOnCondition : MonoBehaviour
    {
        [SerializeField]
        private float delay = -1;
        [SerializeField]
        private bool destroyOnRecycle;

        private void OnEnable()
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (delay != -1)
            {
                StartCoroutine(DelayDeactivate());
            }
        }

        private IEnumerator DelayDeactivate()
        {
            yield return new WaitForSeconds(delay);
            OnRecycle();
        }

        public virtual void OnRecycle()
        {
            if (!destroyOnRecycle)
            {
                GameObjectPooler.Release(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

}
