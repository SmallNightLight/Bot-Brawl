using ScriptableArchitecture.Data;

public class NodeVariable : Node //Setup variables in a different window as global
{
    public VariableReferenceType VariableType;
    public bool FromUser;
    public bool IsValue;
    public string Name;

    public BoolReference BoolReference;
    public IntReference IntReference;
    public FloatReference FloatReference;

    public bool BoolValue;
    public int IntValue;
    public float FloatValue;

    public object ValueValue
    {
        get
        {
            if (IsValue)
            {
                return VariableType switch
                {
                    VariableReferenceType.Bool => BoolValue,
                    VariableReferenceType.Int => IntValue,
                    VariableReferenceType.Float => FloatReference,
                    _ => null
                };
            }
            else
            {
                return VariableType switch
                {
                    VariableReferenceType.Bool => BoolReference.Value,
                    VariableReferenceType.Int => IntReference.Value,
                    VariableReferenceType.Float => FloatReference.Value,
                    _ => null
                };
            }
        }
    }

    public object Value
    {
        get
        {
            if (IsValue)
            {
                return VariableType switch
                {
                    VariableReferenceType.Bool => BoolValue,
                    VariableReferenceType.Int => IntValue,
                    VariableReferenceType.Float => FloatReference,
                    _ => null
                };
            }
            else
            {
                return VariableType switch
                {
                    VariableReferenceType.Bool => BoolReference,
                    VariableReferenceType.Int => IntReference,
                    VariableReferenceType.Float => FloatReference,
                    _ => null
                };
            }
        }
        set
        {
            if (IsValue)
            {
                switch (value)
                {
                    case bool boolValue:
                        BoolValue = boolValue;
                        break;
                    case int intValue:
                        IntValue = intValue;
                        break;
                    case float floatValue:
                        FloatValue = floatValue;
                        break;
                }
            }
            else
            {
                switch (value)
                {
                    case BoolReference boolReference:
                        BoolReference = boolReference;
                        break;
                    case bool boolValue:
                        BoolReference.Value = boolValue;
                        break;
                    case IntReference intReference:
                        IntReference = intReference;
                        break;
                    case int intValue:
                        IntReference.Value = intValue;
                        break;
                    case FloatReference floatReference:
                        FloatReference = floatReference;
                        break;
                    case float floatValue:
                        FloatReference.Value = floatValue;
                        break;
                }
            }
        }
    }

    public static bool operator ==(NodeVariable variable1, NodeVariable variable2)
    {
        return variable1.Value == variable2.Value;
    }

    public static bool operator !=(NodeVariable variable1, NodeVariable variable2)
    {
        return variable1.Value != variable2.Value;
    }

    public static bool operator >(NodeVariable variable1, NodeVariable variable2)
    {
        object variable1Value = variable1.Value;
        object variable2Value = variable2.Value;

        if (variable1Value is IntReference && variable2Value is IntReference)
            return (variable1Value as IntReference).Value > (variable2Value as IntReference).Value;
        else if (variable1Value is FloatReference && variable2Value is FloatReference)
            return (variable1Value as FloatReference).Value > (variable2Value as FloatReference).Value;

        return false;
    }

    public static bool operator <(NodeVariable variable1, NodeVariable variable2)
    {
        object variable1Value = variable1.Value;
        object variable2Value = variable2.Value;

        if (variable1Value is IntReference && variable2Value is IntReference)
            return (variable1Value as IntReference).Value < (variable2Value as IntReference).Value;
        else if (variable1Value is FloatReference && variable2Value is FloatReference)
            return (variable1Value as FloatReference).Value < (variable2Value as FloatReference).Value;

        return false;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        return this == (NodeVariable)obj;
    }

    public override int GetHashCode()
    {
        return GetHashCode();
    }

    public enum VariableReferenceType
    {
        Bool,
        Int,
        Float
    }
}