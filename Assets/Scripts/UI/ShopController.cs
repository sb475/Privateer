using RPG.Items;
using UnityEngine;
using RPG.Control;
using RPG.Global;

namespace RPG.UI
{
    public class ShopController : MonoBehaviour
    {
        public UIShopInventory player;
        public UIShopInventory merchant;
        [SerializeField] private GameObject gameBuffer;
        
        public void BrokerBuy(ItemInInventory item)
        {
            if (merchant.inventory.BuyItemCost(item) > player.inventory.GetCurrency())
            {
                GameEvents.instance.SendEventMessage("You do not have enough money");
                return;
            }
            else
            {
                merchant.inventory.BuyItem(item, player.inventory);
            }

            
        }

        private void OnEnable() {
            gameBuffer.SetActive(false);
        }

        private void OnDisable() {
            gameBuffer.SetActive(true);
        }

        public void BrokerSell(ItemInInventory item)
        {

            Debug.Log (merchant.inventory.SellItemCost(item));

            if (player.inventory.SellItemCost(item) > merchant.inventory.GetCurrency())
            {
                GameEvents.instance.SendEventMessage("Merchant does not have enough money");
                return;
            }
            else{

                merchant.inventory.SellItem(item, player.inventory);

            }
}
    }
}