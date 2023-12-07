using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerObjectPart : ObjectPart
{
    [SerializeField] private Animator _animator;

    private void Start()
    {
        _animator.SetBool("IsOn", false);
    }


    void Update()
    {
        if (IsActive)
            _animator.SetBool("IsOn", PartData.GetBool("IsOn"));
    }
}