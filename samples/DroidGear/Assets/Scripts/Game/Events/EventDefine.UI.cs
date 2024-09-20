using System.Collections.Generic;
using Game.Framework;
using Game.Upgrades;
using UnityEngine;

namespace Game.Events
{

    #region Combat

    public struct RequestShowFloatingText
    {
        public Vector3 WorldPos;
        public string Content;
    }

    public struct ReqShowDamageText
    {
        public Vector3 WorldPos;
        public Damage Damage;
    }

    public struct ReqShowUpgradeItems
    {
        public List<UpgradeItem> Items;
    }

    #endregion
}