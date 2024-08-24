using System.Collections.Generic;
using UnityEngine;

namespace Game.Framework
{
    public class UpgradeComponent : MonoBehaviour
    {
        public Character Owner { get; private set; }
        public List<Upgrade> Modifiers { get; } = new();

        public void Init(Character owner)
        {
            Owner = owner;
        }

        public void ApplyModifier(Upgrade modifier)
        {
            Modifiers.Add(modifier);
        }

        public void RemoveModifier(Upgrade modifier)
        {
            Modifiers.Remove(modifier);
        }

    }
}