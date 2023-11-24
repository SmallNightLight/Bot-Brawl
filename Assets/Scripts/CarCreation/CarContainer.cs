using System;
using System.Collections.Generic;
using CarCreation.UnitObjects;
using UnityEngine;

namespace CarCreation
{
    public class CarContainer : MonoBehaviour
    {
        private readonly Dictionary<Vector3, CoreUnit> _units = new Dictionary<Vector3, CoreUnit>();
        
        public void AddUnit(Vector3 v, CoreUnit u)
        {
            _units.Add(v, u);
        }
        public void RemoveUnit(Vector3 v)
        {
            _units.Remove(v);
        }
        public CoreUnit GetUnit(Vector3 v)
        {
            return _units[v];
        }
        
        public void Serialize()
        {
            throw new NotImplementedException();
        }
        
        
    }
}