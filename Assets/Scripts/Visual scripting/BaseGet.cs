using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGet : Node
{
    public abstract object GetValue();

    public virtual List<BaseGet> GetInput() => new List<BaseGet> { };

    public virtual List<BaseGet> GetDefaultInput() => new List<BaseGet>();

    public virtual void SetInput(List<BaseGet> input) { }

    public virtual string GetNodeText() => "";

    public virtual string[] GetBeforeNodeText()
    {
        int size = GetInput().Count;
        string[] emptyStringArray = new string[size];

        for (int i = 0; i < size; i++)
            emptyStringArray[i] = string.Empty;

        return emptyStringArray;
    }
}