using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseComparator : BaseGetBool
{
    [HideInInspector] public BaseGet Reference1;
    [HideInInspector] public BaseGet Reference2;

    public BaseGet DefaultReference1;
    public BaseGet DefaultReference2;

    public override bool GetBool()
    {
        object value1 = Reference1.GetValue();
        object value2 = Reference2.GetValue();

        if (value1.GetType() != value2.GetType())
            return false;

        return GetComparator(value1, value2);
    }

    public abstract bool GetComparator(object value1, object value2);

    public override List<BaseGet> GetInput(){ return new List<BaseGet> { Reference1, Reference2}; }

    public override List<BaseGet> GetDefaultInput() { return new List<BaseGet> { DefaultReference1, DefaultReference2 }; }
}