using System.Collections.Generic;
using UnityEngine;

namespace CarCreation
{
    public class CarRender : MonoBehaviour
    {
        // Make sure you rotate the instantiated unit and not only the unit object its self
        private readonly Dictionary<Vector3, GameObject> _parts = new Dictionary<Vector3, GameObject>();
        
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