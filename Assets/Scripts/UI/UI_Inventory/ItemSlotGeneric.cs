using System;
using RPG.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class ItemSlotGeneric : DropContainer
        {
            public SlotType slotType = SlotType.inventorySlot;

        public Inventory sourceInventory;
        public UIInventory parentUIInventory;

            public event EventHandler<OnItemDroppedEventArgs> OnItemDropped;
            public class OnItemDroppedEventArgs : EventArgs
            {
                public Item itemInInventory;
            }
           

            public override void Awake()
            {
                base.Awake();
       
                parentUIInventory = GetComponentInParent<UIInventory>();
                if (sourceInventory == null) sourceInventory = parentUIInventory.inventory;
            }

            public override void OnDrop(PointerEventData eventData)
            {
                GameObject droppedObject = eventData.pointerDrag;

            }


            public UIInventory GetInventoryController()
            {
                return parentUIInventory;
            }

            protected virtual void ActivateOnItemDropped (ItemSlot itemSlot, Item uiItemInInventory)
            {

                OnItemDropped?.Invoke(this, new OnItemDroppedEventArgs { itemInInventory = uiItemInInventory });

            }               

        }
}