using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Timeline
{
    public enum GenerateType
    {
        //间隔多少秒,一次生成一个
        Interval,
    }

    [Serializable]
    public class GenerateSetting
    {
        public List<int> id = new() { 0 };

        // public GenerateType generateType = GenerateType.Interval;
        [InspectorName("数量")] public int count = 10;
        [InspectorName("间隔")] public float interval = 0.2f;
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