namespace Game.Framework
{
    /// <summary>
    /// Damage and healing
    /// </summary>
    public struct Damage
    {
        public Character Instigator;
        public float Value;
        public bool IsCritical;

        public Damage(Character instigator, float value, bool isCritical = false)
        {
            Instigator = instigator;
            Value = value;
            IsCritical = isCritical;
        }
    }
}