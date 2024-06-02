using Game.Framework;

namespace Game.Events
{

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