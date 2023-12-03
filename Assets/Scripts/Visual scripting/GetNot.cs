using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultNodeNot", menuName = "Nodes/Not")]
public class GetNot : BaseGetBool
{
    [HideInInspector] public BaseGetBool Reference;
    public BaseGetBool DefaultReference;

    public override bool GetBool() => !Reference.GetBool();

    public override string GetBeforeNodeText() => "Not";

    public override List<BaseGet> GetInput() => new List<BaseGet> { Reference };

    public override List<BaseGet> GetDefaultInput() => new List<BaseGet> { DefaultReference };
}