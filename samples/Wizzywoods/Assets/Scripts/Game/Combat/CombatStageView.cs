using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Combat.Stage
{
    public class CombatStageView : MonoBehaviour
    {
        public Transform[] PlayerStarts;
        [FormerlySerializedAs("OpponentStarts")]
        public Transform[] EnemyStarts;
    }
}