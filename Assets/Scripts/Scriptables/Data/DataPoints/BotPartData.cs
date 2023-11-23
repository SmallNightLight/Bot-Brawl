using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class BotPartData<T> : PartData where T : IPartSettings
    {
        public T PartSettings;
    }
}