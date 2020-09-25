using UnityEngine;
using RPG.Stats;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Base;

namespace RPG.Items
{

    [CreateAssetMenu(fileName = "ConsumableItem", menuName = "Item/Make Consumable", order = 0)]
    [System.Serializable]
    public class ConsumableConfig : ItemConfig
    {
        const string itemName = "ConsumableItem";

        private void Awake()
        {
            itemType = ItemType.consumable;
            isEquippable = false;
            isStackable = true;
        }

        public override void UseItem(GameObject inventoryOwner)
        {
            Debug.Log("Used " + this.name);
            foreach (Modifier mod in genericModifier.GetAdditiveModifiers())
            {
                inventoryOwner.GetComponent<Health>().Heal((int)mod.statValue);

            }
        }

    }
}
