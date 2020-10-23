using System.Collections;
using System.Collections.Generic;
using RPG.Items;
using UnityEngine;
using RPG.Control;
using System;
using UnityEngine.EventSystems;
using RPG.Stats;
using RPG.Base;

namespace RPG.UI{

    public class UIItemData : MonoBehaviour
    {
        public Item uiItemInInventory;

        public void SetItemData (Item itemInInventory)
        {
            uiItemInInventory = itemInInventory;
            
        }
        public Item GetItemData ()
        {
            return uiItemInInventory;
        }
        public void SetEquipped(bool state)
        {
            uiItemInInventory.EquipItem(state);
        }
        public ItemConfig GetInventoryItemConfig()
        {
            return uiItemInInventory.itemObject;
        }


    }
}
