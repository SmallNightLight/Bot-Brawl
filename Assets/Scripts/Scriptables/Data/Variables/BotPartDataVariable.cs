using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [CreateAssetMenu(fileName = "BotPartWheelDataVariable", menuName = "Scriptables/Variables/BotPartData/Wheel")]
    public class BotPartDataVariable : Variable<BotPartData<WheelPartSettings>>
    {
    }
}