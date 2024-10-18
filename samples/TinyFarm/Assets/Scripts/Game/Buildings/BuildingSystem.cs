using PamisuKit.Framework;
using UnityEngine;

namespace Game.Buildings
{
    public class BuildingSystem : MonoSystem
    {
        // TODO TEMP
        [SerializeField]
        private GameObject _warehouse;

        public GameObject Warehouse => _warehouse;

    }
}