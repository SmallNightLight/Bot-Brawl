using ScriptableArchitecture.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPart : MonoBehaviour
{
    [Header("Base")]
    [SerializeField] protected BoolReference _isPowered;
    [SerializeField] protected FixedJoint _fixedJoint;

    public void SetBaseBlock(Rigidbody rigidbody)
    {
        _fixedJoint.connectedBody = rigidbody;
    }
}