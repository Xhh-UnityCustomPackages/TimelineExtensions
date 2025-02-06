using System.Collections;
using System.Collections.Generic;
using Game.Timeline;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GenerateTargetSO generateTargetSO;
    public List<Transform> spawnPoints;


    // Start is called before the first frame update
    void Start()
    {
        EnemySpawnController.S.Init(generateTargetSO);
        //自定义生成方法
        EnemySpawnController.S.generate = (prefab, id) =>
        {
            var go = GameObject.Instantiate(prefab, spawnPoints[id]);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            return go;
        };
    }

    // Update is called once per frame
    void Update()
    {
    }
}