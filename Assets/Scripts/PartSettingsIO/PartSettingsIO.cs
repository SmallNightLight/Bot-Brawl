using System.Collections.Generic;
using PartSettingsIO.SettingProcessor;
using ScriptableArchitecture.Data;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PartSettingsIO
{
    public class PartSettingsIO : MonoBehaviour
    {
        [SerializeField] private PartData partDataAsset;
        
        [SerializeField] private GameObject togglePrefab;
        [SerializeField] private GameObject inputFieldPrefab;
        
        private Transform _panelTransform;
        private readonly List<SettingProcessorBase> _processes = new List<SettingProcessorBase>();
    
        void Start()
        {
            _panelTransform = transform.Find("Panel");
            ProcessPartSettings();
        }

        private void ProcessPartSettings()
        {
            if (partDataAsset != null)
            {
                foreach (PartSetting setting in partDataAsset.Settings)
                {
                    switch (setting.VariableType)
                    {
                        case PartSetting.SettingType.Bool:
                            _processes.Add(BoolSettingProcessor.CreateInstance(Instantiate(togglePrefab, _panelTransform), setting));
                            break;
                        case PartSetting.SettingType.Int:
                            
                            break;
                        case PartSetting.SettingType.ClampedInt:
                            
                            break;
                        case PartSetting.SettingType.Float:
                            
                            break;
                        case PartSetting.SettingType.ClampedFloat:
                            _processes.Add(ClampedFloatSettingProcessor.CreateInstance(Instantiate(inputFieldPrefab, _panelTransform), setting));
                            break;
                    }
                }
            }
        }
    
        private void WriteSettings()
        {
            #if UNITY_EDITOR
            AssetDatabase.SaveAssets();
            #endif
        }
        
        private void OnDestroy()
        {
            WriteSettings();
            foreach (var process in _processes)
            {
                Destroy(process);
            }
        }
    }
}

