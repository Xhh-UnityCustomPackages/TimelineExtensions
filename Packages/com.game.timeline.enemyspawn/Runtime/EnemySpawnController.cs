using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Timeline
{
    public class EnemySpawnController
    {
        private static EnemySpawnController _Instance;

        public static EnemySpawnController S
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new();
                }

                return _Instance;
            }
        }

        public delegate GameObject Generate(GameObject prefab, int spawnID);

        public Generate generate = null;

        private List<GenerateTarget> m_Targets = new List<GenerateTarget>();
        public List<GenerateTarget> targets => m_Targets;

        //填充运行时targets数据
        public void Init(GenerateTargetSO targetso)
        {
            targets.AddRange(targetso.Targets);
        }
    }
}