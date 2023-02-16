using UnityEngine;

namespace Pamisu.Platformer2D
{
    public class GameDebugger : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 1f)]
        private float timeScale = 1f;

        private void Update()
        {
            Time.timeScale = timeScale;
        }
    }
}