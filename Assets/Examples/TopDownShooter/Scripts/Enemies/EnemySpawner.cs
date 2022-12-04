using System.Collections;
using System.Collections.Generic;
using Pamisu.Commons;
using Pamisu.Commons.Pool;
using Pamisu.Gameplay;
using UnityEngine;

namespace Pamisu.TopDownShooter.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {

        [SerializeField]
        private float spawnHeight = 20f;
        [SerializeField]
        private EnemyWave[] waves;
        [SerializeField]
        private float spawnRadius;
        [SerializeField]
        private TriggerArea battleTriggerArea;
        
        private List<EnemyController> activeEnemies;
        private int currentWaveIndex;

        private void Start()
        { 
            foreach (var it in activeEnemies)
            {
                it.Spawner = this;
            }

            battleTriggerArea.TriggerEnter += OnBattleTriggerAreaEnter;
        }

        private void OnBattleTriggerAreaEnter(Collider obj)
        {
            if (obj.CompareTag("Player"))
            {
                battleTriggerArea.TriggerEnter -= OnBattleTriggerAreaEnter;
                battleTriggerArea.gameObject.SetActive(false);
            }
        }

        private IEnumerator ProcessCurrentWave()
        {
            if (currentWaveIndex > waves.Length - 1)
                yield break;

            var wave = waves[currentWaveIndex];
            yield return new WaitForSeconds(wave.StartDelay);

            var spawns = wave.AltEnemySpawns.RandomItem();
            foreach (var it in spawns.List)
            {
                var b = UnityUtil.RandomPointOnNavMesh(transform.position, spawnRadius, out var pos);
                if (!b)
                {
                    Debug.Log("Enemy Spawn: Get random walkable position failed.");
                    continue;
                }
                var go = GameObjectPooler.Spawn(it, pos, RandomUtil.RandomYRotation());
                var enemy = go.GetComponent<EnemyController>();
                enemy.Spawn(spawnHeight, pos);
                activeEnemies.Add(enemy);
                yield return new WaitForSeconds(wave.SpawnInterval);
            }
        }

        public void OnEnemyKilled(EnemyController enemy)
        {
            if (!activeEnemies.Contains(enemy)) return;
            activeEnemies.Remove(enemy);
            if (activeEnemies.Count == 0)
            {
                // Next wave
                currentWaveIndex++;
                StartCoroutine(ProcessCurrentWave());
            }
        }
        
    }
}