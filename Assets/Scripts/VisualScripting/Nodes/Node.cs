using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : ScriptableObject
{
    public List<Node> ScopeNodes = new List<Node>();

    //Drawing
    public Color _displayColor;
    

    public virtual void ExecuteChildren(VariableCollection variables)
    {
        for (int i = 0; i < ScopeNodes.Count; i++)
        {
            Node currentNode = ScopeNodes[i];
            Node nextNode = null; 

            if (i != ScopeNodes.Count - 1)
                nextNode = ScopeNodes[i + 1];

            currentNode.ExecuteNode(variables);
            currentNode.ExecuteChildren(variables);

            if (nextNode == null || !(currentNode is NodeIf || currentNode is NodeElseIf) || !(nextNode is NodeElse || nextNode is NodeElseIf) || !(currentNode as NodeIf).ExecuteNextStatement())
                i++; //Skip else node
        }
    }

    public virtual void ExecuteNode(VariableCollection variables) { }
}