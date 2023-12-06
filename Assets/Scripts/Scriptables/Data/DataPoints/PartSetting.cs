using UnityEngine;

[System.Serializable]
public class PartSetting
{
    public string Name;

    public object Value
    {
        set
        {
            switch (VariableType)
            {
                case SettingType.Bool:
                    BoolValue = (value as bool?) ?? false;
                    break;
                case SettingType.Float:
                    FloatValue = (value as float?) ?? 0f;
                    break;
                case SettingType.Int:
                    IntValue = (value as int?) ?? 0;
                    break;
                case SettingType.ClampedInt:
                    int v = (value as int?) ?? 0;
                    IntValue = Mathf.Clamp(v, MinInt, MaxInt);
                    break;
                case SettingType.ClampedFloat:
                    float f = (value as float?) ?? 0f;
                    FloatValue = Mathf.Clamp(f, MinFloat, MaxFloat);
                    break;
            }
        }
    }


    public enum SettingType
    {
        Bool,
        Int,
        ClampedInt,
        Float,
        ClampedFloat,
        Vector3,
        Vector3Int,
        ClampedVector3Int
    }

    public SettingType VariableType;

    //Bool
    public bool BoolValue;

    //Int / Clamped
    public int IntValue;
    public int MinInt;
    public int MaxInt;

    //Float / Clamped
    public float FloatValue;
    public float MinFloat;
    public float MaxFloat;

    //Vector3
    public Vector3 Vector3Value;

    //Vector3Int
    public Vector3Int Vector3IntValue;    
}