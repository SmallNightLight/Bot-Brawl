using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [CreateAssetMenu(fileName = "DefaultNodeBool", menuName = "Nodes/Bool")]
    public class NodeBool : Node
    {
        public OperatorType ComparatorType;
        public NodeBool NodeBool1;
        public NodeBool NodeBool2;

        public NodeVariable Value;
        public NodeVariable Value2;

        public bool IsTrue()
        {
            switch (ComparatorType)
            {
                case OperatorType.Value:
                    return (bool)Value.ValueValue;
                case OperatorType.Equal:
                    return Value == Value2;
                case OperatorType.UnEqual:
                    return Value != Value2;
                case OperatorType.Bigger:
                    return Value > Value2;
                case OperatorType.Smaller:
                    return Value < Value2;
                case OperatorType.And:
                    return NodeBool1.IsTrue() && NodeBool2.IsTrue();
                case OperatorType.Or:
                    return NodeBool1.IsTrue() || NodeBool2.IsTrue();
                default:
                    return false;
            }
        }

        public enum OperatorType
        {
            Value,
            Or,
            And,

            Equal,
            UnEqual,

            Bigger,
            Smaller
        }
    }
}