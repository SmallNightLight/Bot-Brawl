public class FloatControl : FieldControl<float>
{
    private Input1Expose _input1;

    private void Start()
    {
        _input1 = GetComponent<Input1Expose>();
        _input1.label.text = Setting.Name;
        _input1.value.contentType = TMPro.TMP_InputField.ContentType.DecimalNumber;
        SetValue(Setting.FloatValue);
        _input1.value.onEndEdit.AddListener((v) => SetValue(float.Parse(v)));
    }

    protected override float GetValue()
    {
        return float.Parse(_input1.value.text);
    }
    protected override void SetValue(float v)
    {
        _input1.value.text = UnityEngine.Mathf.Clamp(v, float.MinValue, float.MaxValue).ToString("F2");
    }
    
    private void OnDestroy()
    {
        Setting.FloatValue = GetValue();
        _input1.value.onEndEdit.RemoveAllListeners();
    }
}