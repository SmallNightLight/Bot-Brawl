using System.Globalization;
using TMPro;
using UnityEngine;

namespace PartSettingsIO.SettingProcessor
{
    public class FloatSettingProcessor : SettingProcessorBase
    {
        private TMP_Text _inputLabel;
        private TMP_InputField _inputField;
        
        public static FloatSettingProcessor CreateInstance(GameObject instantiatedUi, PartSetting uiSetting)
        {
            var instance = instantiatedUi.AddComponent<FloatSettingProcessor>();
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
            _inputField.GetComponentInChildren<TMP_Text>().text = "float:";
            _inputLabel.text = UiSetting.Name; 
            _inputField.contentType = TMP_InputField.ContentType.DecimalNumber;
            _inputField.text = UiSetting.FloatValue.ToString("F", CultureInfo.InvariantCulture);
        }
        
        protected override void SetEvent()
        {
            _inputField.onEndEdit.AddListener((value) =>
            {
                ModifySetting();
            });
        }
    
        protected override void ValidateInput() 
        {
        }
    
        protected override void ModifySetting()
        {
            UiSetting.FloatValue = float.Parse(_inputField.text);
        }
        
        private void OnDestroy()
        {
            _inputField.onEndEdit.RemoveAllListeners();
            
        }
    }

}
