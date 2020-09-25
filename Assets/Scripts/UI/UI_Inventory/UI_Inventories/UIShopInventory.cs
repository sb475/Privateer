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
            if (inventoryName == InventoryName.Player)
            {
                inventory = uIController.GetCrewToDisplay().GetComponent<Inventory>();
            }
            else
            {
                inventory = inventoryOwner.GetComponent<Inventory>();
            }

            uIController.UpdateCrewDisplayedChanged += OnCrewDisplayChange;
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