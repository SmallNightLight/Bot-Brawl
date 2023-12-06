using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultNodeSetSettingNumber", menuName = "Nodes/SetSetting/Number")]
public class SetSettingFloat : BaseDo
{
    public GetSettingsVariable VariableToChange;
    public GetSettingsVariable DefaultVariableToChange;

    public BaseGetNumber Value;
    public BaseGetNumber DefaultValue;

    public override void Do()
    {
        VariableToChange.Value.FloatValue = Value.Value;
    }

    public override List<BaseGet> GetInput() => new List<BaseGet> { VariableToChange, Value };

    public override List<BaseGet> GetDefaultInput() => new List<BaseGet> { DefaultVariableToChange, DefaultValue };

    public override void SetInput(List<BaseGet> input)
    {
        VariableToChange = input[0] as GetSettingsVariable;
        Value = input[1] as BaseGetNumber;
    }

    public override string[] GetBeforeNodeText() => new string[] { "Set", "to" };
}