using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [CreateAssetMenu(fileName = "DefaultNodeElse", menuName = "Nodes/Else")]
    public class NodeElse : Node //Should be after If or ElseIf node
    {
        public override bool HasScope => true;
    }
}