using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace RPG.UI 
{
    public class ItemBehavior : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {

        public static ItemBehavior Instance { get; private set; }

        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform parentRectTransform;

        private Vector3 lastPosition;
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
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

        public void OnPointerClick(PointerEventData eventData)
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
                // if (itemData.GetItemData().itemObject.itemType == ItemType.consumable)
                // {
                //     itemData.GetItemData().itemObject.UseItem(uIInventory.GetInventoryOwner());
                //     uIInventory.UI_ItemRemove(itemData.uiItemInInventory);
                   
                // }
            }

            ItemToolTip.HideToolTip_Static();
                
        }

        public UIItemData GetItemData()
        {
            return itemData;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {

            lastPosition = gameObject.transform.position;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = .6f;
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;


            //sets item back to where it came from if not dropped on an ItemSlot
            transform.localPosition = Vector3.zero;

        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public RectTransform GetParentAnchor()
        {
            return parentRectTransform;
        }

        public void OnDrop(PointerEventData eventData)
        {
          GetComponentInParent<ItemSlot>().AddItemToSlot(eventData.pointerDrag, GetComponentInParent<ItemSlot>());
        }

        public Vector3 GetLastItemPosition()
        {
            return lastPosition;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {

            ItemToolTip.ShowToolTip_Static(GetComponent<UIItemData>(), uIInventory);

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ItemToolTip.HideToolTip_Static();
        }
    }
}
