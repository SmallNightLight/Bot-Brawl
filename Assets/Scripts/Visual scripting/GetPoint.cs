using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GetPoint : MonoBehaviour
{
    [HideInInspector] public Color BaseColor;

    public BaseGet DefaultGet;

    private DisplayGet _childGet;

    private Gettype _type;

    private enum Gettype
    {
        Condition,
        Number,
        Part
    }

    [HideInInspector] public DisplayGet ChildGet
    {
        get
        {
            return _childGet;
        }
        set
        {
            _childGet = value;

            if (_type == Gettype.Number)
                ReloadInputField();
        }
    }

    bool IsDerivedFrom(object obj, Type baseType)
    {
        return obj is BaseGet && obj.GetType().IsSubclassOf(baseType);
    }

    [SerializeField] private TMP_InputField _numberInputField;

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

        if (DefaultGet == null)
        {
            _type = Gettype.Condition;
            return;
        }

        if (IsDerivedFrom(DefaultGet, typeof(BaseGetNumber)))
        {
            _type = Gettype.Number;
            ReloadInputField();
        }
        else if (IsDerivedFrom(DefaultGet, typeof(BaseGetBool)))
        {
            _type = Gettype.Condition;
        }
        else
            _type = Gettype.Part;
    }

    private void ReloadInputField()
    {
        bool current = false;

        DisplayGet mainGet = GetComponentInParent<DisplayGet>();
        DisplayDo mainDo = MainDo;

        if (mainGet != null)
            current = !mainGet.IsDefaultNode;
        else if (mainDo != null)
            current = !mainDo.IsDefaultNode;

        current &= ChildGet == null && _type == Gettype.Number;
        _numberInputField.gameObject.SetActive(current);
    }

    private void OnDestroy()
    {
        ALLGETPOINTS.Remove(GetComponent<RectTransform>());
    }

    public bool HasChild()
    {
        return ChildGet != null;
    }

    public float GetNumberValue()
    {
        return float.Parse(_numberInputField.text.Replace(',', '.'));
    }

    public bool IsNumber()
    {
        return _type == Gettype.Number;
    }
}