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

                    var str = $"ID:{idStr} Num:{generateSetting.count} Interval:{generateSetting.interval}";
                    title += str;
                    title += "\n";

                    var duration = (generateSetting.count + 1) * generateSetting.interval;
                    maxDuration = Mathf.Max(maxDuration, duration);
                }

                clip.displayName = title;

                maxDuration = Mathf.Max(1.0f, maxDuration);
                clip.duration = maxDuration;
            }
        }
    }
}