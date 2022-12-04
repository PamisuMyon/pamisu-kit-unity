using System;
using UnityEngine;

namespace Pamisu.TopDownShooter.Enemies
{
    [CreateAssetMenu(fileName = "EnemyWaveConfig", menuName = "Go22/Enemy Wave Config")]
    public class EnemyWave : ScriptableObject
    {
        public float StartDelay;
        public float SpawnInterval;
        public EnemySpawnList[] AltEnemySpawns;
        public GameObject TreasurePrefab;
    }

    [Serializable]
    public class EnemySpawnList
    {
        public GameObject[] List;
    }
    
}