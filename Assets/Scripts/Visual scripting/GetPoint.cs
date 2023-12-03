using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPoint : MonoBehaviour
{
    [HideInInspector] public Color BaseColor;

    [HideInInspector] public BaseGet DefaultGet;
    [HideInInspector] public DisplayGet ChildGet;
    
    public DisplayDo MainDo
    {
        get
        {
            return GetComponentInParent<DisplayDo>();
        }
    }

    [HideInInspector] public static List<RectTransform> ALLGETPOINTS = new List<RectTransform>();
    [HideInInspector] public bool IsMoving;

    private void Start()
    {
        ALLGETPOINTS.Add(GetComponent<RectTransform>());

        BaseColor = GetComponent<Image>().color;
    }

    private void OnDestroy()
    {
        ALLGETPOINTS.Remove(GetComponent<RectTransform>());
    }

    public bool HasChild()
    {
        return ChildGet != null;
    }
}