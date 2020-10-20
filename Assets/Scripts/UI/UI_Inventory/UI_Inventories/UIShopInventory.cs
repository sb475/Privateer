using System;
using RPG.Items;
using UnityEngine;

namespace RPG.UI
{

    public class UIShopInventory : UIInventory
    {

        public enum InventoryName
        {
            Player, 
            Merchant
        }

        public ShopController shopController;
        public InventoryName inventoryName;


        public override void Awake()
        {
            base.Awake();
        }

        private void OnEnable() {

            Debug.Log(uIController.GetCrewToDisplay());
            if (inventoryName == InventoryName.Player)
            {
                inventory = uIController.GetCrewToDisplay().GetComponent<Inventory>();
            }
            else
            {
                inventory = inventoryOwner.GetComponent<Inventory>();
            }

            uIController.OnCrewToDisplayChange += OnCrewDisplayChange;
        }
        private void OnDisable() {
            uIController.OnCrewToDisplayChange -= OnCrewDisplayChange;
        }
        public override void RefreshCurrencyDisplay()
        {
            displayCurrency.text = inventory.GetCurrency().ToString();
        }


        private void OnCrewDisplayChange(object sender, EventArgs e)
        {
            if (inventoryName == InventoryName.Player)
            {
                SetInventory(uIController.GetCrewToDisplay().GetComponent<Inventory>());
            }
        }

        public override void LeftClick(ItemBehavior itemBehavior)
        {
            if (this.inventoryName == InventoryName.Player)
            {
                SellItemUI(itemBehavior.GetItemData().uiItemInInventory); 
            }
            else 
            {
                BuyItemUI(itemBehavior.GetItemData().uiItemInInventory);
            }
        }
        
        public void BuyItemUI (ItemInInventory item)
        {
            shopController.BrokerBuy(item);
        }

        public void SellItemUI(ItemInInventory item)
        {
            shopController.BrokerSell(item);
        }

    }

}