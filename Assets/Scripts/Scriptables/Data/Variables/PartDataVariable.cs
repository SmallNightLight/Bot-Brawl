using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [CreateAssetMenu(fileName = "PartDataVariable", menuName = "Scriptables/Variables/PartDataVariable")]
    public class PartDataVariable : Variable<PartData>
    {
        public void SetName(string name)
        {
            Value.SetName(name);
        }

        public void SetBool(bool value)
        {
            Value.SetBool(value);
        }

        public void SetInt(int value)
        {
            Value.SetInt(value);
        }

        public void SetFloat(float value)
        {
            Value.SetFloat(value);
        }
    }
}