using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Items;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace RPG.UI{

    public class UIInventory : MonoBehaviour
    {
        public UIController uIController;
        public Inventory inventory;
        ItemInInventory uiItemData;
        [SerializeField] private GameObject itemSlotContainer;
        [SerializeField] private GameObject itemSlot;
        [SerializeField] public GameObject inventoryOwner;
        [SerializeField] GameObject selectedSlot;
        [SerializeField] TextMeshProUGUI displayCurrency;
        [SerializeField] ItemInInventory selectedItemInInventory;

        public virtual void Awake() {
            
            inventory = inventoryOwner.GetComponent<Inventory>();
        }

        public virtual void Start() {
            SetInventory(inventory);
        }

        public virtual void SetInventory (Inventory inventory)
        {
            inventory.OnInventoryChanged -= Inventory_OnInvetoryChanged;
            this.inventory = inventory;
            inventory.OnInventoryChanged += Inventory_OnInvetoryChanged;
            RefreshInventoryItems();
        }

        private void Inventory_OnInvetoryChanged(object sender, EventArgs e)
        {
            RefreshInventoryItems();
        }

        private void RefreshCurrencyDisplay()
        {
            displayCurrency.text = inventory.GetCurrency().ToString();
        }

        public void RefreshInventoryItems ()
        {
            foreach (Transform child in itemSlotContainer.transform) {
                if (child == itemSlot.transform) 
                {
                    child.gameObject.SetActive(false);
                    continue;
                }
                Destroy(child.gameObject);    
            }

            int x = 0; 
            int y = 0;
            float itemSlotCellSize = 30f;

            //cycles through inventory items and generates image in ineventory UI.
            foreach (ItemInInventory item in inventory.GetItemList())
            {
                if (item.isEquipped == true) continue;
                
                RectTransform itemSlotRectTransform = Instantiate(itemSlot.transform, itemSlotContainer.transform).GetComponent<RectTransform>();
   
                itemSlotRectTransform.gameObject.SetActive(true);
                
                itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);

                GameObject uiItem = itemSlotRectTransform.Find("UI_Item").gameObject;

                //Sets icon of Item Config
                Image uiItemImage = uiItem.GetComponent<Image>();
                uiItemImage.sprite = item.itemObject.itemIcon;

                //Sets up relavent data information for referencing later
                UIItemData itemData = uiItem.GetComponent<UIItemData>();
                itemData.SetItemData(item);
                

                Text displayItemAmount = itemSlotRectTransform.Find("UI_ItemQuantityText").GetComponentInChildren<Text>();
                if (item.itemQuantity > 1)
                {
                    displayItemAmount.text = item.itemQuantity.ToString();
                }
                else
                {
                    displayItemAmount.text = "";
                }


                //think this is dead code
                x++;
                if (x > 4) {
                    x = 0;
                    y++;
                }
            }
            RefreshCurrencyDisplay();
        }

        public virtual void LeftClick(ItemBehavior itemBehavior)
        {
            Debug.Log ("Left Click");
        }
        public virtual void MiddleClick(ItemBehavior itemBehavior)
        {
            Debug.Log("Middle Click");
        }
        public virtual void RightClick(ItemBehavior itemBehavior)
        {
            Debug.Log("Right Click");
        }

        public void UI_ItemRemove(ItemInInventory itemToRemove)
        {
            inventory.RemoveItem(itemToRemove);
            RefreshInventoryItems();
        }

        public void UI_ItemAdd(ItemInInventory itemToAdd)
        {
            inventory.AddItem(itemToAdd);
            RefreshInventoryItems();
        }


        public void SelectItemInInventory(GameObject itemToSelect)
        {
           selectedSlot = itemToSelect;
           //selectedItemInInventory = selectedSlot.GetComponents<UIItemData>().GetItemData();
           //Debug.Log (selectedSlot + " item has been selected");
        }


        //this function for use with Player Inventory. Can be modified.
        public virtual GameObject GetEquipmentSlots()
        {
            return null;
        }

        public UIItemData GetUIItemFromSlot(ItemSlot itemSlot)
        {
            return itemSlot.GetComponentInChildren<UIItemData>();
        }

        public GameObject GetInventoryOwner()
        {
            return inventoryOwner;
        }

    }
}
