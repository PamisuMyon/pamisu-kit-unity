using Game.Framework;

namespace Game.Characters.Monster.States
{
    public static partial class MonsterStates
    {
        public class Blackboard
        {
            public Character Target { get; internal set; }
            public Ability AttackAbility { get; internal set; }
        }
    }
}