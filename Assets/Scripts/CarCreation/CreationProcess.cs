using System.Collections.Generic;
using UnityEngine;

namespace CarCreation
{
    public class CreationProcess : MonoBehaviour
    {
        [SerializeField] private GameObject cursorPrefab;
        [SerializeField] private CarContainer carContainer;
        private GameObject _cursor;
        [SerializeField] private Inventory inventory;
        //[SerializeField] private List<InventoryItem> inventory = new List<InventoryItem>();
        
        private void OnEnable()
        {
            //GridCursor.CursorMoved += func;
        }
        private void OnDisable()
        {
            //GridCursor.CursorMoved -= func;
        }
        
        private void Awake()
        {
            if (cursorPrefab != null) _cursor = Instantiate(cursorPrefab, Vector3.zero, Quaternion.identity);
            
        }
        
        private void Update()
        {
            
        }
        
    }
}
