using System;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Framework
{
    public class Unit : MonoEntity
    {
        [Header("Unit")]
        public string Id;
        public Vector2 VisualCenter = Vector2Int.zero;
        public Vector2 VisualSize = Vector2Int.one;

        public static string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}