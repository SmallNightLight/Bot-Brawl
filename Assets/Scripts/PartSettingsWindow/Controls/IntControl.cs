public class IntControl : FieldControl<int>
{
    private Input1Expose _input1;

    private void Start()
    {
        _input1 = GetComponent<Input1Expose>();
        _input1.label.text = Setting.Name;
        _input1.value.contentType = TMPro.TMP_InputField.ContentType.IntegerNumber;
        SetValue(Setting.IntValue);
        _input1.value.onEndEdit.AddListener((v) => SetValue(int.Parse(v)));
    }

    protected override int GetValue()
    {
        return int.Parse(_input1.value.text);
    }
    protected override void SetValue(int v)
    {
        _input1.value.text = System.Math.Clamp(v, int.MinValue, int.MaxValue).ToString("N");
    }

    private void OnDestroy()
    {
        Setting.IntValue = GetValue();
        _input1.value.onEndEdit.RemoveAllListeners();
    }
}