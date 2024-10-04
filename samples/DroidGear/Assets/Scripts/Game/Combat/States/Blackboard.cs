using System;
using System.Collections.Generic;
using Game.Characters;
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
            public int PlayerLevel { get; internal set; }
            public float Experience { get; internal set; }
            public float NextLevelExperience { get; internal set; }
            public PlayerController Player { get; internal set; }
            public List<DroidController> Droids { get; } = new();
            public List<CharacterController> EnemiesOnStage { get; } = new();
        }
    }
}
