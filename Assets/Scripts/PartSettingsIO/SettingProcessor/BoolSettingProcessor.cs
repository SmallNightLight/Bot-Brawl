using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PartSettingsIO.SettingProcessor
{
    public class BoolSettingProcessor : SettingProcessorBase
    {
        private TMP_Text _toggleLabel;
        private Toggle _toggleCheckbox;
        
        public static BoolSettingProcessor CreateInstance(GameObject instantiatedUi, PartSetting uiSetting)
        {
            var instance = instantiatedUi.AddComponent<BoolSettingProcessor>();
            instance.UiObject = instantiatedUi;
            instance.UiSetting = uiSetting;
            instance.Init();
            return instance;
        }
    
        private void Init()
        {
            InitializeUi();
            SetEvent();
        }

        protected override void InitializeUi()
        {
            _toggleLabel = UiObject.GetComponentInChildren<TMP_Text>();
            _toggleCheckbox = UiObject.GetComponentInChildren<Toggle>();
            _toggleLabel.text = UiSetting.Name;
            _toggleCheckbox.isOn = UiSetting.BoolValue;
        }

        protected override void SetEvent()
        {
            _toggleCheckbox.onValueChanged.AddListener((value) =>
            {
                ModifySetting();
            });
        }

        protected override void ValidateInput() {}

        protected override void ModifySetting()
        {
            UiSetting.BoolValue = _toggleCheckbox.isOn;
        }
    
        private void OnDestroy()
        {
            _toggleCheckbox.onValueChanged.RemoveAllListeners();
            
        }
    }
}