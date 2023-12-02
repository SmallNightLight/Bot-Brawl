public class BoolControl : FieldControl<bool>
{
    private ToggleExpose _toggle;
    
    private void Start()
    {
        _toggle = GetComponent<ToggleExpose>();
        _toggle.label.text = Setting.Name;
        SetValue(Setting.BoolValue);
        _toggle.value.onValueChanged.AddListener(SetValue);
    }

    protected override bool GetValue()
    {
        return _toggle.value.isOn;
    }
    protected override void SetValue(bool v)
    {
        _toggle.value.isOn = v;
    }

    private void OnDestroy()
    {
        Setting.BoolValue = GetValue();
        _toggle.value.onValueChanged.RemoveAllListeners();
    }
}
