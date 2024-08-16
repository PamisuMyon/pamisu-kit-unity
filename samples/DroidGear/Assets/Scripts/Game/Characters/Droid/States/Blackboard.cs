using System.Collections.Generic;
using Game.Framework;

namespace Game.Characters.Droid.States
{
    public static partial class DroidStates
    {
        public class Blackboard
        {
            public Character Player { get; internal set; }
            public Character Target { get; internal set; }
            public List<Character> Targets { get; internal set; } = new();
            public Ability AttackAbility { get; internal set; }
            public bool ShouldReturnToPlayer;
        }
    }
}