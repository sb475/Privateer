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

            uIController.OnCrewToDisplayChange += OnCrewDisplayChange;
        }
        private void OnDisable() {
            uIController.OnCrewToDisplayChange -= OnCrewDisplayChange;
        }

        private void OnCrewDisplayChange(object sender, EventArgs e)
        {
            if (inventoryName == InventoryName.Player)
            {
                //SetInventory(uIController.GetCrewToDisplay().GetComponent<Inventory>());
            }
        }

        public override void LeftClick(ItemBehavior itemBehavior)
        {
            if (this.inventoryName == InventoryName.Player)
            {
                SellItemUI(itemBehavior.GetItemData().uiItem); 
            }
            else 
            {
                BuyItemUI(itemBehavior.GetItemData().uiItem);
            }
        }
        
        public void BuyItemUI (Item item)
        {
            //shopController.BrokerBuy(item);
        }

        public void SellItemUI(Item item)
        {
            //shopController.BrokerSell(item);
        }

    }

}