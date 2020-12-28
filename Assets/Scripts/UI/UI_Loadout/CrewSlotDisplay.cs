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
    CrewPanel crewPanel;
    public GameObject itemSlotContainer;
    public ItemSlot itemSlot;
    Inventory cargoHold;
    [SerializeField] ItemType itemType;

    private void Awake()
    {
        crewPanel = GetComponentInParent<CrewPanel>();
    }

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
            if (itemList[invIndex] != null)
            {
                //get the itemslot stored in child
                ItemSlot itemSlot = child.GetComponent<ItemSlot>();
                itemSlot.sourceInventory = crewPanel.crewSlot.crewOnSlot.inventory;
                Debug.Log(itemSlotContainer.transform.childCount);
                Debug.Log("Inventory Index is: " + invIndex);
                if (invIndex < itemCount && itemCount != 0)
                {
                    Debug.Log("UIItem in slow: " + itemSlot.uiItemInSlot);
                    Debug.Log("Item in itemlist: " + itemList[invIndex]);
                    //set item data from current crew inventory
                    itemSlot.uiItemInSlot.SetItemData(itemList[invIndex]);


                    //set the amount based on inventory
                    Text displayItemAmount = child.GetComponentInChildren<Text>();
                    if (itemSlot.uiItemInSlot.uiItem.itemQuantity > 1)
                    {
                        displayItemAmount.text = itemSlot.uiItemInSlot.uiItem.itemQuantity.ToString();
                    }
                    else
                    {
                        displayItemAmount.text = "";
                    }


                }
            }
        }

        return invIndex - itemCount;
    }


    public virtual void BuildInvSlots(int slotCount)
    {
        Debug.Log("Inventory count is: " + slotCount);
        int childCount = itemSlotContainer.transform.childCount;
        Debug.Log("Current child count is: " + childCount);

        if (childCount < slotCount)
        {
            for (int i = slotCount - childCount; i < slotCount; i++)
            {
                    CreateInvSlot();
            }
            Debug.Log("Added " + (slotCount - childCount) + " slots");
        }
        else if (childCount > slotCount)
        {
            Debug.Log("Removing " + (childCount - slotCount) + " slots");

            Debug.Log(childCount);
            for (int i = childCount; i > slotCount; i-=1)
            {
                Debug.Log(i);
                transform.GetChild(i-1).gameObject.SetActive(false);
            }
        }
        else if (childCount == slotCount)
        {
            Debug.Log("Slots equal, setting object active");
            for (int i = 0; i < slotCount; i++)
            {
                
                //if child is not active activate it
                if (transform.GetChild(i).gameObject.activeSelf == false) transform.GetChild(i).gameObject.SetActive(true);
            }
                
        }
    }

    private void DestroyChild(Transform child)
    {
        if (child == itemSlot.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void CreateInvSlot()
    {
        RectTransform itemSlotRectTransform = Instantiate(itemSlot.transform, itemSlotContainer.transform).GetComponent<RectTransform>();

        itemSlotRectTransform.gameObject.SetActive(true);
    }
}
