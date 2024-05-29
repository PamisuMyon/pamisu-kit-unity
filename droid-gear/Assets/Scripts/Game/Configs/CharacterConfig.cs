using System.Collections.Generic;
using Game.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "Configs/CharacterConfig", order = 0)]
    public class CharacterConfig : ScriptableObject
    {
        [Header("Character")]
        public string Id;
        public AssetReferenceGameObject PrefabRef;
        public AbilityConfig AttackAbility;
        [Header("Attributes")]
        public float MaxHealth;
        public float MoveSpeed;
        public float Damage;
        public float AttackSpeed;

        public Dictionary<AttributeType, float> AttributeDict = new();

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