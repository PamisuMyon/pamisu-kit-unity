using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.Framework;

namespace Game.Abilities
{
    public class SprayAbility : Ability
    {
        public SprayAbility(AbilityConfig config) : base(config)
        {
        }

        protected override async UniTask DoActivate(CancellationToken cancellationToken)
        {
        }
        
    }
}