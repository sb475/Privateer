using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Items;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using RPG.Control;

namespace RPG.UI{

    public class UIPlayerInventory : UIInventory
    {
        public TextMeshProUGUI displayName;

        [SerializeField] private GameObject characterEquipSlots;

        public override void Awake()
        { 
            base.Awake();
            
        }

        public void Start() {

            //SetPlayerInventory(inventoryOwner);
        }
        public override void RefreshCurrencyDisplay()
        {
            displayCurrency.text = inventory.GetCurrency().ToString();
        }


        private void OnEnable() {
            uIController.OnCrewToDisplayChange += UpdateInvetoryDisplay;
            SetPlayerInventory(uIController.GetCrewToDisplay().gameObject);
        }
        private void OnDisable() {
            uIController.OnCrewToDisplayChange -= UpdateInvetoryDisplay;
        }

        private void UpdateInvetoryDisplay(object sender, EventArgs e)
        {
            SetPlayerInventory(uIController.GetCrewToDisplay().gameObject);
        }

        public bool SetPlayerInventory(GameObject inventoryOwnerObj)
        {
            if (inventoryOwnerObj == null) return false;
            inventoryOwner = inventoryOwnerObj;
            inventory = inventoryOwner.GetComponent<Inventory>();
            SetInventory(inventory);
            displayName.text = uIController.GetCrewToDisplay().GetCrewName();
            return true;
        }

        public void UI_ItemDrop (Item itemToDrop){
            UI_ItemRemove(itemToDrop);
            Instantiate(itemToDrop.itemObject.inWolrdItemPrefab, inventoryOwner.transform.position, Quaternion.identity);
            RefreshInventoryItems();
        }

        public void Btn_DropItem (UIInventory uIInventory)
        {
           // uIInventory.UI_ItemDrop(selectedSlot);
        }

        public void Btn_UseItem (UIInventory uIInventory)
        {
            // if ( uIInventory.selectedSlot.itemObject.itemType == ItemType.consumable )
            // {
            //     uIInventory.selectedSlot.itemObject.UseItem(inventoryOwner);
            //     UI_ItemRemove(selectedSlot);
            // }
            // else
            // {
            //     Debug.Log ("This item does not have a use function");
            // }
        }

        public override GameObject GetEquipmentSlots()
        {
            return characterEquipSlots;
        }
    }
}
