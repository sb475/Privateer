
using System;
using RPG.Base;
using RPG.Control;
using RPG.Items;
using RPG.UI;
using UnityEngine;

namespace RPG.Events
{
    public class Shop : MonoBehaviour
    {

        public Inventory storeInventory;
        public UIShopInventory uIShopInventory;
        public InventoryData shopInventoryData;
        [SerializeField] UIController uIController;
        private bool inventoryPopulated;

        private void Awake() {
            uIController = (UIController)GameObject.FindObjectOfType(typeof(UIController));
            if (uIController == null) Debug.Log ("Something went wrong with finding controller in shop " + gameObject.name);
            inventoryPopulated = false;
        }

        public void OpenShopMenu()
        {
            uIController.ShowShopMenu();
            storeInventory = GetComponent<CharacterInteraction>().inventory;
            PopulateInventory();
            uIShopInventory.SetInventory(storeInventory);
        }

        private void PopulateInventory()
        {
            if (inventoryPopulated == false)
            {
                foreach (ItemInInventory item in shopInventoryData.inventoryData)
                {
                    storeInventory.AddItem(item);
                }

            }
            inventoryPopulated = true;
        }
    }
}