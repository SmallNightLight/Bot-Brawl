using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultNodeSetSetting", menuName = "Nodes/SetSetting")]
public class SetSetting : BaseDo
{
    public GetSettingsVariable VariableToChange;
    public GetSettingsVariable DefaultVariableToChange;

    public BaseGetSetting Value;
    public BaseGetSetting DefaultValue;

    public override void Execute()
    {
        //Variable?.Value

        base.Execute();
    }

    public override List<BaseGet> GetInput() => new List<BaseGet> { VariableToChange, Value };

    public override List<BaseGet> GetDefaultInput() => new List<BaseGet> { DefaultVariableToChange, DefaultValue };

    public override void SetInput(List<BaseGet> input)
    {
        VariableToChange = input[0] as GetSettingsVariable;
        Value = input[1] as BaseGetSetting;
    }

    public override string[] GetBeforeNodeText() => new string[] { "Set", "to" };
}