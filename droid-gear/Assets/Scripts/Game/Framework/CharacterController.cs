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
        private string _autoInitCharacterId;

        public CharacterConfig Config => Chara.Config;
        public Character Chara { get; private set; }
        public CharacterModel Model => Chara.Model;
        public bool IsInitiated { get; protected set; }

        // private void Awake()
        // {
        //     if (_autoInit && !string.IsNullOrEmpty(_autoInitCharacterId))
        //         AutoInit().Forget();
        // }

        // private async UniTaskVoid AutoInit()
        // {
        //     Debug.Log($"{GetType().Name} AutoInit Id {_autoInitCharacterId}");
        //     if (!CombatDirector.IsReady)
        //         await UniTask.WaitUntil(() => CombatDirector.IsReady);

        //     if (!ConfigSystem.Instance.Characters.TryGetValue(_autoInitCharacterId, out var config))
        //     {
        //         Debug.LogError($"Character Id {_autoInitCharacterId} could not be found", Go);
        //         return;
        //     }

        //     // SetupEntity(CombatDirector.Instance.Region);
        //     Init(config);
        // }

        public virtual void Init(CharacterConfig config)
        {
            if (IsInitiated)
                return;
            Chara = GetComponent<Character>();
            Chara.Setup(Region);
            Chara.Init(config);
            Chara.Controller = this;
            // Chara.Pooler = CombatSystem.Instance.Pooler;
            Chara.Die += OnDie;
            
            IsInitiated = true;
        }

        protected virtual void OnDie(Character character)
        {
        }
        
    }
}