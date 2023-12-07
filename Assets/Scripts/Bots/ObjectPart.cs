using ScriptableArchitecture.Core;
using ScriptableArchitecture.Data;
using UnityEngine;

public abstract class ObjectPart : MonoBehaviour
{
    public PartData PartData;

    [SerializeField] private bool _dealsDamage;

    public bool IsActive;

    public virtual void SetPartData(PartData partData)
    { 
        PartData = partData;
        Setup();
    }

    protected virtual void Setup() { }
}