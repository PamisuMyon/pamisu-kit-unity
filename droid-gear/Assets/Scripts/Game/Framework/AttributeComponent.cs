using System.Collections.Generic;
using UnityEngine;

namespace Game.Framework
{
    public class AttributeComponent : MonoBehaviour
    {

        public delegate void HealthChangeDelegate(AttributeComponent attrComp, float delta, float newHealth);
        public event HealthChangeDelegate HealthChanged;

        public Character Owner { get; private set; }
        private readonly Dictionary<AttributeType, Attribute> _attrDict = new();

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
                return _attrDict[key] = new Attribute();
            }
        }

        public void SetValue(AttributeType key, float value)
        {
            if (!_attrDict.TryGetValue(key, out var attr))
            {
                attr = new Attribute();
            }
            attr.Value = value;
            _attrDict[key] = attr;
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