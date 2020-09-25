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
        public ItemInInventory uiItemInInventory;

        public void SetItemData (ItemInInventory itemInInventory)
        {
            uiItemInInventory = itemInInventory;
            
        }
        public ItemInInventory GetItemData ()
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
