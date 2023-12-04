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
            for (int i = 0; i < _scope.Count; i++)
                _scope[i].Execute();
    }

    public void AddChild(BaseDo childDo)
    {
        _scope.Add(childDo);
    }

    public void RemoveAddChild(BaseDo childDo)
    {
        _scope.Remove(childDo);
    }

    public virtual void Do() { }

    public virtual List<BaseGet> GetInput() => new List<BaseGet> { };

    public virtual List<BaseGet> GetDefaultInput() => new List<BaseGet> { };

    public virtual void SetInput(List<BaseGet> input) { }

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