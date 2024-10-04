using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "DroidConfig", menuName = "Configs/DroidConfig", order = 103)]
    public class DroidConfig : CharacterConfig
    {
        [Header("Droid")]
        public float MinFollowRadius = 5f;
        public float MaxFollowRadius = 10f;
    }
}
