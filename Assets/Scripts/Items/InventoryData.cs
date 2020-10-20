using System.Collections.Generic;
using UnityEngine;

namespace RPG.Items
{
    [CreateAssetMenu(fileName = "InventoryData", menuName = "Item/Make New InventoryData", order = 0)]
    public class InventoryData : ScriptableObject
    {

        const string inventoryName = "InventoryData";
        public List<ItemInInventory> inventoryData;
    }
}