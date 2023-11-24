using System.Collections.Generic;
using UnityEngine;
using System;

namespace CarCreation
{
    public class GridCursor : MonoBehaviour
    {
        public static event Action<Vector3> CursorMoved;
        // private const int ContainerDim = 6;
        private GameObject _cursorObject;
        private Vector3 _currPosition;
        private Vector3 _prevPosition;
        private Dictionary<KeyCode, Vector3> _keyMappings;

        private void Awake()
        {
            _prevPosition = transform.position;
            
            _keyMappings = new Dictionary<KeyCode, Vector3>
            {
                { KeyCode.W, Vector3.forward },
                { KeyCode.S, Vector3.back },
                { KeyCode.D, Vector3.right },
                { KeyCode.A, Vector3.left },
                { KeyCode.E, Vector3.up },
                { KeyCode.Q, Vector3.down }
            };
        }
    
        private void Update()
        {
            _currPosition = transform.position;
            _prevPosition = _currPosition;
            foreach (var kvp in _keyMappings)
            {
                if (Input.GetKeyDown(kvp.Key)) _currPosition += kvp.Value;
            }
            // _currPosition = new Vector3(
            //     Mathf.Clamp(_currPosition.x, 0, ContainerDim - 1),
            //     Mathf.Clamp(_currPosition.y, 0, ContainerDim - 1),
            //     Mathf.Clamp(_currPosition.z, 0, ContainerDim - 1)
            // );
            
            transform.position = _currPosition;
            
            if (!_currPosition.Equals(_prevPosition)) CursorMoved?.Invoke(_currPosition - _prevPosition);
            
        }
        
        
    }
}