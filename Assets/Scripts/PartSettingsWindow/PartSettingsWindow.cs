using UnityEngine;
using System.Collections.Generic;
using ScriptableArchitecture.Data;
using UnityEngine.UI;
using TMPro;

public class PartSettingsWindow : MonoBehaviour
{
    private PartData _partData; // temporarily testing
    
    [SerializeField] private List<GameObject> _fieldPrefabs;
    [SerializeField] private GameObject _namePrefab;

    [SerializeField] private Transform _windowContext;

    [SerializeField] Image image;

    public void SetPartData(PartData partData)
    {
        _partData = partData;
        InitWindow();
    }

    public void Clear()
    {
        if (_windowContext != null)
            foreach(Transform child in _windowContext)
                Destroy(child.gameObject);

        image.enabled = false;
    }
    
    void Start()
    {
        image.enabled = false;
    }

    private void InitWindow()
    {
        Clear();

        if (_partData.Settings.Count == 0)
            return;
            
        image.enabled = true;

        TMP_InputField inputName = Instantiate(_namePrefab, _windowContext).GetComponentInChildren<TMP_InputField>();
        inputName.onValueChanged.AddListener(name => _partData.CustomName = name);
        inputName.text = _partData.CustomName;

        foreach (var setting in _partData.Settings)
        {
            switch (setting.VariableType)
            {
                case PartSetting.SettingType.Bool:
                    Instantiate(_fieldPrefabs[0], _windowContext).AddComponent<BoolControl>().ReceiveSetting(setting);
                    break;
                case PartSetting.SettingType.Int:
                    Instantiate(_fieldPrefabs[1], _windowContext).AddComponent<IntControl>().ReceiveSetting(setting);
                    break;
                case PartSetting.SettingType.ClampedInt:
                    Instantiate(_fieldPrefabs[1], _windowContext).AddComponent<ClampedIntControl>().ReceiveSetting(setting);
                    break;
                case PartSetting.SettingType.Float:
                    Instantiate(_fieldPrefabs[1], _windowContext).AddComponent<FloatControl>().ReceiveSetting(setting);
                    break;
                case PartSetting.SettingType.ClampedFloat:
                    Instantiate(_fieldPrefabs[1], _windowContext).AddComponent<ClampedFloatControl>().ReceiveSetting(setting);
                    break;
                case PartSetting.SettingType.Vector3:
                    Instantiate(_fieldPrefabs[2], _windowContext).AddComponent<Vector3Control>().ReceiveSetting(setting);
                    break;
                case PartSetting.SettingType.Vector3Int:
                    Instantiate(_fieldPrefabs[2], _windowContext).AddComponent<Vector3IntControl>().ReceiveSetting(setting);
                    break;
                case PartSetting.SettingType.ClampedVector3Int:
                    Instantiate(_fieldPrefabs[2], _windowContext).AddComponent<ClampedVector3IntControl>().ReceiveSetting(setting);
                    break;
            }
        }

        _windowContext.position = Input.mousePosition;
    }
}