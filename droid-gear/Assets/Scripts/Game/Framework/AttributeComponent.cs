using System.Collections.Generic;
using UnityEngine;

namespace Game.Framework
{
    public class AttributeComponent : MonoBehaviour
    {

        public delegate void HealthChangeDelegate(AttributeComponent attrComp, float delta, float newHealth);
        public event HealthChangeDelegate HealthChanged;

        public Character Owner { get; private set; }
        private readonly Dictionary<AttributeType, Attribute> _attrDic = new Dictionary<AttributeType, Attribute>();

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
                if (_attrDic.TryGetValue(key, out var value))
                    return value;
                return _attrDic[key] = new Attribute();
            }
        }

        public void SetValue(AttributeType key, float value)
        {
            if (!_attrDic.TryGetValue(key, out var attr))
            {
                attr = new Attribute();
            }
            attr.Value = value;
            _attrDic[key] = attr;
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
                _attrDic[AttributeType.MaxHealth] = maxHealth;
                _attrDic[AttributeType.Health] = health;
            }
            else
            {
                SetValue(AttributeType.MaxHealth, maxHealthValue);
            }
        }

        public void ChangeHealth(float delta)
        {
            var health = this[AttributeType.Health];
            var newHealthValue = health.Value + delta;
            if (delta < 0)
            {
                newHealthValue = Mathf.Max(0, newHealthValue);
            }
            else if (delta > 0)
            {
                var maxHealth = this[AttributeType.MaxHealth];
                newHealthValue = Mathf.Min(newHealthValue, maxHealth.Value);
            }

            health.Value = newHealthValue;
            _attrDic[AttributeType.Health] = health;

            HealthChanged?.Invoke(this, delta, newHealthValue);
        }

        public void Revive()
        {
            var newHealthValue = _attrDic[AttributeType.MaxHealth].Value;
            SetValue(AttributeType.Health, newHealthValue);
            HealthChanged?.Invoke(this, 0, newHealthValue);
        }
        
    }
}