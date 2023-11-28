using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [CreateAssetMenu(fileName = "DefaultNodeSetVariable", menuName = "Nodes/SetVariable")]
    public class NodeSetVariable : Node
    {
        public NodeVariable Variable;
        public NodeVariable NewVariable;

        public override void ExecuteNode()
        {
            Variable.Value = NewVariable.Value;
        }
    }
}