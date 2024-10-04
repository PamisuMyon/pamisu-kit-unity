using Cysharp.Threading.Tasks;
using Game.Characters;
using Game.Combat.States;
using Game.Configs;
using Game.Events;
using Game.Framework;
using PamisuKit.Common.Assets;
using PamisuKit.Common.FSM;
using PamisuKit.Common.Util;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Combat
{
    public class CombatSystem : MonoSystem
    {
        public LevelConfig LevelConfig;
        public HeroConfig PlayerConfig;
        
        [Space]
        public float CrystalDropChance = .6f;
        public AssetReferenceGameObject CrystalRef;

        public CombatStates.Blackboard Bb { get; private set; }
        public StateMachine Fsm { get; private set;}
        public CameraController Cam { get; internal set; }

        protected override void OnCreate()
        {
            base.OnCreate();
            Cam = FindFirstObjectByType<CameraController>();
            Bb = new CombatStates.Blackboard();
            Fsm = new StateMachine();
            Fsm.AddState(new CombatStates.Begin(this));
            Fsm.AddState(new CombatStates.Battle(this));
            Fsm.AddState(new CombatStates.End(this));
        }

        public void StartCombat()
        {
            Fsm.ChangeState<CombatStates.Begin>();
        }

        public void AddPlayerExp(float delta)
        {
            Bb.Experience += delta;
            // var level = Bb.PlayerLevel;
            var levelUpDelta = 0;
            while (Bb.Experience >= Bb.NextLevelExperience)
            {
                levelUpDelta++;
                // Level up
                Bb.PlayerLevel++;
                Bb.Experience -= Bb.NextLevelExperience;
                float increment;
                if (Bb.PlayerLevel >= 2 && Bb.PlayerLevel < 20)
                    increment = 10;
                else if (Bb.PlayerLevel >= 20 && Bb.PlayerLevel < 40)
                    increment = 13;
                else
                    increment = 16;
                Bb.NextLevelExperience += increment;
            }
            Emit(new PlayerExpChanged
            {
                NewLevel = Bb.PlayerLevel,
                LevelUpDelta = levelUpDelta,
                NewExp = Bb.Experience,
                ExpDelta = delta,
                NextLevelExp = Bb.NextLevelExperience
            });
        }
        
        public async UniTask AddDroid(CharacterConfig config)
        {
            RandomUtil.RandomPositionOnNavMesh(Bb.Player.Trans.position, 1f, 8f, out var pos);
            var prefab = await AssetManager.LoadAsset<GameObject>(config.PrefabRef);
            var go = Instantiate(prefab);
            var droid = go.GetComponent<DroidController>();
            droid.Setup(Region);
            droid.Init(config);
            droid.Trans.SetPositionAndRotation(pos, RandomUtil.RandomYRotation());
            Bb.Droids.Add(droid);
            
            Emit(new GearAdded { Config = config });
        }
        
    }
}