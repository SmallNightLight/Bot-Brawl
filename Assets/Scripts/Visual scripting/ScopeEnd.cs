using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopeEnd : MonoBehaviour
{
    [HideInInspector] public static List<RectTransform> ALLSCOPEENDS = new List<RectTransform>();
    [HideInInspector] public bool IsMoving;

    [HideInInspector] public DisplayDo ParentDisplayDo;

    private void Start()
    {
        ALLSCOPEENDS.Add(GetComponent<RectTransform>());

        ParentDisplayDo = GetComponentInParent<DisplayDo>();
    }
}