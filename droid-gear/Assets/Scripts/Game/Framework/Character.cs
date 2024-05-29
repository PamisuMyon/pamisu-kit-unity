using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.Events;
using PamisuKit.Common;
using PamisuKit.Common.Pool;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Framework
{
    public class Character : MonoEntity, IUpdatable
    {
        public AttributeComponent AttrComp { get; protected set; }
        public AbilityComponent AbilityComp { get; protected set; }
        public CharacterModel Model { get; protected set; }
        public Rigidbody Rb { get; protected set; }
        public CharacterController Controller { get; protected set; }
        public CharacterConfig Config { get; protected set; }
        public MonoPooler Pooler { get; internal set; }
        
        public bool IsAlive => AttrComp[AttributeType.Health].Value > 0;

        public virtual void Init(CharacterConfig config)
        {
            Config = config;
            
            Model = GetComponentInChildren<CharacterModel>();
            Rb = GetComponent<Rigidbody>();

            AttrComp = GetComponent<AttributeComponent>();
            AttrComp.Init(this, config.AttributeDict);
            AttrComp.Revive();
            AttrComp.HealthChanged += OnHealthChanged;

            AbilityComp = GetComponent<AbilityComponent>();
            AbilityComp.Init(this);
            // if (config.AttackAbility != null)
            //     AbilityComp.GrantAbility(AbilityFactory.Create(config.AttackAbility));
                
            EventBus.Emit(new CharacterHealthChanged(this, AttrComp[AttributeType.Health].Value, AttrComp[AttributeType.MaxHealth].Value, 0f));
        }

        public void OnUpdate(float deltaTime)
        {
            AbilityComp.OnUpdate(deltaTime);
        }

        protected virtual void OnHealthChanged(AttributeComponent attrComp, float delta, float newHealth)
        {
            EventBus.Emit(new CharacterHealthChanged(this, newHealth, attrComp[AttributeType.MaxHealth].Value,delta));
            // if (delta < 0)
            // {
            //     EventBus.Emit(new RequestShowFloatingText
            //     {
            //         WorldPos = Trans.position,
            //         Content = $"{delta}"
            //     });
            // }
        }
        
        public virtual async UniTask Revive()
        {
            await UniTask.Delay(2000);
            AttrComp.Revive();
        }

    }
}
