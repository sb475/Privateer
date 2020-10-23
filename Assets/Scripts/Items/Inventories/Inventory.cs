using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Base;
using UnityEngine;

namespace RPG.Items{

    [System.Serializable]
    public class Item
    {
        public ItemConfig itemObject;
        public int itemQuantity;
        public bool isEquipped = false;

        public void EquipItem(bool state)
        {
            if (itemObject.isEquippable == true)
            {
                isEquipped = state;
            }
        }

    }

    [System.Serializable]
    public class Inventory
    {

        public List<Item> itemList;
        [SerializeField] int currency;
        public event EventHandler OnInventoryChanged;
        [Header("Import items")]
        public InventoryData itemData;

        public Inventory()
        {
            if (itemData != null)
            {
                itemList = itemData.inventoryData;
            }
            else
            {
                itemList = new List<Item>();
            }
        }
        public Inventory(List<Item> inventoryData) 
        {
                itemList = inventoryData;
        }

        public void AddItem(Item itemToAdd)
        {
            //checks to see if item should be stacked

            if (itemToAdd.itemObject.CheckStackable())
            {
                bool alreadyInInventory = false;
                foreach (Item item in itemList)
                {
                    if (item.itemObject == itemToAdd.itemObject)
                    {
                        item.itemQuantity += itemToAdd.itemQuantity;
                        alreadyInInventory = true;
                    }
                }
                if (!alreadyInInventory)
                {
                    itemList.Add(itemToAdd);
                }
            }
            else if (!itemToAdd.itemObject.CheckStackable() && itemToAdd.itemQuantity > 1)
            {
                Debug.Log ("Item is not stackable but is stacked");
                for (int i = 0; i < itemToAdd.itemQuantity; i++)
                {
                    AddItem(new Item { itemObject = itemToAdd.itemObject, itemQuantity = 1 });
                }
            }
            else {

                itemList.Add(itemToAdd);
            }
            
            OnInventoryChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RemoveItem (Item itemToRemove)
        {
            if (itemToRemove.itemObject.CheckStackable())
            {
                Item itemInInventory = null;
                foreach (Item item in itemList)
                {
                    
                    if (item.itemObject == itemToRemove.itemObject)
                    {
                        //set prompt to remove all items or only certain number
                        item.itemQuantity -= 1;
                        itemInInventory = item;
                    }
                }
                if (itemInInventory != null && itemInInventory.itemQuantity <= 0)
                {
                    itemList.Remove(itemToRemove);
                }
            }
            else
            {

                itemList.Remove(itemToRemove);
            }

            OnInventoryChanged?.Invoke(this, EventArgs.Empty);
        }

        public void AddItemFromConfig(ItemConfig item, int quantity)
        {
            AddItem(new Item { itemObject = item, itemQuantity = quantity });
        }

        public Item RemoveItemFromInventory(ItemConfig item, int quantity)
        {
            Item itemToReturn = new Item();
            
            for (int i = 0; i < itemList.Count; i++)
            {
                if(itemList[i].itemObject == item)
                {
                    if (itemList[i].itemQuantity <= quantity)
                    {
                        Debug.Log("You do not have " + item.name);
                        return null;
                    }
                    
                    itemList[i].itemQuantity -= quantity;
                    if (itemList[i].itemQuantity == 0)
                    {
                        itemList.Remove(itemList[i]);
                    }

                    itemToReturn = new Item { itemObject = item, itemQuantity = quantity };
                }
            }

            return itemToReturn;
        }

        public List<Item> GetItemList(){
            return itemList;
        }

        public int GetCurrency()
        {
            return currency;
        }

        public void AddCurrency (int amount)
        {
            currency += amount;
        }

        public void RemoveCurreny (int amount)
        {
            currency -= amount;
            if (currency < 0) return;
        }

        public void SellItem(Item item, Inventory sellToInventoy)
        {
            this.RemoveCurreny(SellItemCost(item));
            this.AddItem(item);

            sellToInventoy.AddCurrency(SellItemCost(item));
            sellToInventoy.RemoveItem(item);
        }

        public void BuyItem(Item item, Inventory moveToInventory)
        {
            moveToInventory.RemoveCurreny(BuyItemCost(item));
            moveToInventory.AddItem(item);

            this.AddCurrency(BuyItemCost(item));
            this.RemoveItem(item);
        }

        public int BuyItemCost(Item item)
        {
            Debug.Log(item.itemObject.baseCost);
            //add modifier here
            return item.itemObject.baseCost * 3;
        }
        public int SellItemCost(Item item)
        {
            Debug.Log(item.itemObject.baseCost);
            //add modifier here
            return item.itemObject.baseCost * 1;
        }
    
    }



}