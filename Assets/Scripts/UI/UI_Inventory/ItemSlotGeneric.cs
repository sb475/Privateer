using System;
using RPG.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class ItemSlotGeneric : DropContainer
        {
            public SlotType slotType = SlotType.inventoryContainer;

            public UIInventory uIInventory;

            public event EventHandler<OnItemDroppedEventArgs> OnItemDropped;
            public class OnItemDroppedEventArgs : EventArgs
            {
                public ItemInInventory itemInInventory;
            }
           

            private void Awake()
            {
                uIInventory = GetComponentInParent<UIInventory>();
            }

            public override void OnDrop(PointerEventData eventData)
            {
                GameObject droppedObject = eventData.pointerDrag;

            }


            public UIInventory GetInventoryController()
            {
                return uIInventory;
            }

            public void UpdateParent(GameObject droppedObject, GameObject parentToUpdate)
            {
                droppedObject.transform.SetParent(parentToUpdate.transform);
                droppedObject.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            }

            protected virtual void ActivateOnItemDropped (ItemSlot itemSlot, ItemInInventory uiItemInInventory)
            {

                OnItemDropped?.Invoke(this, new OnItemDroppedEventArgs { itemInInventory = uiItemInInventory });

            }               

        }
}