using ScriptableArchitecture.Core;
using ScriptableArchitecture.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeVariable : Node //Setup variables in a different window as global
{
    public VariableReferenceType VariableType;
    public bool FromUser;
    public string Name;

    public BoolReference BoolReference;
    public IntReference IntReference;
    public FloatReference FloatReference;

    public enum VariableReferenceType
    {
        Bool,
        Int,
        Float
    }
}