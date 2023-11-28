using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace PartSettingsIO.SettingProcessor
{
    public class ClampedIntSettingProcessor : SettingProcessorBase
    {
        private TMP_Text _inputLabel;
        private TMP_InputField _inputField;
        
        public static ClampedIntSettingProcessor CreateInstance(GameObject instantiatedUi, PartSetting uiSetting)
        {
            var instance = instantiatedUi.AddComponent<ClampedIntSettingProcessor>();
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
            _inputLabel = UiObject.GetComponentInChildren<TMP_Text>();
            _inputField = UiObject.GetComponentInChildren<TMP_InputField>();
            _inputField.GetComponentInChildren<TMP_Text>().text = $"int: {UiSetting.MinInt}...{UiSetting.MinInt}";
            _inputLabel.text = UiSetting.Name; 
            _inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
            _inputField.text = UiSetting.IntValue.ToString("N", CultureInfo.InvariantCulture);
        }
        
        protected override void SetEvent()
        {
            _inputField.onEndEdit.AddListener((value) =>
            {
                ValidateInput();
                ModifySetting();
            });
        }
    
        protected override void ValidateInput() 
        {
            int clamped = Math.Clamp(int.Parse(_inputField.text), UiSetting.MinInt, UiSetting.MaxInt);
            _inputField.text = clamped.ToString("N", CultureInfo.InvariantCulture);
        }
    
        protected override void ModifySetting()
        {
            UiSetting.IntValue = int.Parse(_inputField.text);
        }
        
        private void OnDestroy()
        {
            _inputField.onEndEdit.RemoveAllListeners();
            
        }
        
    }

}