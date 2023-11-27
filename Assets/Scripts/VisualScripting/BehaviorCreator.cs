using UnityEngine;

public class BehaviorCreator : MonoBehaviour
{
    public Node BaseNode;
    public VariableCollection Variables;

    public void Execute(VariableCollection variables)
    {
        BaseNode.ExecuteChildren(variables);
    }
}