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

        public delegate void Generate(int objectID, int spawnID);

        public Generate generate = null;
    }
}