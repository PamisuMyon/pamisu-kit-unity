using Cysharp.Threading.Tasks;
using Game.Characters;
using Game.Characters.Monster;
using Game.Characters.Player;
using Game.Configs;
using Game.Framework;
using PamisuKit.Common.Assets;
using PamisuKit.Common.Util;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Combat
{
    public class CombatSystem : MonoSystem<CombatSystem>
    {
        [SerializeField]
        private LevelConfig _levelConfig;
        [SerializeField]
        private string _playerId;
        [SerializeField]
        private string _droidId;

        public CombatBlackboard Bb { get; private set; }
        public CameraController Cam { get; private set; }

        protected override void OnCreate()
        {
            base.OnCreate();
            Bb = new CombatBlackboard();
        }

        public async UniTaskVoid StartCombat()
        {
            await InitPlayer();
            await InitDroid();
            PerformLevel().Forget();
        }

        private async UniTask InitPlayer()
        {
            if (string.IsNullOrEmpty(_playerId))
                return;
            var playerStarts = GameObject.FindGameObjectsWithTag("PlayerStart");
            if (playerStarts == null || playerStarts.Length == 0)
                return;

            if (!ConfigSystem.Instance.Characters.TryGetValue(_playerId, out var config))
            {
                Debug.LogError($"Player config of Id {_playerId} not found");
                return;
            }

            var playerStart = playerStarts.RandomItem().transform;
            var prefab = await AssetManager.LoadAsset<GameObject>(config.PrefabRef);
            var go = Instantiate(prefab);
            var player = go.GetComponent<PlayerController>();
            player.Setup(Region);
            player.Init(config);
            player.Trans.SetPositionAndRotation(playerStart.position, playerStart.rotation);
            Bb.Player = player;

            Cam = FindFirstObjectByType<CameraController>();
            Cam.Target = player.Trans;
        }

        // TODO TEMP
        private async UniTask InitDroid()
        {
            if (string.IsNullOrEmpty(_droidId))
                return;
            if (!ConfigSystem.Instance.Characters.TryGetValue(_droidId, out var config))
            {
                Debug.LogError($"Droid config of Id {_droidId} not found");
                return;
            }

            RandomUtil.RandomPositionOnNavMesh(Bb.Player.Trans.position, 1f, 8f, out var pos);
            var prefab = await AssetManager.LoadAsset<GameObject>(config.PrefabRef);
            var go = Instantiate(prefab);
            var droid = go.GetComponent<DroidController>();
            droid.Setup(Region);
            droid.Init(config);
            droid.Trans.SetPositionAndRotation(pos, RandomUtil.RandomYRotation());
        }

        private async UniTaskVoid PerformLevel()
        {
            TickCombatTimer();
            if (_levelConfig == null)
                return;
            var waves = _levelConfig.Waves;
            var enemyStarts = FindObjectsByType<EnemyStart>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            for (int i = 0; i < waves.Count; i++)
            {
                PerformWave(waves[i], enemyStarts).Forget();
                await Region.Ticker.Delay(waves[i].WaveDuration, destroyCancellationToken);
            }
            Debug.Log("All waves processed.");
        }

        private async void TickCombatTimer()
        {
            Bb.CombatTime = 0;
            do
            {
                await UniTask.Yield(destroyCancellationToken);
                Bb.CombatTime += Region.Ticker.DeltaTime;
            } while (destroyCancellationToken.IsCancellationRequested);
        }

        private async UniTask PerformWave(WaveConfig wave, EnemyStart[] enemyStarts)
        {
            var enemyCount = Bb.EnemiesOnStage.Count;
            var pooler = GetDirector<GameDirector>().Pooler;
            for (int i = 0; i < wave.EnemyNum; i++)
            {
                var enemyId = wave.EnemyIds.RandomItem();
                if (!ConfigSystem.Instance.Characters.TryGetValue(enemyId, out var enemyConfig))
                {
                    Debug.LogError($"Character config of Id {enemyId} not found.");
                    continue;
                }

                var enemyStart = enemyStarts.RandomItem();
                RandomUtil.RandomPositionOnNavMesh(enemyStart.transform.position, enemyStart.SpawnRadius, out var resultPos);

                var controller = await pooler.Spawn<MonsterController>(enemyConfig.PrefabRef.RuntimeKey.ToString());
                controller.gameObject.SetActive(true);
                controller.transform.SetPositionAndRotation(resultPos, RandomUtil.RandomYRotation());
                controller.Setup(Region);
                controller.Init(enemyConfig);
                controller.Chara.Die += OnMonsterDie;
                controller.Chara.Died += OnMonsterDied;

                Bb.EnemiesOnStage.Add(controller);
                enemyCount++;
                if (Bb.EnemiesOnStage.Count > _levelConfig.EnemyLimit)
                {
                    Debug.Log($"Enemy limit {_levelConfig.EnemyLimit} reached.");
                    return;
                }

                await Region.Ticker.Delay(wave.SpawnInterval, destroyCancellationToken);
            }
        }

        private void OnMonsterDie(Character monster)
        {
            monster.Die -= OnMonsterDie;
            Bb.EnemiesOnStage.Remove(monster.Controller);
        }

        private void OnMonsterDied(Character monster)
        {
            monster.Die -= OnMonsterDied;
            var controller = monster.Controller as MonsterController;
            controller.gameObject.SetActive(false);
            GetDirector<GameDirector>().Pooler.Release(controller);
        }

    }
}