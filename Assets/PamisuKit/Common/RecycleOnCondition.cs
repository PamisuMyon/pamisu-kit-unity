using System.Collections;
// using Pamisu.Common.Pool;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pamisu.Common
{

    public class RecycleOnCondition : MonoBehaviour
    {
        [SerializeField]
        private float _delay = -1;
        [SerializeField]
        private bool _destroyOnRecycle;

        void OnEnable()
        {
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
