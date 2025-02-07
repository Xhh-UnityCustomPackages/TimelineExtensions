using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Timeline
{
    [Serializable]
    public class GenerateSetting
    {
        public enum GenerateType
        {
            //间隔多少秒,一次生成一个
            Interval,
            Formation,
        }

        public List<int> id = new() { 0 };

        public GenerateType generateType = GenerateType.Interval;
        [InspectorName("数量")] public int count = 10;
        [InspectorName("间隔")] [Min(0f)] public float interval = 0.2f;

        [InspectorName("阵型")] public FormationConfig formationConfig = null;
    }


    [Serializable]
    public class EnemySpawnClipAsset : BaseClipAsset<EnemySpawnBehaviour>
    {
        public int spawnID;

        public List<GenerateSetting> GenerateSettings = new();

        public int totalCount
        {
            get
            {
                int _totalCount = 0;
                foreach (var setting in GenerateSettings)
                {
                    _totalCount += setting.count;
                }

                return _totalCount;
            }
        }
    }
}