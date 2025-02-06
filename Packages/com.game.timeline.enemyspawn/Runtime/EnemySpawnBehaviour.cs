using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Game.Timeline
{
    public class EnemySpawnBehaviour : BaseBehaviour
    {
        private List<GenerateFactory> m_Generaters;

        protected EnemySpawnClipAsset clipAsset => GetData<EnemySpawnClipAsset>();


        protected override void OnStart(object binding)
        {
            foreach (var generateSetting in clipAsset.GenerateSettings)
            {
                if (generateSetting.count <= 0)
                    continue;

                if (m_Generaters == null) m_Generaters = new();
                var prefab = EnemySpawnController.S.targets[0].target;
                m_Generaters.Add(new GenerateFactory_Interval(prefab, clipAsset.spawnID, generateSetting.count, generateSetting.interval));
            }

            if (m_Generaters != null)
                foreach (var generater in m_Generaters)
                {
                    generater.OnStart();
                }
        }

        protected override void OnUpdate(object binding, float deltaTime)
        {
            if (m_Generaters != null)
                foreach (var generater in m_Generaters)
                {
                    generater.OnUpdate(deltaTime);
                }
        }

        protected override void OnDestroy()
        {
            m_Generaters = null;
        }
    }


    public abstract class GenerateFactory
    {
        public virtual void OnStart()
        {
        }

        public virtual void OnUpdate(float deltaTime)
        {
        }
    }

    public class GenerateFactory_Interval : GenerateFactory
    {
        private GameObject m_Prefab;
        private readonly float m_Interval = 2;
        private readonly int m_MaxCount;
        private readonly int m_SpawnID;

        private int m_GenerateCount;
        private float m_Timer;

        public GenerateFactory_Interval(GameObject prefab, int spawnID, int count, float interval)
        {
            m_SpawnID = spawnID;
            m_Prefab = prefab;
            m_MaxCount = count;
            m_Interval = interval;
        }

        public override void OnStart()
        {
            m_Timer = m_Interval;
            m_GenerateCount = 0;
        }

        public override void OnUpdate(float deltaTime)
        {
            if (m_GenerateCount > m_MaxCount) return;

            m_Timer -= deltaTime;
            if (m_Timer <= 0)
            {
                m_Timer = m_Interval;
                Generate(m_Prefab);
                m_GenerateCount++;
            }
        }


        public void Generate(GameObject prefab)
        {
            if (prefab == null) return;

            if (EnemySpawnController.S.generate == null)
            {
                Debug.LogError("Not Set EnemyGenerateController Generate Method");
                return;
            }

            EnemySpawnController.S.generate.Invoke(prefab, m_SpawnID);
        }
    }
}