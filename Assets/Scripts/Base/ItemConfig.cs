using UnityEngine;
using RPG.Stats;
using System.Collections.Generic;
using RPG.Items;

namespace RPG.Base
{

    [System.Serializable]
    public class ItemConfig : ScriptableObject
    {
        [Header("General Item Configs")]
        public ItemType itemType;
        public float mass;
        public bool isStackable;
        public bool isEquippable;
        public int baseCost;
        public ItemInWorld inWolrdItemPrefab;
        [SerializeField] public Sprite itemIcon;
        [TextArea(15,20)]
        [SerializeField] string itemDescription;

        //this provides arrays for percentage and additive modifiers.
        public GenericModifier genericModifier;

        const string itemName = "Item";

        public virtual void UseItem(GameObject inventoryOwner)
        {
            Debug.Log("Used " + this.name);
        }
        public bool CheckStackable()
        {
            return isStackable;
        }
        public ItemType CheckItemType()
        {
            return itemType;
        }
        
        public string GetDescription()
        {
            return itemDescription;
        }
        public string GetName()
        {
            return this.name;
        }
    }
}
