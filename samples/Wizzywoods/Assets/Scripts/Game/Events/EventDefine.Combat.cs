using Game.Framework;

namespace Game.Events
{

    public struct CharacterHealthChanged
    {
        public Character Owner;
        public float Health;
        public float MaxHealth;
        public float Delta;
    }

    public struct RequestCharacterHealthInfoBroadcast
    {
    }

    public struct RequestActivatePlayerAbility
    {
        public string Id;
    }

    public struct RequestEndPlayerTurn
    {
    }

}