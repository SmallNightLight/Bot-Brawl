using UnityEngine;

[CreateAssetMenu(fileName = "DefaultGetBool", menuName = "Nodes/Get/Bool")]
public class BaseGetBool : BaseGet
{
    public bool Value;

    public virtual bool GetBool() => Value;

    public override object GetValue() => Value;
}