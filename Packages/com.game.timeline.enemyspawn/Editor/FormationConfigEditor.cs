using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game.Timeline.Editor
{
    [CustomEditor(typeof(FormationConfig))]
    public class FormationConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // 1. 绘制默认 Inspector 内容
            DrawDefaultInspector();

            // 2. 添加分隔线
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("阵型编辑工具", EditorStyles.boldLabel);

            // 3. 添加打开编辑器按钮
            if (GUILayout.Button("打开阵型编辑器", GUILayout.Height(30)))
            {
                OpenFormationEditorWindow();
            }
        }

        private void OpenFormationEditorWindow()
        {
            // 获取当前编辑的 FormationConfig
            FormationConfig config = target as FormationConfig;

            // 获取或创建编辑器窗口
            FormationEditorWindow window = EditorWindow.GetWindow<FormationEditorWindow>("阵型编辑器", true);

            // 将当前配置传递给窗口
            window.SetCurrentConfig(config);
        }
    }
}