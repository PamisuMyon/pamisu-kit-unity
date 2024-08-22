using System.Collections.Generic;
using UnityEngine;

namespace Game.Framework
{
    public class ModifierComponent : MonoBehaviour
    {
        public Character Owner { get; private set; }
        public List<IGameplayModifier> Modifiers { get; } = new();

        public void Init(Character owner)
        {
            Owner = owner;
        }

        public void AddModifier(IGameplayModifier gameplayModifier)
        {
            Modifiers.Add(gameplayModifier);
            if (gameplayModifier is CharacterModifier characterModifier)
                characterModifier.Apply(Owner);
        }

    }
}