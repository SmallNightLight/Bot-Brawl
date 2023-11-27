public class NodeSetVariable : Node
{
    public NodeVariable Variable;
    public NodeVariable NewVariable;

    public override void ExecuteNode(VariableCollection variables)
    {
        Variable.Value = NewVariable.Value;
    }
}