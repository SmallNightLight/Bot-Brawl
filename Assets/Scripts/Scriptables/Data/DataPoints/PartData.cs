using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public abstract class PartData : ScriptableObject
    {
        public BasePartDataReference BasePart;

        public Vector3 Rotation;
        public int Material;
        public int Cost;

        public List<PartSetting> Settings = new List<PartSetting>();

        [HideInInspector] public Vector3Int Position;

        private string _currentSettingName;

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

        private bool TryGetSetting(string settingName, out PartSetting setting)
        {
            setting = Settings.FirstOrDefault(s => s.Name == settingName);

            if (setting == null)
                Debug.LogWarning("Setting name not found: " + settingName + " on: " + BasePart.Value.PartName);

            return setting != null;
        }
    }
}