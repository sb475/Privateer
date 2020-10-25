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

    public class UIShipCargo : UIInventory
    {
        public TextMeshProUGUI displayName;

        public override void Awake()
        { 
            base.Awake();
            
        }

        public void Start() {

            //SetPlayerInventory(inventoryOwner);
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
            //SetInventory(inventory);
            displayName.text = uIController.GetCrewToDisplay().GetCrewName();
            return true;
        }


        public void Btn_DropItem (UIInventory uIInventory)
        {
           // uIInventory.UI_ItemDrop(selectedSlot);
        }

    }
}
