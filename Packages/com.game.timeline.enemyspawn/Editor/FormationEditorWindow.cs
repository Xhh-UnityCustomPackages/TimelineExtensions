using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game.Timeline.Editor
{
    public class FormationEditorWindow : EditorWindow
    {
        private FormationConfig currentConfig;
        private Vector2 scrollPos;
        private bool snapToGrid = true;
        private float gridSize = 0.5f;

        [MenuItem("Tools/Spawn System/Formation Editor")]
        public static void ShowWindow()
        {
            GetWindow<FormationEditorWindow>("阵型编辑器");
        }

        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        void OnGUI()
        {
            // 配置选择区域
            currentConfig = EditorGUILayout.ObjectField("当前阵型配置",
                currentConfig, typeof(FormationConfig), false) as FormationConfig;

            if (currentConfig == null) return;

            // 工具栏
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            // if (GUILayout.Button("生成默认网格", EditorStyles.toolbarButton))
            // {
            //     // GenerateGridFormation();
            // }

            snapToGrid = GUILayout.Toggle(snapToGrid, "网格吸附", EditorStyles.toolbarButton);
            gridSize = EditorGUILayout.FloatField(gridSize);
            EditorGUILayout.EndHorizontal();

            // 可视化编辑区域
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandHeight(true));

            EditorGUILayout.LabelField("拖拽调整点位");
            EditorGUILayout.Space(10);

            // 绘制自定义点位编辑器
            for (int i = 0; i < currentConfig.customPositions.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                currentConfig.customPositions[i] = EditorGUILayout.Vector3Field(
                    $"点位 {i}", currentConfig.customPositions[i]);

                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    currentConfig.customPositions.RemoveAt(i);
                }

                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("+ 添加新点位"))
            {
                currentConfig.customPositions.Add(Vector3.zero);
            }

            EditorGUILayout.EndScrollView();

            // 场景视图同步更新
            SceneView.RepaintAll();
        }


        void OnSceneGUI(SceneView sceneView)
        {
            if (currentConfig == null) return;

            Handles.color = Color.cyan;
            for (int i = 0; i < currentConfig.customPositions.Count; i++)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 newPos = Handles.PositionHandle(
                    currentConfig.customPositions[i],
                    Quaternion.identity
                );

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(currentConfig, "Move Formation Point");
                    if (snapToGrid)
                    {
                        newPos = new Vector3(
                            Mathf.Round(newPos.x / gridSize) * gridSize,
                            Mathf.Round(newPos.y / gridSize) * gridSize,
                            Mathf.Round(newPos.z / gridSize) * gridSize
                        );
                    }

                    currentConfig.customPositions[i] = newPos;
                }

                Handles.color = Color.red;
                Handles.Label(newPos, $"Point {i}");
            }
        }
    }
}