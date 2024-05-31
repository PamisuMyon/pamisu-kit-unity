﻿using Cysharp.Threading.Tasks;
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
        private string _autoInitCharacterId;

        public CharacterConfig Config => Chara.Config;
        public Character Chara { get; private set; }
        public CharacterModel Model => Chara.Model;

        private void Awake()
        {
            if (_autoInit && !string.IsNullOrEmpty(_autoInitCharacterId))
                AutoInit().Forget();
        }

        private async UniTaskVoid AutoInit()
        {
            Debug.Log($"{GetType().Name} AutoInit Id {_autoInitCharacterId}");
            if (!CombatDirector.IsReady)
                await UniTask.WaitUntil(() => CombatDirector.IsReady);

            if (!ConfigSystem.Instance.Characters.TryGetValue(_autoInitCharacterId, out var config))
            {
                Debug.LogError($"Character Id {_autoInitCharacterId} could not be found", Go);
                return;
            }

            SetupEntity(CombatDirector.Instance.Region);
            Init(config);
        }

        public virtual void Init(CharacterConfig config)
        {
            Chara = GetComponent<Character>();
            Chara.SetupEntity(Region);
            Chara.Init(config);
            Chara.Pooler = CombatDirector.Instance.Pooler;
        }

    }
}