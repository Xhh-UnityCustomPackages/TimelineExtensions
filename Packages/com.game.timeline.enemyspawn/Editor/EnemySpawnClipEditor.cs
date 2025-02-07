using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace Game.Timeline.Editor
{
    [CustomTimelineEditor(typeof(EnemySpawnClipAsset))]
    public class EnemySpawnClipEditor : ClipEditor
    {
        // protected 
        public override void OnClipChanged(TimelineClip clip)
        {
            if (clip.asset is EnemySpawnClipAsset changeNameClass)
            {
                var title = $"SpawnID:{changeNameClass.spawnID}\n";
                var maxDuration = 0f;


                foreach (var generateSetting in changeNameClass.GenerateSettings)
                {
                    string idStr = "";
                    foreach (var id in generateSetting.id)
                    {
                        idStr += id + ", ";
                    }

                    int count = 0;

                    if (generateSetting.generateType == GenerateSetting.GenerateType.Interval)
                    {
                        count = generateSetting.count;
                    }
                    else if (generateSetting.generateType == GenerateSetting.GenerateType.Formation)
                    {
                        if (generateSetting.formationConfig == null)
                        {
                            count = 0;
                        }
                        else
                        {
                            count = generateSetting.formationConfig.customPositions.Count;
                        }
                    }

                    var str = $"ID:{idStr} Num:{count} Interval:{generateSetting.interval}";
                    title += str;
                    title += "\n";

                    var duration = GetDuration(generateSetting);
                    maxDuration = Mathf.Max(maxDuration, duration);
                }

                clip.displayName = title;

                maxDuration = Mathf.Max(1.0f, maxDuration);
                clip.duration = maxDuration;
            }
        }

        public float GetDuration(GenerateSetting generateSetting)
        {
            if (generateSetting.generateType == GenerateSetting.GenerateType.Interval)
            {
                var duration = (generateSetting.count + 1) * generateSetting.interval;
                return duration;
            }
            else if (generateSetting.generateType == GenerateSetting.GenerateType.Formation)
            {
                if (generateSetting.interval <= 0)
                {
                    return 1f;
                }
                else
                {
                    if (generateSetting.formationConfig == null)
                        return 1f;
                    else
                        return (generateSetting.formationConfig.customPositions.Count + 1) * generateSetting.interval;
                }
            }

            return 1f;
        }
    }
}