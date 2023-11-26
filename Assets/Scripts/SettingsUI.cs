using ScriptableArchitecture.Data;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    public BoolReference _isPlacing;
    public GameObject _buttons;

    void Update()
    {
        //Change for more behaviour
        _buttons.SetActive(!_isPlacing.Value);
    }
}