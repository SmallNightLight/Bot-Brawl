using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class WheelPartSettings : IDataPoint
    {
        public float Power = 150f;
        public float MaxAngle = 90f;
        public float Offset = 0f;
    }
}