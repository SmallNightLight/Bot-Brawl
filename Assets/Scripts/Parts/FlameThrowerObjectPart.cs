using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerObjectPart : ObjectPart
{
    [SerializeField] private Animator _animator;


    void Update()
    {
        _animator.SetBool("IsOn", PartData.GetBool("IsOn"));
    }
}