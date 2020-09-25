using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Base;
using UnityEngine;

namespace RPG.Items{

    [System.Serializable]
    public class ItemInInventory
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
    public class Inventory : MonoBehaviour
    {
        [SerializeField] public List<ItemInInventory> itemList;
        [SerializeField] int currency;
        public event EventHandler OnInventoryChanged;

        public Inventory(List<ItemInInventory> inventoryData) 
        {
            if (inventoryData == null)
            {
                itemList = new List<ItemInInventory>();
            }
            else {
                itemList = inventoryData;
            }
        }

        public void AddItem(ItemInInventory itemToAdd)
        {
            //checks to see if item should be stacked

            if (itemToAdd.itemObject.CheckStackable())
            {
                bool alreadyInInventory = false;
                foreach (ItemInInventory item in itemList)
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
                    AddItem(new ItemInInventory { itemObject = itemToAdd.itemObject, itemQuantity = 1 });
                }
            }
            else {

                itemList.Add(itemToAdd);
            }
            
            OnInventoryChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RemoveItem (ItemInInventory itemToRemove)
        {
            if (itemToRemove.itemObject.CheckStackable())
            {
                ItemInInventory itemInInventory = null;
                foreach (ItemInInventory item in itemList)
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

        public List<ItemInInventory> GetItemList(){
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

        public void SellItem(ItemInInventory item, Inventory sellToInventoy)
        {
            this.RemoveCurreny(SellItemCost(item));
            this.AddItem(item);

            sellToInventoy.AddCurrency(SellItemCost(item));
            sellToInventoy.RemoveItem(item);
        }

        public void BuyItem(ItemInInventory item, Inventory moveToInventory)
        {
            moveToInventory.RemoveCurreny(BuyItemCost(item));
            moveToInventory.AddItem(item);

            this.AddCurrency(BuyItemCost(item));
            this.RemoveItem(item);
        }

        public int BuyItemCost(ItemInInventory item)
        {
            Debug.Log(item.itemObject.baseCost);
            //add modifier here
            return item.itemObject.baseCost * 3;
        }
        public int SellItemCost(ItemInInventory item)
        {
            Debug.Log(item.itemObject.baseCost);
            //add modifier here
            return item.itemObject.baseCost * 1;
        }
    
    }



}