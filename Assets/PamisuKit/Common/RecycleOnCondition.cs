using System.Collections;
// using Pamisu.Common.Pool;
using UnityEngine;

namespace Pamisu.Common
{

    public class RecycleOnCondition : MonoBehaviour
    {
        [SerializeField]
        private float delay = -1;
        [SerializeField]
        private bool destroyOnRecycle;

        void OnEnable()
        {
            if (delay != -1)
            {
                StartCoroutine(DelayDeactivate());
            }
        }

        IEnumerator DelayDeactivate()
        {
            yield return new WaitForSeconds(delay);
            OnAnimFinished();
        }

        public void OnAnimFinished()
        {
            if (!destroyOnRecycle)
            {
                // PoolManager.ReleaseObject(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

}
