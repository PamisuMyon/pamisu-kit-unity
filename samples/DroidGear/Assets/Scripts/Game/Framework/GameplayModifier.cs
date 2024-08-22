
namespace Game.Framework
{
    public interface IGameplayModifier
    {
    }

    public abstract class CharacterModifier: IGameplayModifier
    {
        public abstract void Apply(Character owner);

        public abstract void Remove();
    }

}