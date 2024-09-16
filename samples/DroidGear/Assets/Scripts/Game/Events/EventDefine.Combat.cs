using System;
using Game.Configs;
using Game.Framework;

namespace Game.Events
{

    public struct CombatStateChanged
    {
        public Type StateType;

        public CombatStateChanged(Type stateType)
        {
            StateType = stateType;
        }
    }

    public struct PlayerExpChanged
    {
        public int NewLevel;
        public int LevelUpDelta;
        public float NewExp;
        public float ExpDelta;
        public float NextLevelExp;
    }

    public struct DroidAdded
    {
        public CharacterConfig Config;
    }

    public struct DroidUpgraded
    {
        public CharacterConfig Config;
        public int Level;
    }

    public struct EnemySpotted
    {
        public Character Instigator;
        public Character Target;

        public EnemySpotted(Character instigator, Character target)
        {
            Instigator = instigator;
            Target = target;
        }
    }

    public struct PlayerDie
    {
    }
    
}