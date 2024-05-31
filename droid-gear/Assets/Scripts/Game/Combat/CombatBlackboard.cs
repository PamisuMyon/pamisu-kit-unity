using System;
using System.Collections.Generic;
using Game.Characters.Player;
using Game.Framework;

namespace Game.Combat
{
    [Serializable]
    public class CombatBlackboard
    {
        public float CombatTime;
        public int Level;
        public float Experience;
        public float NextLevelExperience;
        public PlayerController Player;
        public List<CharacterController> EnemiesOnStage = new();
    }
}