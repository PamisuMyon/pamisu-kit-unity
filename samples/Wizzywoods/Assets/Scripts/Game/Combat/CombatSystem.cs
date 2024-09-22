using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Combat.Stage;
using Game.Combat.States;
using PamisuKit.Framework;
using PamisuKit.Common.FSM;
using PamisuKit.Common.Pool;
using UnityEngine;

namespace Game.Combat
{
    public class CombatSystem : MonoSystem
    {
        public CombatStageView StageView { get; private set; }
        public GameObjectPooler Pooler { get; private set; }
        public StateMachine Fsm { get; private set; }
        public CombatStates.Blackboard Bb { get; private set; }

        public void Init()
        {
            StageView = Object.FindObjectOfType<CombatStageView>();
            Pooler = new GameObjectPooler(Trans);
            Bb = new CombatStates.Blackboard();
            InitFsm();
        }

        private void InitFsm()
        {
            Fsm = new StateMachine();
            Fsm.AddState(new CombatStates.Init(this));
            Fsm.AddState(new CombatStates.PlayerTurn(this));
            Fsm.AddState(new CombatStates.EnemyTurn(this));
            Fsm.ChangeState<CombatStates.Init>();
        }

        internal async UniTask InitPlayer()
        {
            // TODO hard-coded
            // var config = ConfigSystem.Tables.TbCharacterConfig.Get("hero_mage");
            // Bb.Player = await EntityUtil.InstantiateMonoEntity<Hero>(config.PrefabRes, Region);
            // Bb.Player.Trans.position = StageView.PlayerStarts[0].position;
            // Bb.Player.Trans.rotation = StageView.PlayerStarts[0].rotation;
            // Bb.Player.Init(config);
            //
            // Bb.PlayerController = EntityUtil.NewMonoEntity<PlayerController>(Region);
            // Bb.PlayerController.Init(Bb.Player);
        }

        internal async UniTask LoadEnemies()
        {
            // TODO hard-coded
            // var config = ConfigSystem.Tables.TbCharacterConfig.Get("enemy_mushnub");
            // var mushnub = await EntityUtil.InstantiateMonoEntity<Enemy>(config.PrefabRes, Region);
            // mushnub.Trans.position = StageView.EnemyStarts[0].position;
            // mushnub.Trans.rotation = StageView.EnemyStarts[0].rotation;
            // mushnub.Init(config);
            // Bb.EnemiesOnStage.Add(mushnub);
            //
            // config = ConfigSystem.Tables.TbCharacterConfig.Get("enemy_mushnub_elite");
            // var mushnubElite = await EntityUtil.InstantiateMonoEntity<Enemy>(config.PrefabRes, Region);
            // mushnubElite.Trans.position = StageView.EnemyStarts[1].position;
            // mushnubElite.Trans.rotation = StageView.EnemyStarts[1].rotation;
            // mushnubElite.Init(config);
            // Bb.EnemiesOnStage.Add(mushnubElite);
        }

    }
}