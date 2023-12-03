using ScriptableArchitecture.Core;
using ScriptableArchitecture.Data;
using UnityEngine;

public abstract class ObjectPart : MonoBehaviour
{
    public PartData PartData;

    public virtual void SetPartData(PartData partData)
    { 
        PartData = partData;
        Setup();
    }

    protected virtual void Setup() { }
}