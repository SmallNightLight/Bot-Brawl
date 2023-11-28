using UnityEngine;

namespace PartSettingsIO.SettingProcessor
{
    public abstract class SettingProcessorBase : MonoBehaviour
    {
        protected GameObject UiObject;
        protected PartSetting UiSetting;
        
        protected abstract void InitializeUi();
    
        protected abstract void SetEvent();

        protected abstract void ValidateInput();

        protected abstract void ModifySetting();
    }
}
