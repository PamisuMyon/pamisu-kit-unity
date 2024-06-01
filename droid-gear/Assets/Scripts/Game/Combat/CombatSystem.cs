using Cysharp.Threading.Tasks;
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

        public CombatBlackboard Bb { get; private set; }

        protected override void OnCreate()
        {
            base.OnCreate();
            Bb = new CombatBlackboard();
        }

        public async UniTaskVoid StartCombat()
        {
            await InitPlayer();
            PerformLevel().Forget();
        }

        protected async UniTask InitPlayer()
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
            var prefab = await AssetManager.LoadAsset<GameObject>(config.PrefabRef.RuntimeKey.ToString());
            var go = Instantiate(prefab);
            var player = go.GetComponent<PlayerController>();
            player.Setup(Region);
            player.Init(config);
            player.Trans.SetPositionAndRotation(playerStart.position, playerStart.rotation);
            Bb.Player = player;
        }

        private async UniTaskVoid PerformLevel()
        {
            TickCombatTimer();
            var waves = _levelConfig.Waves;
            var enemyStarts = FindObjectsByType<EnemyStart>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            for (int i = 0; i < waves.Count; i++)
            {
                await PerformWave(waves[i], enemyStarts);
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
                if (!RandomUtil.RandomPositionOnNavMesh(enemyStart.transform.position, enemyStart.SpawnRadius, out var resultPos))
                    resultPos = enemyStart.transform.position;

                var controller = await pooler.Spawn<MonsterController>(enemyConfig.PrefabRef.RuntimeKey.ToString());
                controller.transform.SetPositionAndRotation(resultPos, RandomUtil.RandomYRotation());
                controller.Setup(Region);
                controller.Init(enemyConfig);
                controller.gameObject.SetActive(true);
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