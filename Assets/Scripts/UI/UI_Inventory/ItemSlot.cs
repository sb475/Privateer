using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RPG.Items;
using RPG.Control;
using RPG.Global;

namespace RPG.UI
{
    public class ItemSlot : ItemSlotGeneric
    {

        public ItemType allowedItem;
        private UIItemData uiItemInSlot;     

        public override void Awake() {
            base.Awake();
            uiItemInSlot = GetComponentInChildren<UIItemData>();
        }

        public override void OnDrop(PointerEventData eventData)
        {
            GameObject droppedObject = eventData.pointerDrag;

            AddItemToSlot(droppedObject, this);

            
        }

        public void AddItemToSlot(GameObject droppedObject, ItemSlotGeneric slotToAddTo)
        {
            if (droppedObject != null)
            {
                //parent of item to later reference to set as ItemSlot variable if move is successful.
                UIItemData uIItemData = droppedObject.GetComponent<UIItemData>();
                //Need to add functionality for "Use, Drop, and Invalid Drop"
                Item uiItemInInventory = droppedObject.GetComponent<UIItemData>().GetItemData();

                //Store where item came from in memory
                ItemSlot parentSlot = droppedObject.GetComponentInParent<ItemSlot>();
                //Function ensures that object can be eqipped at current slot

                if (CheckToTransferSlot(uiItemInInventory, slotToAddTo))
                {
                    Debug.Log("transfer check good");

                    MoveItemOver(droppedObject, parentSlot, slotToAddTo);
                    //sends the message that item has been dropped and pushes to CharacterEquip script.

                    //Slot behavior
                    HandleDropItem(slotToAddTo, uiItemInInventory);

                    if (slotType != SlotType.inventoryContainer)
                    {
                        uiItemInSlot = uIItemData;
                    }

                }
                else
                {
                    GameEvents.instance.SendEventMessage("You cannot equip this here");
                    // eventData.pointerDrag.transform.position = eventData.pointerDrag.GetComponent<ItemDrag>().GetLastItemPosition();
                    return;
                }

                //uIInventory.RefreshInventoryItems();
            }
        }

        public virtual bool ItemMoveCondition()
        {
            return true;
        }

        public virtual void HandleDropItem(ItemSlotGeneric slotToAddTo, Item uiItemInInventory)
        {
            switch (slotToAddTo.slotType)
            {
                case SlotType.characterEquipSlot:
                    uiItemInInventory.isEquipped = true;
                    if (uIInventory.inventory.itemList.Contains(uiItemInInventory)) uIInventory.inventory.AddItem(uiItemInInventory);
                    GameEvents.instance.uiController.UpdateDisplayValues();
                    GameEvents.instance.OnItemChanged();
                    break;
                case SlotType.inventorySlot:
                    uiItemInInventory.isEquipped = false;
                    break;
                case SlotType.shopSlot:
                    break;
            }
        }

        private void MoveItemOver(GameObject droppedObject, ItemSlot parentSlot, ItemSlotGeneric slotToMoveTo )
        {
            Debug.Log(slotToMoveTo.slotType);

            if (slotToMoveTo.slotType == SlotType.inventoryContainer) UpdateParent(droppedObject, slotToMoveTo.gameObject);
            //pushed signal to listening CharacterEquip, add ship slot in future
            if (parentSlot.slotType == SlotType.characterEquipSlot)
            {
                GameEvents.instance.OnItemChanged();
            }

            //this needs to be changed to reference UI_Item type
            if (slotToMoveTo.GetComponentInChildren<UIItemData>() != null)
            {
                SwapItem(droppedObject, parentSlot);
            }
            else
            {
                UpdateParent(droppedObject, slotToMoveTo.gameObject);
            }

        }

        private void SwapItem(GameObject itemToSwap, ItemSlot parentSlotToSwapTo)
        {
            GameObject currentChild = GetComponentInChildren<UIItemData>().gameObject;
                    
                UpdateParent(itemToSwap, gameObject);

                if (parentSlotToSwapTo.GetComponentInChildren<UIItemData>() == null)
                {
                    //Debug.Log(parent + "has no more children");
                    AddItemToSlot(currentChild, parentSlotToSwapTo);
                }
                else {
                    //Debug.Log("Something went wrong");
                    return;
                }

            }

        private bool CheckToTransferSlot(Item uiItemInInventory, ItemSlotGeneric uiSlot)
        {
            if (!ItemMoveCondition()) return false;
            Debug.Log ("Check to Transfer: " + uiSlot.slotType);
            //if inventory slot, always allow
            if (uiSlot.slotType == SlotType.inventoryContainer || uiSlot.slotType == SlotType.shopSlot || uiSlot.slotType == SlotType.inventorySlot) 
            {
                return true;
            }

            

            ItemSlot uiItemSlot = (ItemSlot)uiSlot; 

            //if slot does not match, do not allow
            if (uiItemSlot.allowedItem == uiItemInInventory.itemObject.CheckItemType())
            {
               return true;
            }

            return false;
        }

        public UIItemData GetItemInSlot()
        {
            return uiItemInSlot;
        }

    }
}
