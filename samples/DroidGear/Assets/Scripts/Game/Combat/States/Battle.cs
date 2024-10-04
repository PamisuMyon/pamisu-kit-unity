using Cysharp.Threading.Tasks;
using Game.Characters.Monster;
using Game.Configs;
using Game.Events;
using Game.Framework;
using PamisuKit.Common;
using PamisuKit.Common.Util;
using UnityEngine;

namespace Game.Combat.States
{
    public static partial class CombatStates
    {
        public class Battle : Base
        {
            public Battle(CombatSystem owner) : base(owner)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                EventBus.Emit(new CombatStateChanged(typeof(Battle)));
                PerformLevel().Forget();
            }

            private async UniTaskVoid PerformLevel()
            {
                TickCombatTimer();
                if (Owner.LevelConfig == null)
                    return;
                var waves = Owner.LevelConfig.Waves;
                var enemyStarts = Object.FindObjectsByType<EnemyStart>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

                for (int i = 0; i < waves.Count; i++)
                {
                    PerformWave(i, waves[i], enemyStarts).Forget();
                    await Owner.Region.Ticker.Delay(waves[i].WaveDuration, Owner.destroyCancellationToken);
                }
                Debug.Log("All waves processed.");
            }

            private async void TickCombatTimer()
            {
                Bb.CombatTime = 0;
                do
                {
                    await UniTask.Yield(Owner.destroyCancellationToken);
                    Bb.CombatTime += Owner.Region.Ticker.DeltaTime;
                } while (Owner.destroyCancellationToken.IsCancellationRequested);
            }

            private async UniTask PerformWave(int index, WaveConfig wave, EnemyStart[] enemyStarts)
            {
                var enemyCount = Bb.EnemiesOnStage.Count;
                var pooler = Owner.GetDirector<GameDirector>().Pooler;
                for (int i = 0; i < wave.EnemyNum; i++)
                {
                    var enemyConfig = wave.Enemies.RandomItem();
                    var enemyStart = enemyStarts.RandomItem();
                    RandomUtil.RandomPositionOnNavMesh(enemyStart.transform.position, enemyStart.SpawnRadius, out var resultPos);

                    var controller = await pooler.Spawn<MonsterController>(enemyConfig.PrefabRef, -1, Owner.destroyCancellationToken);
                    controller.gameObject.SetActive(true);
                    controller.transform.SetPositionAndRotation(resultPos, RandomUtil.RandomYRotation());
                    controller.Setup(Owner.Region);
                    controller.Init(enemyConfig);
                    controller.ApplyGrowth(index + 1);
                    controller.Chara.Die += OnMonsterDie;
                    controller.Chara.Died += OnMonsterDied;

                    Bb.EnemiesOnStage.Add(controller);
                    enemyCount++;
                    if (Bb.EnemiesOnStage.Count > Owner.LevelConfig.EnemyLimit)
                    {
                        Debug.Log($"Enemy limit {Owner.LevelConfig.EnemyLimit} reached.");
                        return;
                    }

                    await Owner.Region.Ticker.Delay(wave.SpawnInterval, Owner.destroyCancellationToken);
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
                Owner.GetDirector<GameDirector>().Pooler.Release(controller);
            }

        }
    }
}
