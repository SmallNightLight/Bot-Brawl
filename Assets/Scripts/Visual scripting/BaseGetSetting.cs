using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGetSetting : BaseGet
{
    public PartSetting Value;

    public override object GetValue() => Value;
}