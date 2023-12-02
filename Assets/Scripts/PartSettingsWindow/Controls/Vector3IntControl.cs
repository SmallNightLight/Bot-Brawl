public class Vector3IntControl : FieldControl<UnityEngine.Vector3Int>
{
    private Input3Expose _input3;
    
    private void Start()
    {
        _input3 = GetComponent<Input3Expose>();
        _input3.label.text = Setting.Name;
        _input3.valueX.contentType = TMPro.TMP_InputField.ContentType.IntegerNumber;
        _input3.valueY.contentType = TMPro.TMP_InputField.ContentType.IntegerNumber;
        _input3.valueZ.contentType = TMPro.TMP_InputField.ContentType.IntegerNumber;
        SetValue(Setting.Vector3IntValue);
        _input3.valueX.onEndEdit.AddListener((v) => SetValue(_input3.valueX, int.Parse(v)));
        _input3.valueY.onEndEdit.AddListener((v) => SetValue(_input3.valueY, int.Parse(v)));
        _input3.valueZ.onEndEdit.AddListener((v) => SetValue(_input3.valueZ, int.Parse(v)));
    }

    protected override UnityEngine.Vector3Int GetValue()
    {
        return new UnityEngine.Vector3Int(int.Parse(_input3.valueX.text), int.Parse(_input3.valueY.text),
            int.Parse(_input3.valueZ.text));
    }
    protected override void SetValue(UnityEngine.Vector3Int v)
    {
        SetValue(_input3.valueX, v.x);
        SetValue(_input3.valueY, v.y);
        SetValue(_input3.valueZ, v.z);
    }
    private void SetValue(TMPro.TMP_InputField f, int v)
    {
        f.text = System.Math.Clamp(v, int.MinValue, int.MaxValue).ToString("N");
    }

    private void OnDestroy()
    {
        Setting.Vector3IntValue = GetValue();
        _input3.valueX.onEndEdit.RemoveAllListeners();
        _input3.valueY.onEndEdit.RemoveAllListeners();
        _input3.valueZ.onEndEdit.RemoveAllListeners();
    }
}