using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : ScriptableObject
{
    public List<Node> _scopeNodes = new List<Node>();
    public virtual void Execute(VariableCollection variables)
    {
        for (int i = 0; i < _scopeNodes.Count; i++)
        {
            _scopeNodes[i].Execute(variables);
        }
    }
}