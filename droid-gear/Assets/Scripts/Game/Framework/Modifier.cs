
namespace Game.Framework
{
    public interface IModifier
    {
    }

    public abstract class CharacterModifier: IModifier
    {
        public abstract void Apply(Character owner);

        public abstract void Remove();
    }

}