using UnityEngine;
using UnityEngine.UI;

namespace Pamisu.TopDownShooter.UI
{
    public class UISkill : MonoBehaviour
    {
        public Image Fill;

        public void SetProgress(float progress)
        {
            Fill.fillAmount = progress;
        }
    }
}