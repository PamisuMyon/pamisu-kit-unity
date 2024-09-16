using Cysharp.Threading.Tasks;
using Game.Characters;
using Game.Characters.Player;
using Game.Events;
using PamisuKit.Common;
using PamisuKit.Common.Assets;
using PamisuKit.Common.Util;
using UnityEngine;

namespace Game.Combat.States
{
    public static partial class CombatStates
    {
        public class Begin : Base
        {
            public Begin(CombatSystem owner) : base(owner)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                EventBus.Emit(new CombatStateChanged(typeof(Begin)));
                DoInit().Forget();
            }

            private async UniTaskVoid DoInit()
            {
                Bb.PlayerLevel = 1;
                Bb.Experience = 0;
                Bb.NextLevelExperience = 5;

                await InitPlayer();
                await InitDroid();

                EventBus.Emit(new PlayerExpChanged
                {
                    NewLevel = Bb.PlayerLevel,
                    LevelUpDelta = 0,
                    NewExp = Bb.Experience,
                    ExpDelta = 0,
                    NextLevelExp = Bb.NextLevelExperience
                });

                Machine.ChangeState<Battle>();
            }

            private async UniTask InitPlayer()
            {
                if (Owner.PlayerConfig == null)
                    return;
                var playerStarts = GameObject.FindGameObjectsWithTag("PlayerStart");
                if (playerStarts == null || playerStarts.Length == 0)
                    return;

                var config = Owner.PlayerConfig;
                var playerStart = playerStarts.RandomItem().transform;
                var prefab = await AssetManager.LoadAsset<GameObject>(config.PrefabRef);
                var go = Object.Instantiate(prefab);
                var player = go.GetComponent<PlayerController>();
                player.Setup(Owner.Region);
                player.Init(config);
                player.Trans.SetPositionAndRotation(playerStart.position, playerStart.rotation);
                Bb.Player = player;

                Owner.Cam.Target = player.Trans;
            }

            // TODO TEMP
            private async UniTask InitDroid()
            {
                if (Owner.DroidConfig == null)
                    return;

                var config = Owner.DroidConfig;
                RandomUtil.RandomPositionOnNavMesh(Bb.Player.Trans.position, 1f, 8f, out var pos);
                var prefab = await AssetManager.LoadAsset<GameObject>(config.PrefabRef);
                var go = Object.Instantiate(prefab);
                var droid = go.GetComponent<DroidController>();
                droid.Setup(Owner.Region);
                droid.Init(config);
                droid.Trans.SetPositionAndRotation(pos, RandomUtil.RandomYRotation());
            }
        }
    }
}
