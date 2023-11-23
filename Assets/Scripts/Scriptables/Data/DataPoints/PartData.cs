using ScriptableArchitecture.Data;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class PartData
    {
        public BasePartDataReference BasePart;

        public Vector3 Rotation;
        public int Material;
        public int Cost;
    }
}