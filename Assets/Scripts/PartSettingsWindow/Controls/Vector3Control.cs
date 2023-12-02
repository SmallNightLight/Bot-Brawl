public class Vector3Control : FieldControl<UnityEngine.Vector3>
{
    private Input3Expose _input3;
    
    private void Start()
    {
        _input3 = GetComponent<Input3Expose>();
        _input3.label.text = Setting.Name;
        _input3.valueX.contentType = TMPro.TMP_InputField.ContentType.DecimalNumber;
        _input3.valueY.contentType = TMPro.TMP_InputField.ContentType.DecimalNumber;
        _input3.valueZ.contentType = TMPro.TMP_InputField.ContentType.DecimalNumber;
        SetValue(Setting.Vector3Value);
        _input3.valueX.onEndEdit.AddListener((v) => SetValue(_input3.valueX, float.Parse(v)));
        _input3.valueY.onEndEdit.AddListener((v) => SetValue(_input3.valueY, float.Parse(v)));
        _input3.valueZ.onEndEdit.AddListener((v) => SetValue(_input3.valueZ, float.Parse(v)));
    }

    protected override UnityEngine.Vector3 GetValue()
    {
        return new UnityEngine.Vector3(float.Parse(_input3.valueX.text), float.Parse(_input3.valueY.text),
            float.Parse(_input3.valueZ.text));
    }
    protected override void SetValue(UnityEngine.Vector3 v)
    {
        SetValue(_input3.valueX, v.x);
        SetValue(_input3.valueY, v.y);
        SetValue(_input3.valueZ, v.z);
    }
    private void SetValue(TMPro.TMP_InputField f, float v)
    {
        f.text = System.Math.Clamp(v, float.MinValue, float.MaxValue).ToString("F2");
    }

    private void OnDestroy()
    {
        Setting.Vector3Value = GetValue();
        _input3.valueX.onEndEdit.RemoveAllListeners();
        _input3.valueY.onEndEdit.RemoveAllListeners();
        _input3.valueZ.onEndEdit.RemoveAllListeners();
    }
}