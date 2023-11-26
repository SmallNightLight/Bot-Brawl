using UnityEngine;

[System.Serializable]
public class PartSetting
{
    public string Name;

    public enum SettingType
    {
        Bool,
        Int,
        ClampedInt,
        Float,
        ClampedFloat,
        Vector3,
        Vector3Int
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