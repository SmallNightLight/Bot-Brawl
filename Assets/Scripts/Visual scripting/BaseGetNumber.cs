public class BaseGetNumber : BaseGet
{
    public float Value;

    public virtual float GetNumber() => Value;

    public override object GetValue() => Value;
}