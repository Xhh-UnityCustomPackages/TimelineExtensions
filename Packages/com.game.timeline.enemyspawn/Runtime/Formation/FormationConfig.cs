using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Timeline
{
    [CreateAssetMenu(fileName = "NewFormation", menuName = "SpawnSystem/Formation")]
    public class FormationConfig : ScriptableObject
    {
        [Header("自定义点位")] public List<Vector3> customPositions = new List<Vector3>();
    }
}