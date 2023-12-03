using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultNodeIf", menuName = "Nodes/If")]
public class NodeIf : BaseDo
{
    [HideInInspector] public BaseGetBool Condition;
    public BaseGetBool DefaultCondition;

    public override void Execute()
    {
        if (Condition.GetBool())
            base.Execute();
    }

    public override List<BaseGet> GetInput() => new List<BaseGet> { Condition };

    public override List<BaseGet> GetDefaultInput() => new List<BaseGet> { DefaultCondition };

    public override string[] GetBeforeNodeText() => new string[] { "If" };

    public override bool HasScope() => true;
}