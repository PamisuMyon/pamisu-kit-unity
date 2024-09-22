using Game.Configs;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Framework
{
    public abstract class CharacterController : MonoEntity
    {
        [SerializeField]
        private CharacterConfig _autoSetupConfig;

        public CharacterConfig Config => Chara.Config;
        public Character Chara { get; private set; }
        public CharacterModel Model => Chara.Model;
        public bool IsInitiated { get; protected set; }

        protected override void OnAutoSetup()
        {
            base.OnAutoSetup();
            Init(_autoSetupConfig);
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