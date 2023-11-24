using System;
using System.Collections.Generic;
using CarCreation.UnitObjects;
using UnityEngine;

namespace CarCreation
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<InventoryItem> inventory = new List<InventoryItem>();
        private Dictionary<KeyCode, List<CoreUnit>> _inventory = new Dictionary<KeyCode, List<CoreUnit>>();
        private CoreUnit _selectedUnit;

        private void Awake()
        {
            foreach (InventoryItem item in inventory)
            {
                KeyCode key = item.key;
                List<CoreUnit> unitList = new List<CoreUnit>(); 
                
                foreach (CoreUnit unit in item.units)
                {
                    CoreUnit copiedUnit = Instantiate(unit); // Don't instantiate it here!
                    unitList.Add(copiedUnit);
                }
                
                _inventory.Add(key, unitList); 
            }
        }

        public CoreUnit Equip()
        {
            throw new NotImplementedException();
        }
        public void Unequip()
        {
            
        }
        
        
    }
}