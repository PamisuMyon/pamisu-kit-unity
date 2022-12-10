using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pamisu.Commons.Pool
{

    public class RecycleOnCondition : MonoBehaviour
    {
        [FormerlySerializedAs("delay")]
        [SerializeField]
        private float recycleDelay = -1;
        [SerializeField]
        private bool destroyOnRecycle;

        private void OnEnable()
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (recycleDelay != -1)
            {
                StartCoroutine(DelayDeactivate());
            }
        }

        private IEnumerator DelayDeactivate()
        {
            yield return new WaitForSeconds(recycleDelay);
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
