using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultNodeEqual", menuName = "Nodes/Function")]
public class BaseFunction : BaseDo
{
    public string DisplayName;

    public override string GetNodeText() => DisplayName;

    public override bool CanSnapUnder() => false;

    public override bool HasScope() => false;

    public override bool IsSecondaryScope() => true;
}