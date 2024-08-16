using Game.Framework;
using UnityEngine;

namespace Game.Events
{

    #region Combat

    public struct RequestShowFloatingText
    {
        public Vector3 WorldPos;
        public string Content;
    }

    public struct RequestShowDamageText
    {
        public Vector3 WorldPos;
        public Damage Damage;
    }

    #endregion
}