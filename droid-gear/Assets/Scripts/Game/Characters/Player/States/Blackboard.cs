using System;
using System.Collections.Generic;
using Game.Framework;

namespace Game.Characters.Player.States
{
    public static partial class PlayerStates
    {
        [Serializable]
        public class Blackboard
        {
            public List<Character> Targets { get; internal set; } = new();
            public Character Target { get; internal set; }
        }
    }
}
