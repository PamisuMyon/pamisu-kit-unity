using System;
using Cysharp.Threading.Tasks;
using Game.Abilities;
using Game.Configs;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Framework
{
    public class Character : MonoEntity, IUpdatable
    {

        public event Action<Character> Die;
        public Action<Character> Died;

        public AttributeComponent AttrComp { get; protected set; }
        public AbilityComponent AbilityComp { get; protected set; }
        public Collider BodyCollider { get; protected set; }
        public CharacterModel Model { get; protected set; }
        public CharacterConfig Config { get; protected set; }
        public CharacterController Controller { get; internal set; }
        // public MonoPooler Pooler { get; internal set; }
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
            if (config.AttackAbility != null)
                AbilityComp.GrantAbility(AbilityFactory.Create(config.AttackAbility));
                
            // EventBus.Emit(new CharacterHealthChanged(this, AttrComp[AttributeType.Health].Value, AttrComp[AttributeType.MaxHealth].Value, 0f));
        }

        public void OnUpdate(float deltaTime)
        {
            AbilityComp.OnUpdate(deltaTime);
        }

        protected virtual void OnHealthChanged(AttributeComponent attrComp, float delta, float newHealth)
        {
            if (newHealth <= 0 && IsAlive)
            {
                IsAlive = false;
                Die?.Invoke(this);
            }
            // EventBus.Emit(new CharacterHealthChanged(this, newHealth, attrComp[AttributeType.MaxHealth].Value,delta));
        }
        
        public virtual async UniTask Revive()
        {
            await Region.Ticker.Delay(2f, destroyCancellationToken);
            AttrComp.Revive();
            IsAlive = AttrComp[AttributeType.Health].Value > 0;
        }

    }
}
