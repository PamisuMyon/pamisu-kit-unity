using Game.Framework;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Buildings
{
    public class BuildingSystem : MonoSystem
    {
        // TODO TEMP
        [SerializeField]
        private Unit _warehouse;

        [SerializeField]
        private Unit _well;
        
        public Unit Warehouse => _warehouse;
        public Unit Well => _well;

    }
}