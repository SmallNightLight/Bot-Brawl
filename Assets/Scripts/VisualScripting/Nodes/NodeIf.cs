using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeIf : Node
{
    public NodeBool Condition;

    public override void Execute(VariableCollection variables)
    {
        if (Condition.IsTrue())
            base.Execute(variables);
    }
}