using System.Collections.Generic;
using CarCreation.UnitObjects;
using UnityEngine;

namespace CarCreation
{
    [CreateAssetMenu(fileName = "New Inventory Item", menuName = "Custom Objects/Inventory Item")]
    public class InventoryItem : ScriptableObject
    {
        public KeyCode key;
        public List<CoreUnit> units = new List<CoreUnit>();
    }
}
