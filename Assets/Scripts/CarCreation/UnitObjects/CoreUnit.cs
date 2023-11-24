using System;
using System.Collections.Generic;
using UnityEngine;

namespace CarCreation.UnitObjects
{
    public class CoreUnit : MonoBehaviour
    {
        [SerializeField] protected ScriptableObject unitConfig;
        private Dictionary<Vector3, CoreUnit> _attachedUnits;
        
        /* Initialize other properties from unitConfig here */
        public HashSet<Vector3> AttachableFaces;
        private int _healthPoints;
        protected int HealthPoints 
        {
            get => _healthPoints;
            set { if(value is >= 0 and <= 200) _healthPoints = value; } 
        }

        public bool Attached(CoreUnit other)
        {
            return _attachedUnits.ContainsKey(other.transform.position);
        }
        private bool IsAttachableTo(CoreUnit other)
        {
            throw new NotImplementedException();
        }
        private void OnewayAttach(CoreUnit other)
        {
            if (!Attached(other) && IsAttachableTo(other))
            {
                _attachedUnits.Add(other.transform.position, other);
            }
        }
        public void Attach(CoreUnit other)
        {
            OnewayAttach(other);
            other.OnewayAttach(this);
        }
        private void Detach(CoreUnit other)
        {
            _attachedUnits.Remove(other.transform.position);
        }

        public void RotateX()
        {
            transform.Rotate(transform.right * 90);
        }
        public void RotateY()
        {
            transform.Rotate(transform.up * 90);
        }
        public void RotateZ()
        {
            transform.Rotate(transform.forward * 90);
        }

        private void OnDestroy()
        {
            foreach(CoreUnit unit in _attachedUnits.Values)
            {
                unit.Detach(this);
            }
        }
        
        
    }
}
