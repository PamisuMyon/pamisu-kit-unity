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
        public float MaxHealth;
        public float MoveSpeed;
        public float Damage;
        public float AttackSpeed;

        public Dictionary<AttributeType, float> AttributeDic = new();

        public virtual void Init()
        {
            AttributeDic.Clear();
            AttributeDic[AttributeType.MaxHealth] = MaxHealth;
            AttributeDic[AttributeType.MoveSpeed] = MoveSpeed;
            AttributeDic[AttributeType.Damage] = Damage;
            AttributeDic[AttributeType.AttackSpeed] = AttackSpeed;
        }
    }
}