using System;
using System.Collections.Generic;
using Game.Framework;

namespace Game.Characters.Drone.States
{
    public static partial class DroneStates
    {
        [Serializable]
        public class Blackboard
        {
            public List<Character> Targets { get; internal set; } = new();
            public Character Target { get; internal set; }
        }
    }
}
