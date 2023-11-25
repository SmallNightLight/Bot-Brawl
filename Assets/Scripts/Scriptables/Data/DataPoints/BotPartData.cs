using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public abstract class BotPartData<T> : PartData where T : IPartSettings
    {
        public T PartSettings;
    }
}