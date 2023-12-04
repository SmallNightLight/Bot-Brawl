using JetBrains.Annotations;
using System;
using System.Collections.Generic;

public class BaseDo : Node
{
    private List<BaseDo> _scope = new List<BaseDo>();

    //Execution
    public virtual void Execute()
    {
        Do();

        if (HasScope())
        {
            for (int i = 0; i < _scope.Count; i++)
            {
                BaseDo currentNode = _scope[i];
                //BaseDo nextNode = null;

                //if (i != _scope.Count - 1)
                //    nextNode = _scope[i + 1];

                currentNode.Execute();
                //if (nextNode == null || !(currentNode is NodeIf || currentNode is NodeElseIf) || !(nextNode is NodeElse || nextNode is NodeElseIf) || !(currentNode as NodeIf).ExecuteNextStatement())
                //    i++; //Skip else node
            }
        }
    }

    public void AddChild(BaseDo childDo)
    {
        _scope.Add(childDo);
    }

    public void RemoveAddChild(BaseDo childDo)
    {
        _scope.Remove(childDo);
    }

    public virtual void Do()
    {
    }

    public virtual List<BaseGet> GetInput()
    {
        return new List<BaseGet> { };
    }

    public virtual List<BaseGet> GetDefaultInput()
    {
        return new List<BaseGet> { };
    }

    public virtual bool HasScope() => false;

    public virtual string GetNodeText() => "";

    public virtual string[] GetBeforeNodeText()
    {
        int size = GetInput().Count;
        string[] emptyStringArray = new string[size];

        for (int i = 0; i < size; i++)
            emptyStringArray[i] = string.Empty;

        return emptyStringArray;
    }

    public virtual bool CanSnapAbove() => true;

    public virtual bool CanSnapUnder() => true;

    //Drawing
    public virtual List<Type> AttachTo()
    {
        return new List<Type>
        {
            typeof(BaseDo)
        };
    }
}