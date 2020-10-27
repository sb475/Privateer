using RPG.Base;
using RPG.Items;
using RPG.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorEquipSlot : ItemSlot
{
    Image armorImage;
    public CrewSlot crewPaneldrop;
    public ArmorConfig currentArmor;
    public CrewSlotDisplay [] slotContainers;

    private void Awake()
    {
        armorImage = GetComponent<Image>();
    }
    public void UpdateArmor(ArmorConfig armor)
    {
        if (currentArmor != null)
        {
            Color c = armorImage.color;
            c.a = 0;
            armorImage.color = c;
        }
        ItemConfig testArmor;
        if (crewPaneldrop.crewOnSlot.equipment.equipped.TryGetValue(EquipmentSlots.armor, out testArmor))
        {
            currentArmor = testArmor as ArmorConfig;
        }
        else
        {
            currentArmor = armor;
        }
        
        foreach (CrewSlotDisplay slotDisplay in slotContainers)
        {
            slotDisplay.UpdateCrewSlotDisplay(this);
        }

    }
    public override bool ItemMoveCondition()
    {
        if (crewPaneldrop.crewOnSlot == null) return false;

        return true;
    }
    public override void HandleDropItem(ItemSlotGeneric slotToAddTo, Item uiItemInInventory)
    {
        uIInventory.inventory.RemoveItem(uiItemInInventory);
        uIInventory.UpdateInventory();
        currentArmor = uiItemInInventory.itemObject as ArmorConfig;
        crewPaneldrop.crewOnSlot.equipment.Equip(EquipmentSlots.armor, currentArmor);
        UpdateArmor(currentArmor);

    }

   

}
