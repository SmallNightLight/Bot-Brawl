using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [CreateAssetMenu(fileName = "BotPartWheelGameEvent", menuName = "Scriptables/GameEvents/BotParts/Wheel")]
    public class BotPartWheelGameEvent : GameEventBase<BotPartData<WheelPartSettings>>
    {
    }
}