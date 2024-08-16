using System.Collections.Generic;
using UnityEngine;

namespace Game.Framework
{
    public class ModifierComponent : MonoBehaviour
    {
        public Character Owner { get; private set; }
        public List<IModifier> Modifiers { get; } = new();

        public void Init(Character owner)
        {
            Owner = owner;
        }

        public void AddModifier(IModifier modifier)
        {
            Modifiers.Add(modifier);
            if (modifier is CharacterModifier characterModifier)
                characterModifier.Apply(Owner);
        }

    }
}