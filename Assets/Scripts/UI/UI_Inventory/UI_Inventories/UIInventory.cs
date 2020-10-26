using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Items;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using RPG.Core;
using RPG.Base;

namespace RPG.UI{
    public class UIInventory : MonoBehaviour
    {
        public UIController uIController;
        Item uiItemData;
        public GameObject selectedSlot;

        public GameObject itemSlotContainer;
        public GameObject itemSlot;
        public int inventorySize;
        public GameObject inventorySource;
        public Inventory inventory;
        public bool setInventorySize;
        public ItemFilter itemFilter;

        public virtual void Awake() {
            
            uIController = GetComponentInParent<UIController>();
        }

        public virtual void Start ()
        {
            InitializeInventory();
        }

        private void InitializeInventory()
        {
            inventory = inventorySource.GetComponent<IInventory>().inventory;
            if (itemFilter != ItemFilter.none)
            {
                List<Item> filteredItems = new List<Item>();
                foreach (Item item in inventory.GetItemList())
                {
                    if (item.itemObject.itemFilter == itemFilter) filteredItems.Add(item);
                    
                }
                UpdateInventory(filteredItems);
            }
            else
            {
                UpdateInventory(inventory.GetItemList());
            }
        }

        public virtual void BuildInvSlots(int inventoryCount)
        {
            Debug.Log("Inventory count is: " + inventoryCount);
            int childCount = itemSlotContainer.transform.childCount;
            Debug.Log("Current child count is: " + childCount);
            if (setInventorySize && itemSlotContainer.transform.childCount < inventorySize)
            {
                for(int i = inventorySize - childCount; i < inventorySize; i++)
                {
                    CreateInvSlot();
                }
                Debug.Log("Added " + (childCount - inventorySize) + " slots");
            }
            else if (itemSlotContainer.transform.childCount < inventoryCount)
            {
                for (int i = inventoryCount - childCount; i < inventoryCount; i++)
                {
                    CreateInvSlot();
                }
                Debug.Log("Added " + (inventoryCount - childCount) + " slots");
            }
        }

        private void CreateInvSlot()
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlot.transform, itemSlotContainer.transform).GetComponent<RectTransform>();

            itemSlotRectTransform.gameObject.SetActive(true);            
        }
        private void UpdateInventory(List<Item> itemList)
        {
            int itemCount;
            if (itemList == null) itemCount = 0;
            
            itemCount = itemList.Count;
            Debug.Log("Item list count: " + itemCount);

            BuildInvSlots(itemCount);

            int invIndex = 0;
            foreach (Transform child in itemSlotContainer.transform)
            {
                Debug.Log(itemSlotContainer.transform.childCount);
                Debug.Log("Inventory Index is: " + invIndex);
                if (invIndex < itemCount && itemCount != 0)
                {
                    Item item = itemList[invIndex];
                    GameObject uiItem = child.Find("UI_Item").gameObject;
                    //Sets icon of Item Config
                    Image uiItemImage = uiItem.GetComponent<Image>();
                    uiItemImage.sprite = item.itemObject.itemIcon;

                    //Sets up relavent data information for referencing later
                    UIItemData itemData = uiItem.GetComponent<UIItemData>();
                    itemData.SetItemData(item);

                    Text displayItemAmount = child.GetComponentInChildren<Text>();
                    if (item.itemQuantity > 1)
                    {
                        displayItemAmount.text = item.itemQuantity.ToString();
                    }
                    else
                    {
                        displayItemAmount.text = "";
                    }
                    if (child.gameObject.activeSelf == false) child.gameObject.SetActive(true);
                }
                else if (setInventorySize)
                {
                    //Set to empty slots
                }
                else
                {
                    RemoveExcessdChildren(child);

                }

                invIndex++;
                
            }           
        }

        private void RemoveExcessdChildren(Transform child)
        {
            if (child == itemSlot.transform)
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                Destroy(child.gameObject);
            }
        }

        public virtual void SetInventory(Inventory inventory)
        {

            inventory.OnInventoryChanged -= Inventory_OnInvetoryChanged;
            inventory.OnInventoryChanged += Inventory_OnInvetoryChanged;
           
        }

        private void Inventory_OnInvetoryChanged(object sender, EventArgs e)
        {
            
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

        //public void UI_ItemRemove(Item itemToRemove)
        //{
        //    inventory.RemoveItem(itemToRemove);
        //    RefreshInventoryItems();
        //}

        //public void UI_ItemAdd(Item itemToAdd)
        //{
        //    inventory.AddItem(itemToAdd);
        //    RefreshInventoryItems();
        //}


        public void SelectItemInInventory(GameObject itemToSelect)
        {
           selectedSlot = itemToSelect;
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

    }
}
