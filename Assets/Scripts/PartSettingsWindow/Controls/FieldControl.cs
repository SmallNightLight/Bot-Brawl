public abstract class FieldControl<T> : UnityEngine.MonoBehaviour
{
    protected PartSetting Setting { get; private set; }
    public void ReceiveSetting(PartSetting s) => Setting ??= s;
    protected abstract T GetValue();
    protected abstract void SetValue(T v);
}
