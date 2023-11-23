using ScriptableArchitecture.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPart : MonoBehaviour
{
    [Header("Base")]
    [SerializeField] protected BoolReference _isPowered;

    public virtual void Setup(BotPartData partData){ }
}