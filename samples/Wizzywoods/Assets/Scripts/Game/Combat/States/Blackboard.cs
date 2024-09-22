using System.Collections.Generic;
using Game.Framework;

namespace Game.Combat.States
{
    public static partial class CombatStates
    {
        public class Blackboard
        {
            public Character Player { get; internal set; }
            public PlayerController PlayerController { get; internal set; }
            public List<Character> EnemiesOnStage { get; internal set; } = new();
        }
    }

}