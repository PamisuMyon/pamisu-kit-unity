namespace Game.Framework
{
    public interface IAttributeModifier
    {
        public float GetMultiplier(AttributeType attributeType);

        public float GetAddend(AttributeType attributeType);
        
    }
}