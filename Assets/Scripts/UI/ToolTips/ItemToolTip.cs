using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Base;
using RPG.Items;
using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI {
    
    public class ItemToolTip : MonoBehaviour
    {

        private static ItemToolTip instance;
        [SerializeField] private Camera uiCamera;
        [SerializeField] private RectTransform canvasRectTransform;
        [SerializeField] private RectTransform backgroundRectTransform;
        [SerializeField] private Text itemName;
        [SerializeField] private Text itemDescription;

        [Header("Item Stat Display Variables")]
        [SerializeField] private GameObject displayItemStatsCell;
        [SerializeField] private GameObject statDisplayContainer;

        [Header("Currently Equipped Stat Display Variables")]
        [SerializeField] private GameObject currentDisplayItemStatsCell;
        [SerializeField] private GameObject currentStatDisplayContainer;

        [Header("If viewing item in shop")]
        [SerializeField] private Text itemCost;

        private void Awake() {
            instance = this;
            gameObject.SetActive(false);
             
        }

        private void Update() {
            Vector2 localpoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localpoint);
            transform.localPosition = localpoint;

            Vector2 anchoredPosition = transform.GetComponent<RectTransform>().anchoredPosition;

            //if the tooltip goes past the edge of our canvas, change the anchored position so that it fits.
            if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
            {
                
                anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
            }
            //if it goes to far left, move tooltip to the right
            else if (anchoredPosition.x < backgroundRectTransform.rect.width + 40f)
            {

                anchoredPosition.x = anchoredPosition.x + backgroundRectTransform.rect.width;
            }

            transform.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        }

        // Start is called before the first frame update
        private void ShowToolTip(UIItemData itemData, UIInventory parentInventory)
        {

            if (itemData == null) return;
            gameObject.SetActive(true);

           
            ItemConfig item = itemData.uiItemInInventory.itemObject;
            
            itemName.text = item.GetName();
            itemDescription.text =item.GetDescription();

            DisplayItemStats(item, displayItemStatsCell, statDisplayContainer);

            if (parentInventory.GetType() == typeof(UIShipCargo)) {

                CompareEquippedItemInSlot(itemData, parentInventory);

            }
            
            if (parentInventory.GetType() == typeof(UIShopInventory))
            {
                //DisplayItemCost(itemData, (UIShopInventory)parentInventory);
            }


            float textPaddingSize = 4f;

            Vector2 backgroundSize = new Vector2(650 + textPaddingSize * 2f, itemName.preferredHeight + itemDescription.preferredHeight + statDisplayContainer.GetComponent<RectTransform>().sizeDelta.y + textPaddingSize * 2f);
            backgroundRectTransform.sizeDelta = backgroundSize;

        }

        //private void DisplayItemCost(UIItemData itemData, UIShopInventory parentInventoryShop)
        //{
        //    if (parentInventoryShop.inventoryName == UIShopInventory.InventoryName.Player)
        //    {
        //        itemCost.text = "Sell for: " + parentInventoryShop.inventory.SellItemCost(itemData.uiItemInInventory).ToString();
        //    }
        //    else
        //    {
        //        itemCost.text = "Buy for: " + parentInventoryShop.inventory.BuyItemCost(itemData.uiItemInInventory).ToString();
        //    }
        //}

        private void DisplayItemStats(ItemConfig item, GameObject itemStatsCell, GameObject displayContainer)
        {
            

            //create custom display per item type
            switch (item.CheckItemType()){
                case ItemType.characterWeapon:
                    //subclasses must be cast in order to call some functions
                    WeaponConfig weaponItem = (WeaponConfig)item;
                    GenerateItemStatDisplay(itemStatsCell, displayContainer, "Damage", (weaponItem.GetDamage() + "" + weaponItem.GetDamageBonus()).ToString());
                    GenerateItemStatDisplay(itemStatsCell, displayContainer, "Range", weaponItem.GetDamage().ToString());
                    break;
            }



            foreach (Modifier statToDisplay in item.genericModifier.GetAdditiveModifiers())
            {
                GenerateItemStatDisplay(itemStatsCell, displayContainer, statToDisplay.statType.ToString(), statToDisplay.statValue.ToString());
    
            }
        }

        private void GenerateItemStatDisplay(GameObject itemStatsCell, GameObject displayContainer, string statName, string statValue)
        {
            RectTransform displayItemStatsRectTransform = Instantiate(itemStatsCell.transform, displayContainer.transform).GetComponent<RectTransform>();

            Text itemStatToDisplay = displayItemStatsRectTransform.GetComponent<Text>();

            displayItemStatsRectTransform.gameObject.SetActive(true);

            displayItemStatsRectTransform.anchoredPosition = new Vector2(itemStatToDisplay.preferredWidth, itemStatToDisplay.preferredHeight);

            itemStatToDisplay.text = statName + " " + statValue;
        }

        public void CompareEquippedItemInSlot(UIItemData itemData, UIInventory parentInventory)
        {
            //if item is equipped, it must already be in the character equipment slot and therefore does not need to be compared to anything. Guards recursive comparison.
            if (itemData.uiItemInInventory.isEquipped == true) return;        

            ItemConfig item = itemData.uiItemInInventory.itemObject;

            foreach (Transform child in parentInventory.GetEquipmentSlots().transform)
            {
                //check to see if child exists, if not then move on
                if (child.gameObject.GetComponentInChildren<UIItemData>() == null) continue;

                ItemConfig equippedItem = child.gameObject.GetComponentInChildren<UIItemData>().uiItemInInventory.itemObject;

                if (equippedItem.CheckItemType() == item.CheckItemType())
                {
                    DisplayItemStats(equippedItem, currentDisplayItemStatsCell, currentStatDisplayContainer);
                }
                else
                {
                    return;
                }

            }
        }

        public void RefreshItemDisplayStat(GameObject statDisplayContainer, GameObject displayItemStatsCell)
        {
            foreach (Transform child in statDisplayContainer.transform)
            {
                if (child == displayItemStatsCell.transform)
                {
                    child.gameObject.SetActive(false);
                    continue;
                }
                Destroy(child.gameObject);
            }
        }

            private void HideToolTip()
        {
            RefreshItemDisplayStat(statDisplayContainer, displayItemStatsCell);
            RefreshItemDisplayStat(currentStatDisplayContainer, currentDisplayItemStatsCell);
            gameObject.SetActive(false);
            
        }


        public static void ShowToolTip_Static( UIItemData itemData, UIInventory parentInventory) {
            instance.ShowToolTip(itemData, parentInventory);

        }

        public static void HideToolTip_Static()
        {
            instance.HideToolTip();

        }

    }

}