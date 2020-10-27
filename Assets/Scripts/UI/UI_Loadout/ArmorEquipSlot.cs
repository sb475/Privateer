using RPG.Base;
using RPG.Control;
using RPG.Items;
using RPG.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorEquipSlot : ItemSlot
{
    Image armorImage;

    public CrewPanel crewPanel;
    public ArmorConfig currentArmor;
    public CrewSlotDisplay [] slotContainers;
    public CrewMember crewRef;

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
        if (crewPanel.crewSlot.crewOnSlot.equipment.equipped.TryGetValue(EquipmentSlots.armor, out testArmor))
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
        if (!crewPanel.panelActive) return false;

        return true;
    }
    public override void HandleDropItem(ItemSlotGeneric slotToAddTo, Item uiItemInInventory)
    {
        uIInventory.inventory.RemoveItem(uiItemInInventory);
        uIInventory.UpdateInventory();
        currentArmor = uiItemInInventory.itemObject as ArmorConfig;
        crewPanel.crewSlot.crewOnSlot.equipment.Equip(EquipmentSlots.armor, currentArmor);
        UpdateArmor(currentArmor);

    }

   

}
