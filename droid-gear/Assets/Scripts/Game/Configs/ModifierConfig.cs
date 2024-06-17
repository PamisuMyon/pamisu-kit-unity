using UnityEngine;

namespace Game.Configs
{
    public enum ModifierType
    {
        None,
        Drone_Damage,
    }

    [CreateAssetMenu(fileName = "ModifierConfig", menuName = "Configs/ModifierConfig", order = 0)]
    public class ModifierConfig : ScriptableObject
    {
        [Header("Modifier")]
        public ModifierType Type;

        // [Header("UI Display")]
        
        public struct AttributePair
        {
            public string Name;
        }
    }
}
