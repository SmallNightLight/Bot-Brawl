using ScriptableArchitecture.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using System.Linq;

namespace ScriptableArchitecture.Data
{
    [Serializable]
    public class BotData : IDataPoint
    {
        public TBaseSO D;

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

        public bool TryGetPartData(Vector3Int position, out BotPartDataReference partData)
        {
            if (_parts.TryGetValue(position, out partData))
                return true;
            else
            {
                partData = null;
                return false;
            }
        }

        public List<KeyValuePair<Vector3Int, BotPartDataReference>> GetParts()
        {
            return _parts.ToList();
        }
    }
}