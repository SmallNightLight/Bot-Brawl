using ScriptableArchitecture.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SerializedPartData
{
    public string CurrentSettingName;
    public string BasePartName;

    public Vector3Int Position;
    public Vector3 Rotation;
    public int Material;
    public int Cost;

    public string CustomName;
    public List<PartSetting> Settings;
    
    public SerializedPartData(string currentSettingsName, BasePartDataReference basePart, Vector3Int position, Vector3 rotation, int material, int cost, string customName, List<PartSetting> settings)
    {
        CurrentSettingName = currentSettingsName;
        BasePartName = basePart.Value.PartName;
        Position = position;
        Rotation = rotation;
        Material = material;
        Cost = cost;
        CustomName = customName;
        Settings = settings;
    }
}