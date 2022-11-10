using System.Collections;
using UnityEngine;

namespace Pamisu.Commons
{

    public class RecycleOnCondition : MonoBehaviour
    {
        [SerializeField]
        private float _delay = -1;
        [SerializeField]
        private bool _destroyOnRecycle;

        void OnEnable()
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (_delay != -1)
            {
                StartCoroutine(DelayDeactivate());
            }
        }

        IEnumerator DelayDeactivate()
        {
            yield return new WaitForSeconds(_delay);
            OnAnimFinished();
        }

        public void OnAnimFinished()
        {
            if (!_destroyOnRecycle)
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
