using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [CreateAssetMenu(fileName = "DefaultNodeFunction", menuName = "Nodes/Function")]
    public class NodeFunction : Node
    {
        public override bool HasScope => true;
    }
}