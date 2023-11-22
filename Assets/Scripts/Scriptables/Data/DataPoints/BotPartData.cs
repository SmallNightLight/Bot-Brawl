using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public struct BotPartData : IDataPoint
    {
        public BasePartDataReference BasePart;
        public Vector3Int Position;

        int Cost;

    }
}