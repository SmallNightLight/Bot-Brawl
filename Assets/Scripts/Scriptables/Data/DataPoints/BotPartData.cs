using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public struct BotPartData<T> : IDataPoint
    {
        public BasePartDataReference BasePart;
        
        public Vector3 Rotation;
        public int Material;
        public int Cost;

        public T PartSettings;
    }
}