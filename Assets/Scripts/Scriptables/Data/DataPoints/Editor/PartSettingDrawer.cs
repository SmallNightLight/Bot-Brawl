using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PartSetting))]
public class PartSettingDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        //Draw the property label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        EditorGUI.indentLevel = 0;

        //Draw the Name field
        float negativeOffset = EditorGUIUtility.currentViewWidth - position.width;
        SerializedProperty customStringProp = property.FindPropertyRelative("Name");
        EditorGUI.PropertyField(new Rect(position.x - negativeOffset, position.y, 130, position.height), customStringProp, GUIContent.none);

        //Draw the VariableType field
        SerializedProperty typeProp = property.FindPropertyRelative("VariableType");
        EditorGUI.PropertyField(new Rect(position.x, position.y, 100, position.height), typeProp, GUIContent.none);

        position.x += 110;

        switch ((PartSetting.SettingType)typeProp.enumValueIndex)
        {
            case PartSetting.SettingType.Bool:
                DrawBoolFields(position, property);
                break;
            case PartSetting.SettingType.Int:
                DrawIntFields(position, property);
                break;
            case PartSetting.SettingType.ClampedInt:
                DrawClampedIntFields(position, property);
                break;
            case PartSetting.SettingType.Float:
                DrawFloatFields(position, property);
                break;
            case PartSetting.SettingType.ClampedFloat:
                DrawClampedFloatFields(position, property);
                break;
            case PartSetting.SettingType.Vector3:
                DrawVector3Fields(position, property);
                break;
            case PartSetting.SettingType.Vector3Int:
                DrawVector3IntFields(position, property);
                break;
        }

        EditorGUI.EndProperty();
    }

    private void DrawBoolFields(Rect position, SerializedProperty property)
    {
        SerializedProperty boolValueProp = property.FindPropertyRelative("BoolValue");
        EditorGUI.PropertyField(new Rect(position.x, position.y, 100, position.height), boolValueProp, GUIContent.none);
    }

    private void DrawIntFields(Rect position, SerializedProperty property)
    {
        SerializedProperty intValueProp = property.FindPropertyRelative("IntValue");
        EditorGUI.PropertyField(new Rect(position.x, position.y, 80, position.height), intValueProp, GUIContent.none);
    }

    private void DrawClampedIntFields(Rect position, SerializedProperty property)
    {
        SerializedProperty intValueProp = property.FindPropertyRelative("IntValue");
        EditorGUI.PropertyField(new Rect(position.x, position.y, 35, position.height), intValueProp, GUIContent.none);

        position.x += 40;

        if ((PartSetting.SettingType)property.FindPropertyRelative("VariableType").enumValueIndex == PartSetting.SettingType.ClampedInt)
        {
            EditorGUI.LabelField(new Rect(position.x, position.y, 30, position.height), "Min");
            SerializedProperty minIntProp = property.FindPropertyRelative("MinInt");
            EditorGUI.PropertyField(new Rect(position.x + 25, position.y, 35, position.height), minIntProp, GUIContent.none);

            position.x += 63;

            EditorGUI.LabelField(new Rect(position.x, position.y, 30, position.height), "Max");
            SerializedProperty maxIntProp = property.FindPropertyRelative("MaxInt");
            EditorGUI.PropertyField(new Rect(position.x + 30, position.y, 35, position.height), maxIntProp, GUIContent.none);
        }
    }

    private void DrawFloatFields(Rect position, SerializedProperty property)
    {
        SerializedProperty floatValueProp = property.FindPropertyRelative("FloatValue");
        EditorGUI.PropertyField(new Rect(position.x, position.y, 80, position.height), floatValueProp, GUIContent.none);
    }

    private void DrawClampedFloatFields(Rect position, SerializedProperty property)
    {
        SerializedProperty floatValueProp = property.FindPropertyRelative("FloatValue");
        EditorGUI.PropertyField(new Rect(position.x, position.y, 35, position.height), floatValueProp, GUIContent.none);

        position.x += 40;

        if ((PartSetting.SettingType)property.FindPropertyRelative("VariableType").enumValueIndex == PartSetting.SettingType.ClampedFloat)
        {
            EditorGUI.LabelField(new Rect(position.x, position.y, 30, position.height), "Min");
            SerializedProperty minFloatProp = property.FindPropertyRelative("MinFloat");
            EditorGUI.PropertyField(new Rect(position.x + 25, position.y, 35, position.height), minFloatProp, GUIContent.none);

            position.x += 63;

            EditorGUI.LabelField(new Rect(position.x, position.y, 30, position.height), "Max");
            SerializedProperty maxFloatProp = property.FindPropertyRelative("MaxFloat");
            EditorGUI.PropertyField(new Rect(position.x + 30, position.y, 35, position.height), maxFloatProp, GUIContent.none);
        }
    }

    private void DrawVector3Fields(Rect position, SerializedProperty property)
    {
        SerializedProperty vector3ValueProp = property.FindPropertyRelative("Vector3Value");
        EditorGUI.PropertyField(new Rect(position.x, position.y, 150, position.height), vector3ValueProp, GUIContent.none);
    }

    private void DrawVector3IntFields(Rect position, SerializedProperty property)
    {
        SerializedProperty vector3IntValueProp = property.FindPropertyRelative("Vector3IntValue");
        EditorGUI.PropertyField(new Rect(position.x, position.y, 150, position.height), vector3IntValueProp, GUIContent.none);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
}
