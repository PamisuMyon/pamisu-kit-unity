using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/LevelConfig", order = 0)]
    public class LevelConfig : ScriptableObject
    {
        public int Id;
        public int EnemyLimit = 200;
        public List<WaveConfig> Waves;
    }

    [Serializable]
    public class WaveConfig
    {
        public float WaveDuration = 30;
        public float SpawnInterval = .2f;
        public List<MonsterConfig> Enemies;
        public int EnemyNum;
    }
}
