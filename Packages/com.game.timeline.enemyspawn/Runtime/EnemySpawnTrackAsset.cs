using System.ComponentModel;
using UnityEngine;
using UnityEngine.Timeline;

namespace Game.Timeline
{
    [DisplayName("怪物生成")]
    [TrackClipType(typeof(EnemySpawnClipAsset))]
    public class EnemySpawnTrackAsset : BaseTrackAsset
    {
    }
}