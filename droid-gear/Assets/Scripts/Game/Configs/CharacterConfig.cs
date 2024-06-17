using System.Collections.Generic;
using Game.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "Configs/CharacterConfig", order = 100)]
    public class CharacterConfig : ScriptableObject
    {
        [Header("Character")]
        public string Id;
        public AssetReferenceGameObject PrefabRef;
        public AbilityConfig AttackAbility;

        [Header("Attributes")]
        public float MaxHealth = 5f;
        public float MoveSpeed = 3f;
        public float Damage = 1f;
        public float AttackSpeed = 1f;

        public Dictionary<AttributeType, float> AttributeDict = new();

        [Header("UI Display")]
        public string DisplayName;
        public AssetReferenceSprite IconRef;
        

        public virtual void Init()
        {
            AttributeDict.Clear();
            AttributeDict[AttributeType.MaxHealth] = MaxHealth;
            AttributeDict[AttributeType.MoveSpeed] = MoveSpeed;
            AttributeDict[AttributeType.Damage] = Damage;
            AttributeDict[AttributeType.AttackSpeed] = AttackSpeed;
        }
    }
}
