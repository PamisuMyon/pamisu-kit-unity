using System;
using System.Collections.Generic;
using Game.Characters.Player;
using Game.Framework;

namespace Game.Combat.States
{
    public static partial class CombatStates
    {
        [Serializable]
        public class Blackboard
        {
            public float CombatTime { get; internal set; }
            public int Level { get; internal set; }
            public float Experience { get; internal set; }
            public float NextLevelExperience { get; internal set; }
            public PlayerController Player { get; internal set; }
            public List<CharacterController> EnemiesOnStage { get; } = new();
        }
    }
}
