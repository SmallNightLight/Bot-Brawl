using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultNodeEqual", menuName = "Nodes/Function")]
public class BaseFunction : BaseDo
{
    public string FunctionName;

    public override string GetNodeText() => FunctionName;
}