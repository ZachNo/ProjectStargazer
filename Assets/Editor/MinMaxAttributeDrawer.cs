using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
public class MinMaxRangeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.Vector2)
        {
            Vector2 ret = property.vector2Value;
            float minOffsetPos = position.width < 339 ? 118 : position.width * 0.45f - 35;
            float maxOffsetPos = position.width * 0.65f + 20;
            //EditorGUI.LabelField(new Rect(position.x, position.y, position.width, position.height), label);
            //EditorGUI.FloatField(new Rect(position.x + position.width * 0.8f, position.y, 200, position.height), new GUIContent("Min"), ret.x);
            //EditorGUI.FloatField(new Rect(position.x + position.width * 0.6f, position.y, 200, position.height), new GUIContent("Max"), ret.x);
            //EditorGUI.MultiFloatField(position, new GUIContent[] { new GUIContent("Min"), new GUIContent("Max") }, new float[] { ret.x, ret.y });
            EditorGUI.FloatField(new Rect(minOffsetPos + 40, position.y, (maxOffsetPos - minOffsetPos - 30), position.height), ret.x);
            EditorGUI.FloatField(new Rect(maxOffsetPos + 45, position.y, (position.width - maxOffsetPos - 31), position.height), ret.y);
            //EditorGUI.LabelField(new Rect(position.x + minOffsetPos, position.y, position.width, position.height), new GUIContent("Min"));
            //EditorGUI.LabelField(new Rect(position.x + maxOffsetPos, position.y, position.width, position.height), new GUIContent("Max"));
            property.vector2Value = ret;
        }
    }
}
