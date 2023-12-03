using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGet : Node
{
    public abstract object GetValue();

    public virtual List<BaseGet> GetInput()
    {
        return new List<BaseGet> { };
    }

    public virtual List<BaseGet> GetDefaultInput()
    {
        return new List<BaseGet>();
    }

    public virtual string GetBeforeNodeText()
    {
        return "";
    }
}