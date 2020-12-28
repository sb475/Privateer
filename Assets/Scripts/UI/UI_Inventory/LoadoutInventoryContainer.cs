using RPG.Global;
using RPG.Items;
using UnityEngine;
using UnityEngine.EventSystems;


namespace RPG.UI
{
    public class LoadoutInventoryContainer: ItemSlotGeneric
    {

        private void Awake()
        {
            parentUIInventory = GetComponentInParent<UIInventory>();
        }

        public override void OnDrop(PointerEventData eventData)
        {
            Debug.Log ("Drop called");
            GameObject droppedObject = eventData.pointerDrag;

            AddItemToSlot(droppedObject);

        }

        public void AddItemToSlot(GameObject droppedObject)
        {
            Debug.Log("Item was dropped " + droppedObject.name );
            if (droppedObject != null)
            {

                //Need to add functionality for "Use, Drop, and Invalid Drop"
                Item uiItemInInventory = droppedObject.GetComponent<UIItemData>().GetItemData();

                //Store where item came from in memory

                //ItemSlot parentSlot = droppedObject.GetComponentInParent<ItemSlot>();

                    if (uiItemInInventory.isEquipped)
                    {
                        uiItemInInventory.isEquipped = false;
                    }

                    UpdateParent(droppedObject, this.gameObject);
                    Destroy(droppedObject);

                    GameEvents.instance.OnItemChanged();

                //pushed signal to listening CharacterEquip, add ship slot in future

                }
                //uIInventory.RefreshInventoryItems();
            
        }

    }
}
