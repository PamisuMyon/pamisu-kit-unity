using System;
using Game.Config.Tables;
using Game.Configs;
using PamisuKit.Framework;
using UnityEngine;
using CharacterConfig = Game.Configs.CharacterConfig;

namespace Game.Framework
{
    public class Character : MonoEntity
    {

        public event Action<Character> Die;
        public Action<Character> Died;

        public AttributeComponent AttrComp { get; protected set; }
        public AbilityComponent AbilityComp { get; protected set; }
        public Collider BodyCollider { get; protected set; }
        public CharacterModel Model { get; protected set; }
        public CharacterConfig Config { get; protected set; }
        public CharacterController Controller { get; internal set; }
        public bool IsAlive { get; protected set; }

        public virtual void Init(CharacterConfig config)
        {
            Config = config;
            var bodyColliderTrans = Trans.Find("BodyCollider");
            if (bodyColliderTrans != null)
                BodyCollider = bodyColliderTrans.GetComponent<Collider>();
            Model = GetComponentInChildren<CharacterModel>();

            AttrComp = GetComponent<AttributeComponent>();
            AttrComp.Init(this, config.AttributeDict);
            AttrComp.Revive();
            AttrComp.HealthChanged += OnHealthChanged;
            IsAlive = AttrComp[AttributeType.Health].Value > 0;

            AbilityComp = GetComponent<AbilityComponent>();
            AbilityComp.Init(this);
            // if (config.AttackAbility != null)
            //     AbilityComp.GrantAbility(AbilityFactory.Create(config.AttackAbility));
        }
        
        protected virtual void OnHealthChanged(AttributeComponent attrComp, float delta, float newHealth)
        {
            if (newHealth <= 0 && IsAlive)
            {
                IsAlive = false;
                Die?.Invoke(this);
            }
        }
        
        public void Revive()
        {
            AttrComp.Revive();
            IsAlive = AttrComp[AttributeType.Health].Value > 0;
        }

        public Ability GetAttackAbility()
        {
            if (AbilityComp.TryGetAbility(Config.AttackAbility.Id, out var attackAbility))
                return attackAbility;
            return null;
        }

    }
}
