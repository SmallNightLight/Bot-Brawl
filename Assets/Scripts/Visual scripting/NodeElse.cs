using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultNodeElse", menuName = "Nodes/Else")]
public class NodeElse : BaseDo
{
    public override string GetNodeText() =>  "Else" ;

    public override bool HasScope() => true;
}