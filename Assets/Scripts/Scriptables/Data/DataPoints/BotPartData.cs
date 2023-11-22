using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public struct BotPartData : IDataPoint
    {
        public BasePartDataReference BasePart;
        
        public int Material;
        public int Cost;
    }
}