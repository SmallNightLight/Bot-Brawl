using ScriptableArchitecture.Data;
using System.Collections;
using System.Collections.Generic;

public class NodePart : Node
{
    public PartData PartData;

    public List<PartSetting> NewSettings = new List<PartSetting>();

    public override void ExecuteNode(VariableCollection variables)
    {
        PartData.Settings = NewSettings;
    }
}