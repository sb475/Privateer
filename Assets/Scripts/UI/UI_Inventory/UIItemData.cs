using System.Collections;
using System.Collections.Generic;
using RPG.Items;
using UnityEngine;
using RPG.Control;
using System;
using UnityEngine.EventSystems;
using RPG.Stats;
using RPG.Base;
using UnityEngine.UI;

namespace RPG.UI{

    public class UIItemData : MonoBehaviour
    {
        public Item uiItem;
        Image image;


        public void SetItemData (Item item)
        {
            image = GetComponent<Image>();
            uiItem = item;
            image.sprite = item.itemObject.itemIcon;
        }
        public Item GetItemData ()
        {
            return uiItem;
        }
        public void SetEquipped(bool state)
        {
            uiItem.EquipItem(state);
        }
        public ItemConfig GetInventoryItemConfig()
        {
            return uiItem.itemObject;
        }


    }
}
