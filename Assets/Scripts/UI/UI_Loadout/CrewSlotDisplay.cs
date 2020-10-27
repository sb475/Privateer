using RPG.Base;
using RPG.Items;
using RPG.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrewSlotDisplay : MonoBehaviour
{
    ArmorEquipSlot armorRef;
    public GameObject itemSlotContainer;
    public ItemSlot itemSlot;
    Inventory cargoHold;
    [SerializeField] ItemType itemType;

    public void UpdateCrewSlotDisplay(ArmorEquipSlot armorRef)
    {
        this.armorRef = armorRef;
        cargoHold = armorRef.crewPanel.uIController.ship.cargoHold;
        Item[] currentlyAvailableItems;
        int slotAmount = 0;

        if (itemType == ItemType.weapon)
        {
            slotAmount = armorRef.currentArmor.weaponSlots;
            currentlyAvailableItems = armorRef.crewRef.equipment.weaponsAvailable;
        }
        else if (itemType == ItemType.special)
        {
            slotAmount = armorRef.currentArmor.specialSlots;
            currentlyAvailableItems = armorRef.crewRef.equipment.specialAvailable;
        }
        else if (itemType == ItemType.utility)
        {
            slotAmount = armorRef.currentArmor.utlitySlots;
            currentlyAvailableItems = armorRef.crewRef.equipment.utilityAvailable;
        }
        else
        {
            slotAmount = armorRef.currentArmor.cargoSlots;
            currentlyAvailableItems = armorRef.crewRef.inventory.itemList.ToArray();
        }

        //update slots on display
        BuildInvSlots(slotAmount);

        //populate current items in character list
        int leftOver = PopulateSlots(armorRef.crewRef.equipment.weaponsAvailable);
        //if any are left over, move items to ships cargo haul and then resize the array, items outside the array size will be lost unless moved to cargo first.
        if (leftOver > 0)
        {
            for (int i = currentlyAvailableItems.Length; i == currentlyAvailableItems.Length - leftOver; i--)
            {
                cargoHold.AddItem(currentlyAvailableItems[i]);
            }
            Array.Resize(ref currentlyAvailableItems, slotAmount);
        }
        
    }

    int PopulateSlots(Item [] itemList)
    {
        int itemCount = itemList.Length;
        int invIndex = 0;
        foreach (Transform child in itemSlotContainer.transform)
        {
            Debug.Log(itemSlotContainer.transform.childCount);
            Debug.Log("Inventory Index is: " + invIndex);
            if (invIndex < itemCount && itemCount != 0)
            {
                Item item = itemList[invIndex];
                GameObject uiItem = child.Find("UI_Item").gameObject;
                //Sets icon of Item Config
                Image uiItemImage = uiItem.GetComponent<Image>();
                uiItemImage.sprite = item.itemObject.itemIcon;

                //Sets up relavent data information for referencing later
                UIItemData itemData = uiItem.GetComponent<UIItemData>();
                itemData.SetItemData(item);

                Text displayItemAmount = child.GetComponentInChildren<Text>();
                if (item.itemQuantity > 1)
                {
                    displayItemAmount.text = item.itemQuantity.ToString();
                }
                else
                {
                    displayItemAmount.text = "";
                }
                if (child.gameObject.activeSelf == false) child.gameObject.SetActive(true);
            }
        }

        return invIndex - itemCount;
    }


    public virtual void BuildInvSlots(int slotCount)
    {
        Debug.Log("Inventory count is: " + slotCount);
        int childCount = itemSlotContainer.transform.childCount;
        Debug.Log("Current child count is: " + childCount);

        if (itemSlotContainer.transform.childCount < slotCount)
        {
            for (int i = slotCount - childCount; i < slotCount; i++)
            {
                CreateInvSlot();
            }
            Debug.Log("Added " + (slotCount - childCount) + " slots");
        }
        else if (itemSlotContainer.transform.childCount > slotCount)
        {
            for (int i = childCount; childCount == slotCount; i--)
            {
                DestroyChild(transform.GetChild(i));
            }
        }
    }

    private void DestroyChild(Transform child)
    {
        if (child == itemSlot.transform)
        {
            child.gameObject.SetActive(false);
        }
        else
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateInvSlot()
    {
        RectTransform itemSlotRectTransform = Instantiate(itemSlot.transform, itemSlotContainer.transform).GetComponent<RectTransform>();

        itemSlotRectTransform.gameObject.SetActive(true);
    }
}
