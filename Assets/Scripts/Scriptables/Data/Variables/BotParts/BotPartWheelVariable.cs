using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [CreateAssetMenu(fileName = "BotPartWheelVariable", menuName = "Scriptables/Variables/BotParts/Wheel")]
    public class BotPartWheelVariable : Variable<BotPartData<WheelPartSettings>>
    {
    }
}