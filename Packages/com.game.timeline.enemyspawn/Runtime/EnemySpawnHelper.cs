using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System;
using UnityEngine.Timeline;

namespace Game.Timeline
{
    public static class EnemySpawnHelper
    {
        public static void TraversePlayables(Playable playable, int depth = 0, Action<Playable> traverseAction = null)
        {
            if (!playable.IsValid()) return;

            // 打印当前 Playable 信息
            // string indent = new string(' ', depth * 2);
            // Debug.LogError($"{indent}Type: {playable.GetPlayableType().Name}, Inputs: {playable.GetInputCount()}");

            traverseAction?.Invoke(playable);


            // 递归遍历所有输入（子 Playable）
            for (int i = 0; i < playable.GetInputCount(); i++)
            {
                Playable input = playable.GetInput(i);
                TraversePlayables(input, depth + 1, traverseAction);
            }
        }


        public static int GetGenerateCount(Playable playable)
        {
            int totalCount = 0;

            TraversePlayables(playable, 0, (playable) =>
            {
                if (playable.GetPlayableType() == typeof(EnemySpawnBehaviour))
                {
                    var behaviour = ((ScriptPlayable<EnemySpawnBehaviour>)(playable)).GetBehaviour();
                    totalCount += behaviour.clipAsset.totalCount;
                }
            });


            return totalCount;
        }

        public static int GetSpawnGenerateCount(Playable playable, int spawnID)
        {
            int totalCount = 0;

            TraversePlayables(playable, 0, (playable) =>
            {
                if (playable.GetPlayableType() == typeof(EnemySpawnBehaviour))
                {
                    var behaviour = ((ScriptPlayable<EnemySpawnBehaviour>)(playable)).GetBehaviour();
                    if (behaviour.clipAsset.spawnID == spawnID)
                        totalCount += behaviour.clipAsset.totalCount;
                }
            });

            return totalCount;
        }

        public static void GetSpawnGenerateDict(Dictionary<int, int> generateDict, Playable playable)
        {
            generateDict.Clear();

            TraversePlayables(playable, 0, (playable) =>
            {
                if (playable.GetPlayableType() == typeof(EnemySpawnBehaviour))
                {
                    var behaviour = ((ScriptPlayable<EnemySpawnBehaviour>)(playable)).GetBehaviour();
                    if (!generateDict.ContainsKey(behaviour.clipAsset.spawnID))
                        generateDict.Add(behaviour.clipAsset.spawnID, 0);

                    generateDict[behaviour.clipAsset.spawnID] += behaviour.clipAsset.totalCount;
                }
            });
        }


        public static void GetSpawnGenerateDict(Dictionary<int, int> generateDict, TimelineAsset timelineAsset)
        {
            generateDict.Clear();
            // 遍历所有轨道
            foreach (var track in timelineAsset.GetOutputTracks())
            {
                // Debug.Log($"轨道名称: {track.name}, 类型: {track.GetType()}");

                // 遍历轨道上的所有 Clip
                foreach (TimelineClip clip in track.GetClips())
                {
                    // Debug.Log($" - Clip: {clip.displayName}, 开始时间: {clip.start}, 时长: {clip.duration}");

                    // 获取 Clip 的 PlayableAsset
                    if (clip.asset is PlayableAsset playableAsset)
                    {
                        // 针对自定义 Clip 类型处理
                        if (playableAsset is EnemySpawnClipAsset spawnClip)
                        {
                            if (!generateDict.ContainsKey(spawnClip.spawnID))
                                generateDict.Add(spawnClip.spawnID, 0);

                            generateDict[spawnClip.spawnID] += spawnClip.totalCount;
                        }
                    }
                }
            }
        }
    }
}