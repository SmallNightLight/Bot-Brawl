using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultNodeSetVariable", menuName = "Nodes/SetVariable")]
public class SetCondition : BaseDo
{
    public GetBoolVariable VariableToChange;
    public GetBoolVariable DefaultVariableToChange;

    public BaseGetBool Value;
    public BaseGetBool DefaultValue;

    public override void Execute()
    {
        //Variable?.Value

        base.Execute();
    }

    public override List<BaseGet> GetInput() => new List<BaseGet> { VariableToChange, Value };

    public override List<BaseGet> GetDefaultInput() => new List<BaseGet> { DefaultVariableToChange, DefaultValue };

    public override void SetInput(List<BaseGet> input)
    {
        VariableToChange = input[0] as GetBoolVariable;
        Value = input[1] as BaseGetBool;
    }

    public override string[] GetBeforeNodeText() => new string[] { "Set condition", "to" };
}