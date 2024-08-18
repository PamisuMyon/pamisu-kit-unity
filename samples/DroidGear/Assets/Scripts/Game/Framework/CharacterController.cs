using Cysharp.Threading.Tasks;
using Game.Combat;
using Game.Configs;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Framework
{
    public abstract class CharacterController : MonoEntity
    {
        [SerializeField]
        private bool _autoInit = false;
        [SerializeField]
        private CharacterConfig _autoInitConfig;

        public CharacterConfig Config => Chara.Config;
        public Character Chara { get; private set; }
        public CharacterModel Model => Chara.Model;
        public bool IsInitiated { get; protected set; }

        private void Awake()
        {
            if (_autoInit && _autoInitConfig != null)
                AutoInit().Forget();
        }

        private async UniTaskVoid AutoInit()
        {
            Debug.Log($"{GetType().Name} AutoInit {_autoInitConfig.Id}");
            var combatDirector = FindFirstObjectByType<CombatDirector>();
            if (!combatDirector.IsReady)
                await UniTask.WaitUntil(() => combatDirector.IsReady);

            Setup(combatDirector.Region);
            Init(_autoInitConfig);
        }

        public virtual void Init(CharacterConfig config)
        {
            if (IsInitiated)
                return;
            Chara = GetComponent<Character>();
            Chara.Setup(Region);
            Chara.Init(config);
            Chara.Controller = this;
            Chara.Die += OnDie;
            
            IsInitiated = true;
        }

        protected virtual void OnDie(Character character)
        {
        }
        
    }
}