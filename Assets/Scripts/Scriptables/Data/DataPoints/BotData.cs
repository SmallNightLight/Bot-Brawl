using ScriptableArchitecture.Core;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class BotData : IDataPoint
    {
        private Dictionary<Vector3Int, BotPartData> _parts = new Dictionary<Vector3Int, BotPartData>();

        public void AddPart(Vector3Int position, BotPartData botPartData)
        {
            _parts.Add(position, botPartData);
        }

        public void ChangePart(Vector3Int position, BotPartData newPartData)
        {
            if (_parts.ContainsKey(position))
                _parts[position] = newPartData;
            else
                AddPart(position, newPartData);
        }

        public void RemovePart(Vector3Int position)
        {
            _parts.Remove(position);
        }

        public BotPartData GetPartData(Vector3Int position)
        {
            return _parts[position];
        }
    }
}