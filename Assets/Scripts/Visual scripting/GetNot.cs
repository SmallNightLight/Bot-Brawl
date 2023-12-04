using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultNodeNot", menuName = "Nodes/Not")]
public class GetNot : BaseGetBool
{
    public BaseGetBool Reference;
    public BaseGetBool DefaultReference;

    public override bool GetBool() => !Reference.GetBool();

    public override string GetNodeText() => "Not";

    public override List<BaseGet> GetInput() => new List<BaseGet> { Reference };

    public override List<BaseGet> GetDefaultInput() => new List<BaseGet> { DefaultReference };

    public override void SetInput(List<BaseGet> input)
    {
        Reference = input[0] as BaseGetBool;
    }
}