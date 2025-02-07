using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace Game.Timeline.Editor
{
    [CustomPropertyDrawer(typeof(GenerateSetting), true)]
    public class GenerateSettingDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // 使用 BeginProperty / EndProperty 来确保 prefab 覆盖逻辑正常工作
            EditorGUI.BeginProperty(position, label, property);

            // 绘制标签
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // 缩进
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // 计算每个属性的矩形区域
            var idRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            var id = property.FindPropertyRelative("id");


            // 绘制每个属性
            var arrayHeight = 0f;
            if (id.isExpanded)
            {
                var arraySize = id.arraySize;
                arrayHeight = arraySize * EditorGUIUtility.singleLineHeight;
                idRect.height += arrayHeight + EditorGUIUtility.singleLineHeight;
            }

            EditorGUI.PropertyField(idRect, id, new GUIContent("ID"));

            var rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            rect.position = new Vector2() { x = rect.position.x, y = rect.position.y + idRect.height + (id.isExpanded ? EditorGUIUtility.singleLineHeight : 0) };
            var generateType = property.FindPropertyRelative("generateType");
            EditorGUI.PropertyField(rect, generateType, new GUIContent("生成类型"));
            rect.position = new Vector2() { x = rect.position.x, y = rect.position.y + 20 };
            if (generateType.enumValueFlag == (int)GenerateSetting.GenerateType.Formation)
            {
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("formationConfig"), new GUIContent("阵型"));
            }
            else if (generateType.enumValueFlag == (int)GenerateSetting.GenerateType.Interval)
            {
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("count"), new GUIContent("数量"));
            }

            rect.position = new Vector2() { x = rect.position.x, y = rect.position.y + 20 };
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("interval"), new GUIContent("间隔"));

            // 恢复缩进
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int propCount = 4;
            var id = property.FindPropertyRelative("id");
            if (id.isExpanded)
            {
                propCount += id.arraySize;
                propCount += 2;
            }

            // 计算总高度，每个属性占一行，每行高度为 EditorGUIUtility.singleLineHeight
            return EditorGUIUtility.singleLineHeight * propCount + EditorGUIUtility.standardVerticalSpacing * (propCount - 1);
        }
    }
}