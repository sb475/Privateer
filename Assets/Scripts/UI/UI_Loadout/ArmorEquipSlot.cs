using RPG.Base;
using RPG.Control;
using RPG.Items;
using RPG.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ArmorEquipSlot : ItemSlot
{
    Image armorImage;
    [SerializeField] GameObject armorPrefab;
    UIItemData prefabItemData;
    public CrewPanel crewPanel;
    public ArmorConfig currentArmor;
    public CrewMember crewRef;

    private void Awake()
    {

        armorImage = GetComponent<Image>();
        prefabItemData = armorPrefab.GetComponent<UIItemData>();
        
    }
    public void UpdateArmor()
    {
        if (currentArmor != null)
        {
            Color c = armorImage.color;
            c.a = 0;
            armorImage.color = c;
        }
        crewRef = crewPanel.crewSlot.crewOnSlot;
        sourceInventory = crewRef.inventory;

        ItemConfig testArmor;
        Debug.Log("ArmorEquipSlot " + crewPanel.crewSlot.crewOnSlot);
        if (currentArmor == null)
        {
            crewPanel.crewSlot.crewOnSlot.equipment.equipped.TryGetValue(EquipmentSlots.armor, out testArmor);
            currentArmor = testArmor as ArmorConfig;
            armorPrefab.SetActive(true);
            prefabItemData.uiItem = new Item { itemObject = currentArmor, isEquipped = true, itemQuantity = 1 };
            uiItemInSlot = prefabItemData;
        }
        else
        {
            if (currentArmor != null) crewPanel.crewSlot.crewOnSlot.equipment.Equip(EquipmentSlots.armor, currentArmor);
        }

        crewPanel.UpdateSlotDisplay();
    }
    public override bool ItemMoveCondition()
    {
        if (!crewPanel.panelActive) return false;

        return true;
    }
    public override void HandleDropItem(ItemSlotGeneric slotToAddTo, Item uiItemInInventory)
    {
        Debug.Log("HandleDropCalled");
        Debug.Log(uiItemInInventory.itemObject.name);
        currentArmor = uiItemInInventory.itemObject as ArmorConfig;
        UpdateArmor();

    }

    public override void OnPostDrag()
    {
        uiItemInSlot = null;
        currentArmor = null;
        if (currentArmor == null)
        {
            Color c = armorImage.color;
            c.a = 1;
            armorImage.color = c;
        }
        armorPrefab.SetActive(false);
    }



}
