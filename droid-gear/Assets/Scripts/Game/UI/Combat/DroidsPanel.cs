using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Combat
{
    public class DroidsPanel : MonoBehaviour
    {
        private LayoutGroup _group;

        private void Awake()
        {
            _group = GetComponent<LayoutGroup>();
        }

        public void Init()
        {
            
        }
    }
}
