using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public struct PlacingInfo : IDataPoint
    {
        public Vector3Int AttachPoint;
        public Vector3Int Normal;
        public PartData OtherPart;
    }
}