using Cysharp.Threading.Tasks;
using Game.Characters;
using Game.Characters.Player;
using Game.Configs;
using Game.Framework;
using PamisuKit.Common.Assets;
using PamisuKit.Common.Pool;
using PamisuKit.Common.Util;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game.Combat
{
    public class CombatDirector : Director<CombatDirector>
    {
        public static bool IsReady => Instance != null && Instance._isReady;
        private bool _isReady;

        [SerializeField]
        private LevelConfig _levelConfig;
        [SerializeField]
        private string _playerId;

        public MonoPooler Pooler { get; private set; }
        public CombatBlackboard Bb { get; private set; }

        protected override void OnCreate()
        {
            base.OnCreate();
            Init().Forget();
        }

        protected async UniTaskVoid Init()
        {
            if (!GlobalDirector.IsGlobalSystemsReady)
                await UniTask.WaitUntil(() => GlobalDirector.IsGlobalSystemsReady);
            
            Pooler = new MonoPooler(Region.Trans);
            Bb = new CombatBlackboard();
            _isReady = true;

            await InitPlayer();
            PerformLevel().Forget();
        }

#if UNITY_EDITOR
        protected override void Update()
        {
            base.Update();
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                Debug.Log("Reload current scene");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            if (Keyboard.current.iKey.wasPressedThisFrame)
            {
                Ticker.TimeScale = Mathf.Min(2, Ticker.TimeScale + 0.1f);
                Debug.Log($"Ticker timescale: {Ticker.TimeScale}");
            }
            if (Keyboard.current.kKey.wasPressedThisFrame)
            {
                Ticker.TimeScale = Mathf.Max(0, Ticker.TimeScale - 0.1f);
                Debug.Log($"Ticker timescale: {Ticker.TimeScale}");
            }
        }
#endif

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
            player.SetupEntity(Region, false);
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
                Bb.CombatTime += Ticker.DeltaTime;
            } while (destroyCancellationToken.IsCancellationRequested);
        }

        private async UniTask PerformWave(WaveConfig wave, EnemyStart[] enemyStarts)
        {
            var enemyCount = Bb.EnemiesOnStage.Count;
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
                
                var controller = await Pooler.Spawn<MonsterController>(enemyConfig.PrefabRef.RuntimeKey.ToString());
                controller.transform.SetPositionAndRotation(resultPos, RandomUtil.RandomYRotation());
                controller.SetupEntity(Region);
                controller.Init(enemyConfig);
                controller.Chara.Die += OnMonsterDie;

                Bb.EnemiesOnStage.Add(controller);
                enemyCount++;
                if (Bb.EnemiesOnStage.Count > _levelConfig.EnemyLimit)
                {
                    Debug.Log($"Enemy limit {_levelConfig.EnemyLimit} reached.");
                    return;
                }

                await Ticker.Delay(wave.SpawnInterval, destroyCancellationToken);
            }
        }

        private void OnMonsterDie(Character monster)
        {
            monster.Die -= OnMonsterDie;
            Bb.EnemiesOnStage.Remove(monster.Controller);
            Pooler.Release(monster.Controller);
        }

    }
}