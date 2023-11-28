using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [CreateAssetMenu(fileName = "DefaultNodeIf", menuName = "Nodes/If")]
    public class NodeIf : Node
    {
        public NodeBool Condition;

        private bool _executeNextStatement; //For else and elseif

        public override void ExecuteChildren()
        {
            _executeNextStatement = !Condition.IsTrue();

            if (!_executeNextStatement)
                base.ExecuteChildren();
        }

        public bool ExecuteNextStatement()
        {
            return _executeNextStatement;
        }

        public override bool HasScope => true;
    }
}