using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Default", menuName = "Scriptables/Variables/Parts/Default")]
    public class BotPartDataD : PartData
    {
    }

    [System.Serializable]
    public class BotPartData<T> : PartData where T : IPartSettings
    {
        public T PartSettings;
    }
}