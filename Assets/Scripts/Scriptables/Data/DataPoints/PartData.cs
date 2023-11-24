using ScriptableArchitecture.Data;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public abstract class PartData : ScriptableObject
    {
        public BasePartDataReference BasePart;

        public Vector3 Rotation;
        public int Material;
        public int Cost;

        [HideInInspector] public Vector3Int Position;
    }
}