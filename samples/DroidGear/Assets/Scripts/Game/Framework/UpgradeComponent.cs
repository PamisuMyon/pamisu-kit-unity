using System.Collections.Generic;
using UnityEngine;

namespace Game.Framework
{
    public class UpgradeComponent : MonoBehaviour
    {
        public Character Owner { get; private set; }
        public List<Upgrade> Upgrades { get; } = new();

        public void Init(Character owner)
        {
            Owner = owner;
        }

        public void ApplyUpgrade(Upgrade upgrade)
        {
            Upgrades.Add(upgrade);
            upgrade.OnApplied(this);
        }

        public void RemoveUpgrade(Upgrade upgrade)
        {
            Upgrades.Remove(upgrade);
            upgrade.OnRemoved();
        }

    }
}