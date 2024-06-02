using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Framework
{
    public class AbilityComponent : MonoBehaviour, IUpdatable
    {

        private const string Tag = "AbilityComponent";

        private readonly Dictionary<string, Ability> _abilityDic = new();

        public bool IsActive => Owner.IsActive;

        public Character Owner { get; private set; }
        public List<Ability> Abilities { get; } = new();
        
        public void Init(Character owner)
        {
            Owner = owner;
        }
        
        public void OnUpdate(float deltaTime)
        {
            for (var i = 0; i < Abilities.Count; i++)
            {
                // Cooldown
                if (Abilities[i].IsCooldownManaged
                    && Abilities[i].State == AbilityState.Inactive
                    && Abilities[i].Cooldown > 0)
                {
                    Abilities[i].Cooldown -= deltaTime;
                }
            }
        }
        
        public bool GrantAbility(Ability ability)
        {
            var b = _abilityDic.TryAdd(ability.Config.Id, ability);
            if (!b)
            {
                Debug.LogWarning($"{Tag} Try to grant ability that already exists: {ability.Config.Id}");
                return false;
            }
            Abilities.Add(ability);
            ability.OnGranted(this);
            return true;
        }

        public bool RevokeAbility(Ability ability)
        {
            var b = _abilityDic.Remove(ability.Config.Id);
            if (b)
            {
                Abilities.Remove(ability);
                ability.OnRevoked();
            }
            return b;
        }

        public bool TryGetAbility(string id, out Ability ability)
        {
            return _abilityDic.TryGetValue(id, out ability);
        }

        public UniTask<bool> TryActiveAbility(string id, CancellationToken cancellationToken = default)
        {
            if (_abilityDic.TryGetValue(id, out var ability))
            {
                if (cancellationToken == default)
                    cancellationToken = destroyCancellationToken;
                return ability.TryActivate(cancellationToken);
            }
            Debug.LogWarning($"{Tag} Try activate ability that not exists: {id}");
            return UniTask.FromResult(false);
        }

        public void ResetAbilities()
        {
            for (int i = 0; i < Abilities.Count; i++)
            {
                Abilities[i].Cancel();
                Abilities[i].Cooldown = 0;
            }
        }
        
    }
}