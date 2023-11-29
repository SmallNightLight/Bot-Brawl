using System.Collections.Generic;
using UnityEngine;

public class UICategory : MonoBehaviour
{
    private List<Animator> _openCategories = new List<Animator>();

    private void Start()
    {
        foreach(Animator child in GetComponentsInChildren<Animator>(true))
            _openCategories.Add(child);
    }

    public void Raise(int index) => _openCategories[index].SetBool("Open", !_openCategories[index].GetBool("Open"));
}