using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class WheelPartSettings : IPartSettings
    {
        public float Power = 150f;
        public float MaxAngle = 30f;
        public float Offset = 0f;
        public bool Inverted = false;
    }
}