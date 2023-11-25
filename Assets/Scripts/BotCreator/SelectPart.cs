using ScriptableArchitecture.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPart : MonoBehaviour
{
    [SerializeField] private BasePartDataReferenceGameEvent _selectedBasePartEvent;
    [SerializeField] private BasePartDataReference _newBasePartData;

    public void Raise()
    {
        _selectedBasePartEvent.Raise(_newBasePartData);
    }
}