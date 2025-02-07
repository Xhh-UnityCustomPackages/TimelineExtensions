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
        private Transform spawnRoot;
        private Vector2 scrollPos;
        private bool snapToGrid = true;
        private float gridSize = 0.5f;

        bool isDirty = false;

        [MenuItem("Tools/Spawn System/Formation Editor")]
        public static void ShowWindow()
        {
            GetWindow<FormationEditorWindow>("阵型编辑器");
        }

        // 新增配置设置方法
        public void SetCurrentConfig(FormationConfig config)
        {
            currentConfig = config;
            Repaint(); // 立即刷新窗口
            SceneView.RepaintAll(); // 刷新场景视图
        }

        // 在原有代码中添加自动保存逻辑
        void OnLostFocus()
        {
            if (currentConfig != null)
            {
                EditorUtility.SetDirty(currentConfig);
                AssetDatabase.SaveAssets();
            }
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
            EditorGUI.BeginChangeCheck();


            // 配置选择区域
            currentConfig = EditorGUILayout.ObjectField("当前阵型配置", currentConfig, typeof(FormationConfig), false) as FormationConfig;

            spawnRoot = EditorGUILayout.ObjectField("当前出怪点", spawnRoot, typeof(Transform), true) as Transform;

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

            // ...原有编辑逻辑
            if (EditorGUI.EndChangeCheck())
            {
                isDirty = true;
            }
        }

        void OnDestroy()
        {
            if (isDirty && currentConfig != null)
            {
                if (EditorUtility.DisplayDialog("保存修改", "是否保存对配置的修改？", "保存", "不保存"))
                {
                    EditorUtility.SetDirty(currentConfig);
                    AssetDatabase.SaveAssets();
                }
            }
        }


        void OnSceneGUI(SceneView sceneView)
        {
            if (currentConfig == null) return;
            if (spawnRoot == null) return;

            Handles.SphereHandleCap(0, spawnRoot.position, Quaternion.identity, 1.1f, EventType.Repaint);

            Handles.color = Color.cyan;
            for (int i = 0; i < currentConfig.customPositions.Count; i++)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 newPos = Handles.PositionHandle(
                    currentConfig.customPositions[i] + spawnRoot.position,
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

                    currentConfig.customPositions[i] = newPos - spawnRoot.position;
                }

                Handles.color = Color.red;
                Handles.Label(newPos, $"Point {i}");
            }
        }
    }
}