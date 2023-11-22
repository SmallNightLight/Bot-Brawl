using ScriptableArchitecture.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace ScriptableArchitecture.Data
{
    [Serializable]
    public class BotData : IDataPoint
    {
        [SerializeField, SerializedDictionary("Position", "Data")]
        private SerializedDictionary<Vector3Int, BotPartDataReference> _parts;

        public void AddPart(Vector3Int position, BotPartDataReference botPartData)
        {
            _parts.Add(position, botPartData);
        }

        public void ChangePart(Vector3Int position, BotPartDataReference newPartData)
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

        public BotPartDataReference GetPartData(Vector3Int position)
        {
            return _parts[position];
        }
    }
}