using ScriptableArchitecture.Data;

public class NodeBool : Node
{
    public OperatorType ComparatorType;
    public NodeVariable.VariableReferenceType ComparatorVariableType;
    public NodeBool NodeBool1;
    public NodeBool NodeBool2;

    public BoolReference Value;

    public BoolReference BoolFirst;
    public BoolReference BoolSecond;

    public IntReference IntFirst;
    public IntReference IntSecond;

    public FloatReference FloatFirst;
    public FloatReference FloatSecond;

    public bool IsTrue()
    {
        switch (ComparatorVariableType)
        {
            case NodeVariable.VariableReferenceType.Bool:
                switch (ComparatorType)
                {
                    case OperatorType.Value:
                        return Value.Value;
                    case OperatorType.Equal:
                        return BoolFirst == BoolSecond;
                    case OperatorType.UnEqual:
                        return BoolFirst != BoolSecond;
                    case OperatorType.And:
                        return NodeBool1.IsTrue() && NodeBool2.IsTrue();
                    case OperatorType.Or:
                        return NodeBool1.IsTrue() || NodeBool2.IsTrue();
                    default:
                        return false;
                }
            case NodeVariable.VariableReferenceType.Int:
                switch (ComparatorType)
                {
                    case OperatorType.Equal:
                        return IntFirst.Value == IntSecond.Value;
                    case OperatorType.UnEqual:
                        return IntFirst.Value != IntSecond.Value;
                    case OperatorType.Bigger:
                        return IntFirst.Value > IntSecond.Value;
                    case OperatorType.Smaller:
                        return IntFirst.Value < IntSecond.Value;
                    default:
                        return false;
                }
            case NodeVariable.VariableReferenceType.Float:
                switch (ComparatorType)
                {
                    case OperatorType.Equal:
                        return FloatFirst.Value == FloatSecond.Value;
                    case OperatorType.UnEqual:
                        return FloatFirst.Value != FloatSecond.Value;
                    case OperatorType.Bigger:
                        return FloatFirst.Value > FloatSecond.Value;
                    case OperatorType.Smaller:
                        return FloatFirst.Value < FloatSecond.Value;
                    default:
                        return false;
                }
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