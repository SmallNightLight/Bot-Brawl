using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    public class Node : ScriptableObject
    {
        public static int ID = 0;

        public List<Node> ScopeNodes = new List<Node>();

        //Drawing
        public Color DisplayColor;

        public List<NodeVariable> InputVariables = new List<NodeVariable>();

        public virtual void ExecuteChildren()
        {
            for (int i = 0; i < ScopeNodes.Count; i++)
            {
                Node currentNode = ScopeNodes[i];
                Node nextNode = null;

                if (i != ScopeNodes.Count - 1)
                    nextNode = ScopeNodes[i + 1];

                currentNode.ExecuteNode();
                currentNode.ExecuteChildren();

                if (nextNode == null || !(currentNode is NodeIf || currentNode is NodeElseIf) || !(nextNode is NodeElse || nextNode is NodeElseIf) || !(currentNode as NodeIf).ExecuteNextStatement())
                    i++; //Skip else node
            }
        }

        public virtual void ExecuteNode() { }

        public virtual bool HasScope => false;
    }
}