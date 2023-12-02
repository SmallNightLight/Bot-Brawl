using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "PartData", menuName = "Scriptables/Variables/PartData")]
    public class PartData : ScriptableObject
    {
        public BasePartDataReference BasePart;

        public Vector3 Rotation;
        public int Material;
        public int Cost;

        public List<PartSetting> Settings = new List<PartSetting>();

        [HideInInspector] public Vector3Int Position;

        private string _currentSettingName;
        
        public enum PartType
        {
            Frame,
            Wheel,
            Weapon,
            Armor
        }

        public void Initialize(string currentSettingName, BasePartDataReference basePart, Vector3Int position, Vector3 rotation, int material, int cost, List<PartSetting> settings)
        {
            _currentSettingName = currentSettingName;
            BasePart = basePart;
            Position = position;
            Rotation = rotation;
            Material = material;
            Cost = cost;
            Settings = settings;
        }

        public void SetName(string name)
        {
            _currentSettingName = name;
        }

        public void SetBool(bool value)
        {
            if (TryGetSetting(_currentSettingName, out var setting))
                setting.BoolValue = value;
        }

        public void SetInt(int value)
        {
            if (TryGetSetting(_currentSettingName, out var setting))
                setting.IntValue = value;
        }

        public void SetFloat(float value)
        {
            if (TryGetSetting(_currentSettingName, out var setting))
                setting.FloatValue = value;
        }

        private PartSetting GetPartSetting(string settingName)
        {
            return Settings.FirstOrDefault(s => s.Name == settingName);
        }

        private bool TryGetSetting(string settingName, out PartSetting setting)
        {
            setting = GetPartSetting(settingName);

            if (setting == null)
                Debug.LogWarning("Setting name not found: " + settingName + " on: " + BasePart.Value.PartName);

            return setting != null;
        }

        public bool GetBool(string boolName) => GetPartSetting(boolName).BoolValue;


        public int GetInt(string intName) => GetPartSetting(intName).IntValue;

        public float GetFloat(string floatName) => GetPartSetting(floatName).FloatValue;
    }
}