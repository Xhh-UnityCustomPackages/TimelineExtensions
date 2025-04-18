using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Game.Timeline
{
    public class EnemySpawnBehaviour : BaseBehaviour
    {
        private List<GenerateFactory> m_Generaters;

        public EnemySpawnClipAsset clipAsset => GetData<EnemySpawnClipAsset>();


        protected override void OnStart(object binding)
        {
            foreach (var generateSetting in clipAsset.GenerateSettings)
            {
                if (generateSetting.id.Count <= 0) continue;
                if (generateSetting.count <= 0) continue;

                if (m_Generaters == null) m_Generaters = new();
                if (generateSetting.generateType == GenerateSetting.GenerateType.Interval)
                {
                    m_Generaters.Add(new GenerateFactory_Interval(generateSetting.id[0], clipAsset.spawnID, generateSetting.count, generateSetting.interval));
                }
                else if (generateSetting.generateType == GenerateSetting.GenerateType.Formation)
                {
                    m_Generaters.Add(new GenerateFactory_Formation(generateSetting.id[0], clipAsset.spawnID, generateSetting.formationConfig, generateSetting.interval));
                }
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
        protected int m_SpawnID;

        public GenerateFactory(int spawnID)
        {
            m_SpawnID = spawnID;
        }

        public virtual void OnStart()
        {
        }

        public virtual void OnUpdate(float deltaTime)
        {
        }

        protected void Generate(int objectID, Vector3 offset)
        {
            if (EnemySpawnController.S.generate == null)
            {
                Debug.LogError("Not Set EnemyGenerateController Generate Method");
                return;
            }

            EnemySpawnController.S.generate.Invoke(objectID, m_SpawnID, offset);
        }
    }

    public class GenerateFactory_Interval : GenerateFactory
    {
        private readonly int m_ObjectID;

        private readonly float m_Interval = 2;
        private readonly int m_MaxCount;

        private int m_GenerateCount;
        private float m_Timer;

        public GenerateFactory_Interval(int objectID, int spawnID, int count, float interval) : base(spawnID)
        {
            m_ObjectID = objectID;
            m_MaxCount = count;
            m_Interval = interval;
            m_Timer = 0;
        }

        public override void OnStart()
        {
            m_Timer = 0;
            m_GenerateCount = 0;
        }

        public override void OnUpdate(float deltaTime)
        {
            if (m_GenerateCount >= m_MaxCount) return;

            m_Timer -= deltaTime;
            if (m_Timer <= 0)
            {
                m_Timer += m_Interval;
                Generate(m_ObjectID, Vector3.zero);
                m_GenerateCount++;
            }
        }
    }

    public class GenerateFactory_Formation : GenerateFactory
    {
        private readonly int m_ObjectID;
        private readonly FormationConfig m_FormationConfig;
        private readonly float m_Interval = 2;

        private int m_GenerateCount;
        private float m_Timer;

        public GenerateFactory_Formation(int objectID, int spawnID, FormationConfig formationConfig, float interval) : base(spawnID)
        {
            m_ObjectID = objectID;
            m_FormationConfig = formationConfig;
            m_Interval = interval;
        }

        public override void OnStart()
        {
            m_Timer = m_Interval;
            m_GenerateCount = 0;
        }

        public override void OnUpdate(float deltaTime)
        {
            if (m_FormationConfig == null) return;

            if (m_GenerateCount >= m_FormationConfig.customPositions.Count) return;

            if (m_Interval <= 0)
            {
                //一瞬间把所有的全部生成了
                foreach (var position in m_FormationConfig.customPositions)
                {
                    Generate(m_ObjectID, position);
                }

                m_GenerateCount = m_FormationConfig.customPositions.Count;
            }
            else
            {
                m_Timer -= deltaTime;
                if (m_Timer <= 0)
                {
                    m_Timer = m_Interval;
                    Generate(m_ObjectID, m_FormationConfig.customPositions[m_GenerateCount]);
                    m_GenerateCount++;
                }
            }
        }
    }
}