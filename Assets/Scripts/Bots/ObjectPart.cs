using ScriptableArchitecture.Core;
using ScriptableArchitecture.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPart : MonoBehaviour
{
    [Header("Base")]
    public BoolReference _isPowered;
    public PartData PartData;

    public virtual void SetPartData(PartData partData)
    { 
        PartData = partData;
        Setup();
    }

    protected virtual void Setup() { }
}

public abstract class ObjectPart<T> : ObjectPart where T : IPartSettings
{
    public T PartSettings;

    public override void SetPartData(PartData partData)
    {
        PartData = partData;
        var data = PartData as BotPartData<T>;

        if (data != null)
            PartSettings = data.PartSettings;
        else
            Debug.LogWarning("Couldnt convert PartData: " + partData.name);

        Setup();
    }
}