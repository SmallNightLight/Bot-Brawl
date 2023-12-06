using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultNodeSetSettingBool", menuName = "Nodes/SetSetting/Bool")]
public class SetSettingBool : BaseDo
{
    public GetSettingsVariable VariableToChange;
    public GetSettingsVariable DefaultVariableToChange;

    public BaseGetBool Value;
    public BaseGetBool DefaultValue;

    public override void Do()
    {
        VariableToChange.Value.BoolValue = Value.Value;
    }

    public override List<BaseGet> GetInput() => new List<BaseGet> { VariableToChange, Value };

    public override List<BaseGet> GetDefaultInput() => new List<BaseGet> { DefaultVariableToChange, DefaultValue };

    public override void SetInput(List<BaseGet> input)
    {
        VariableToChange = input[0] as GetSettingsVariable;
        Value = input[1] as BaseGetBool;
    }

    public override string[] GetBeforeNodeText() => new string[] { "Set", "to" };
}