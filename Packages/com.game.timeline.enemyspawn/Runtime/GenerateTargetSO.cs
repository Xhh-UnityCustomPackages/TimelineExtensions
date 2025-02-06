using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenerateTargetSO", menuName = "ScriptableObjects/GenerateTargetSO")]
public partial class GenerateTargetSO : ScriptableObject
{
    public List<GenerateTarget> Targets;
}


[Serializable]
public class GenerateTarget
{
    public string name;
    public GameObject target;
}