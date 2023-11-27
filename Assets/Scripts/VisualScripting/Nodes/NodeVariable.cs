using ScriptableArchitecture.Data;

public class NodeVariable : Node //Setup variables in a different window as global
{
    public VariableReferenceType VariableType;
    public bool FromUser;
    public string Name;

    public BoolReference BoolReference;
    public IntReference IntReference;
    public FloatReference FloatReference;

    public object Value
    {
        get
        {
            return VariableType switch
            {
                VariableReferenceType.Bool => BoolReference,
                VariableReferenceType.Int => IntReference,
                VariableReferenceType.Float => FloatReference,
                _ => null
            };
        }
        set
        {
            switch(value)
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

    public enum VariableReferenceType
    {
        Bool,
        Int,
        Float
    }
}