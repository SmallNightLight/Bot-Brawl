using ScriptableArchitecture.Data;
using System.Collections.Generic;
using UnityEngine;

namespace CarCreation
{
    public class CarRender : MonoBehaviour
    {
        // Make sure you rotate the instantiated unit and not only the unit object its self
        private readonly Dictionary<Vector3, GameObject> _parts = new Dictionary<Vector3, GameObject>();

        //For now just one prefab
        [SerializeField] private GameObject _defaultPrefab;
        [SerializeField] private BotDataReference _botData;

        private void Start()
        {
            //Load bot
            foreach (var part in _botData.Value.GetParts())
                Add(part.Value);
        }

        //Just to make it work with the data
        public void Add(PartData partData)
        {
            Vector3 partPosition = partData.Position;
            Vector3 partRotation = partData.Rotation;

            Instantiate(_defaultPrefab, partPosition, Quaternion.Euler(partRotation.x, partRotation.y, partRotation.z));
        }

        public void Add(Vector3 v, GameObject u)
        {
            _parts.Add(v, Instantiate(u, u.transform.position, u.transform.rotation));
        }
        public void Remove(Vector3 v)
        {
            if(!_parts.ContainsKey(v)) return;
            Destroy(_parts[v]);
            _parts.Remove(v);
        }
        
    }
}