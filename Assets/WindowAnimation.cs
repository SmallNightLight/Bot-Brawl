using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WindowAnimation : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void EnableWindow()
    {
        _animator.SetBool("IsActive", true);
    }

    public void DisableWindow()
    {
        _animator.SetBool("IsActive", false);
    }
}