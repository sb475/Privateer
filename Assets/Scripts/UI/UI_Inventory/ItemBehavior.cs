using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace RPG.UI 
{
    public class ItemBehavior : DroppableObject
    {
        public ItemSlot parentSlot;
        private UIInventory uIInventory;

        private UIItemData itemData;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            parentRectTransform = GetComponent<RectTransform>();
            parentSlot = GetComponentInParent<ItemSlot>();
            itemData = GetComponent<UIItemData>();
            uIInventory = parentSlot.GetInventoryController();

        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Debug.Log("Left click");
                uIInventory.LeftClick(this);
            }
                
            else if (eventData.button == PointerEventData.InputButton.Middle)
            {
                Debug.Log("Middle click");
                uIInventory.MiddleClick(this);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                uIInventory.RightClick(this);

            }

            ItemToolTip.HideToolTip_Static();
                
        }

        public UIItemData GetItemData()
        {
            return itemData;
        }

        
        public RectTransform GetParentAnchor()
        {
            return parentRectTransform;
        }

        public override void OnDrop(PointerEventData eventData)
        {
          GetComponentInParent<ItemSlot>().AddItemToSlot(eventData.pointerDrag, GetComponentInParent<ItemSlot>());
        }

        public Vector3 GetLastItemPosition()
        {
            return lastPosition;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {

            ItemToolTip.ShowToolTip_Static(GetComponent<UIItemData>(), uIInventory);

        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            ItemToolTip.HideToolTip_Static();
        }
    }
}
