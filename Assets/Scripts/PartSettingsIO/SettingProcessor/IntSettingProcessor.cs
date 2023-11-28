using System.Globalization;
using TMPro;
using UnityEngine;

namespace PartSettingsIO.SettingProcessor
{
    public class IntSettingProcessor : SettingProcessorBase
    {
        private TMP_Text _inputLabel;
        private TMP_InputField _inputField;
        
        public static IntSettingProcessor CreateInstance(GameObject instantiatedUi, PartSetting uiSetting)
        {
            var instance = instantiatedUi.AddComponent<IntSettingProcessor>();
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
            _inputField.GetComponentInChildren<TMP_Text>().text = "int:";
            _inputLabel.text = UiSetting.Name; 
            _inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
            _inputField.text = UiSetting.IntValue.ToString("N0", CultureInfo.InvariantCulture);
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
            UiSetting.IntValue = int.Parse(_inputField.text);
        }
        
        private void OnDestroy()
        {
            _inputField.onEndEdit.RemoveAllListeners();
            
        }
        
    }
    
}
