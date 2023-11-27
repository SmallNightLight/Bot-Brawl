using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeIf : Node
{
    public NodeBool Condition;

    private bool _executeNextStatement; //For else and elseif

    public override void ExecuteChildren(VariableCollection variables)
    {
        _executeNextStatement = !Condition.IsTrue();

        if (!_executeNextStatement)
            base.ExecuteChildren(variables);
    }

    public bool ExecuteNextStatement()
    {
        return _executeNextStatement;
    }
}