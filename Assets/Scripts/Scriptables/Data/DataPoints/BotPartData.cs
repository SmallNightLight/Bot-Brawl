using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class BotPartData : PartData
    {
    }

    [System.Serializable]
    public class BotPartData<T> : BotPartData where T : IPartSettings
    {
        public T PartSettings;
    }
}