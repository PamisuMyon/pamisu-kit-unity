using System.Collections.Generic;
using Game.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "DroidConfig", menuName = "Configs/DroidConfig", order = 0)]
    public class DroidConfig : CharacterConfig
    {
        [Header("Droid")]
        public float MinFollowRadius = 5f;
        public float MaxFollowRadius = 10f;
    }
}
