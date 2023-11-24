using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [CreateAssetMenu(fileName = "IntVariable", menuName = "Scriptables/Variables/Int")]
    public class IntVariable : Variable<int>
    {
        public int containerDimensions = 4;
    }
}