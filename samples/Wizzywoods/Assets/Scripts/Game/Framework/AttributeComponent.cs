using System.Collections.Generic;
using UnityEngine;

namespace Game.Framework
{
    public class AttributeComponent : MonoBehaviour
    {
        
        private readonly Dictionary<AttributeType, Attribute> _attrDict = new();
        private readonly List<AttributeType> _attrKeys = new();
        
        public Character Owner { get; private set; }
        public List<IAttributeModifier> Modifiers { get; private set; } = new();
        
        public delegate void HealthChangeDelegate(AttributeComponent attrComp, float delta, float newHealth);
        public event HealthChangeDelegate HealthChanged;

        public void Init(Character owner, Dictionary<AttributeType, float> valueDic = null)
        {
            Owner = owner;
            if (valueDic != null)
            {
                foreach (var pair in valueDic)
                {
                    SetValue(pair.Key, pair.Value);
                }
            }
        }
        
        public Attribute this[AttributeType key]
        {
            get
            {
                if (_attrDict.TryGetValue(key, out var value))
                    return value;
                _attrKeys.Add(key);
                return _attrDict[key] = new Attribute();
            }
        }

        public void SetValue(AttributeType key, float value)
        {
            if (!_attrDict.TryGetValue(key, out var attr))
            {
                attr = new Attribute();
                _attrKeys.Add(key);
            }
            attr.BaseValue = attr.Value = value;
            _attrDict[key] = attr;
        }

        public void AddModifier(IAttributeModifier modifier)
        {
            Modifiers.Add(modifier);
        }

        public void RemoveModifier(IAttributeModifier modifier)
        {
            Modifiers.Remove(modifier);
        }

        public void Refresh()
        {
            for (int i = 0; i < _attrKeys.Count; i++)
            {
                var key = _attrKeys[i];
                var multiplier = 1f;
                var addend = 0f;
                for (int j = 0; j < Modifiers.Count; j++)
                {
                    multiplier += Modifiers[j].GetMultiplier(key);
                    addend += Modifiers[j].GetAddend(key);
                }
                multiplier = Mathf.Max(0, multiplier);
                var attr = _attrDict[key];
                attr.Value = attr.BaseValue * multiplier + addend;
                _attrDict[key] = attr;
                // ...To be implemented: Attribute value restriction
            }
        }

        public void SetMaxHealth(float maxHealthValue, bool updateHealth = false)
        {
            var maxHealth = this[AttributeType.MaxHealth];
            if (updateHealth && maxHealth.Value != 0)
            {
                var health = this[AttributeType.Health];
                var ratio = health.Value / maxHealth.Value;
                maxHealth.Value = maxHealthValue;
                health.Value = maxHealth.Value * ratio;
                _attrDict[AttributeType.MaxHealth] = maxHealth;
                _attrDict[AttributeType.Health] = health;
            }
            else
            {
                SetValue(AttributeType.MaxHealth, maxHealthValue);
            }
        }

        public void ChangeHealth(Damage damage)
        {
            var health = this[AttributeType.Health];
            var newHealthValue = health.Value + damage.Value;
            if (damage.Value < 0)
            {
                newHealthValue = Mathf.Max(0, newHealthValue);
            }
            else if (damage.Value > 0)
            {
                var maxHealth = this[AttributeType.MaxHealth];
                newHealthValue = Mathf.Min(newHealthValue, maxHealth.Value);
            }

            health.Value = newHealthValue;
            _attrDict[AttributeType.Health] = health;

            HealthChanged?.Invoke(this, damage.Value, newHealthValue);
        }

        public void Revive()
        {
            var newHealthValue = _attrDict[AttributeType.MaxHealth].Value;
            SetValue(AttributeType.Health, newHealthValue);
            HealthChanged?.Invoke(this, 0, newHealthValue);
        }
        
    }
}