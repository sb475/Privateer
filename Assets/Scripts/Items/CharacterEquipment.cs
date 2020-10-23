﻿using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;
using RPG.Base;
using BrainFailProductions.PolyFew;

namespace RPG.Items
{
    public enum EquipmentSlots
    {
        primary,
        secondary,
        tool,
        armor,
        special
    }

    [System.Serializable]
    public class CharacterEquipment
    {
        Character character;
        DefaultEquip defaultEquip;
        public Dictionary<EquipmentSlots, ItemConfig> equipped;
        public List<WeaponConfig> weaponsAvailable;
        public List<ArmorConfig> armorAvailable;
        public List<ToolConfig> toolsAvailable;
        public List<SpecialConfig> specialAvailable;

        private List<Modifier> modifiersToAdd = new List<Modifier>();

        public CharacterEquipment(DefaultEquip defaultEquip, Character character)
        {
            this.character = character;
            if (defaultEquip != null)
                this.defaultEquip = defaultEquip;
        }

        public void InitializeEquipment() 
        {
            equipped = new Dictionary<EquipmentSlots, ItemConfig>();

            //Debug.Log(defaultEquip.defaultPrimary.GetName());
            Equip(EquipmentSlots.primary, defaultEquip.defaultPrimary);
            Equip(EquipmentSlots.secondary, defaultEquip.defaultSeconday);
            Equip(EquipmentSlots.armor, defaultEquip.defaultArmor);
            Equip(EquipmentSlots.tool, defaultEquip.defaultTool);
            Equip(EquipmentSlots.special, defaultEquip.defaultSpecial);
                    
        }

        public void Equip (EquipmentSlots equipmentSlot, ItemConfig itemToEquip)
        {
            if (itemToEquip == null) return;

            if (equipmentSlot == EquipmentSlots.primary)
            {
                TryEquip(equipmentSlot, itemToEquip as WeaponConfig);
            }
            else if (equipmentSlot == EquipmentSlots.secondary)
            {
                TryEquip(equipmentSlot, itemToEquip as WeaponConfig);
            }
            else if (equipmentSlot == EquipmentSlots.armor)
            {
                TryEquip(equipmentSlot, itemToEquip as ArmorConfig);
            }
            else if (equipmentSlot == EquipmentSlots.armor)
            {
                TryEquip(equipmentSlot, itemToEquip as SpecialConfig);
            }
            else if (equipmentSlot == EquipmentSlots.armor)
            {
                TryEquip(equipmentSlot, itemToEquip as ToolConfig);
            }
        }

        private void TryEquip(EquipmentSlots equipmentSlot, ItemConfig itemToEquip)
        {
            ItemConfig itemInSlot;
            if (equipped.TryGetValue(equipmentSlot, out itemInSlot))
            {
                SwapEquip(equipmentSlot, itemToEquip);
                Debug.Log("Replaced " + itemInSlot + " with " + itemToEquip);
            }
            else
            {
                equipped.Add(equipmentSlot, itemToEquip);
                AddModifiersFromEquipment(equipped[equipmentSlot]);
            }
        }

        private void AddModifiersFromEquipment(ItemConfig itemToReplaceWith)
        {
            //if item has no modifiers don't do anything
            if (itemToReplaceWith.genericModifier.additiveModifiers == null) return;

            float swapStatValue;
            foreach (Modifier mod in itemToReplaceWith.genericModifier.additiveModifiers)
            {
                if (mod == null) continue;
                if (character.characterModAdd.TryGetValue(mod.statType, out swapStatValue))
                {
                    character.characterModAdd[mod.statType] = character.characterModAdd[mod.statType] += mod.statValue;
                }
                else
                {
                    character.characterModAdd[mod.statType] = mod.statValue;
                }
            }
        }

        private void RemoveModifiersFromEquipment(ItemConfig itemToReplaceWith)
        {
            //if item has no modifiers don't do anything
            if (itemToReplaceWith.genericModifier.additiveModifiers == null) return;

            float swapStatValue;
            //remove current item modifier from dictionary
            foreach (Modifier mod in itemToReplaceWith.genericModifier.additiveModifiers)
            {
                if (character.characterModAdd.TryGetValue(mod.statType, out swapStatValue))
                {
                    character.characterModAdd[mod.statType] = character.characterModAdd[mod.statType] -= mod.statValue;
                }
                else
                {
                    Debug.Log("Something went wrong with SwapEquip " + itemToReplaceWith);
                }
            }
        }

        private void SwapEquip(EquipmentSlots equipmentSlot, ItemConfig itemToReplaceWith)
        {

            //remove current item modifier from dictionary
            RemoveModifiersFromEquipment(equipped[equipmentSlot]);
            AddModifiersFromEquipment(itemToReplaceWith);

            //add incoming item modifier
            equipped[equipmentSlot] = itemToReplaceWith;
        }





        //private void EventSelectedCharacterToDsiplay(object sender, CrewMember e)
        //{
           
        //    if (GetComponent<CrewMember>() == GameEvents.instance.uiController.GetCrewToDisplay())
        //    {
        //        Debug.Log("Crew chagned");
        //            GameEvents.instance.uiController.UpdateItemsFromEquipped(equippedItems);
        //    }
        //}


        //private void Start() 
        //{

        //    StartCoroutine(InitializeEquipmentInSlots());

        //    if (GetComponent<CrewMember>() == true)
        //    {
        //        GameEvents.instance.ItemChanged += EventCharacterEquip;
        //        GameEvents.instance.selectCrewChanged += EventSelectedCharacterToDsiplay;
        //        equippedItemsObject = GameEvents.instance.equippedItemSlots;
        //        }
        //}

        //public List<ItemConfig> GetEquippedItems ()
        //{
        //    return equippedItems;
        //}

        //private IEnumerator InitializeEquipmentInSlots()
        //{
        //    yield return new WaitUntil(() => (LoadWeaponInSlots()));
        //    EquipToIAttack();

        //}

        //private void EventCharacterEquip(object sender, System.EventArgs e)
        //{
        //    Debug.Log ("Equipment changed called");
        //    RefreshEquippedItems();
        //}

        //private void RefreshEquippedItems()
        //{
        //    equippedItems = new List<ItemConfig>();
        //    modifiersToAdd = new List<Modifier>();

        //    //This mapping is based off of the order of object under "UI_EquippedItems" in the Inventory UI. Udpdated in the future or double check if other issues occur.
        //    //0 = secondary
        //    //1 = armor
        //    //2 = primary
        //    //3 =accessory

        //    //if the item does not have an equippedItemsObject, meaning it's not a player with access to UI, move on to equip.
        //    if (equippedItemsObject != null)
        //    {
        //        foreach (Transform child in equippedItemsObject.transform)
        //        {
        //            //if the slot does not have a child or item in it, load it as null. The equip functions EquipWeapon and EquipArmor are designed
        //            // to deal with null values.
        //            if (child.GetComponentInChildren<UIItemData>() == null)
        //            {
        //                equippedItems.Add(null);
        //            }
        //            else
        //            {
        //                //add the item in slot to equippedItems array for stat calculation.
        //                equippedItems.Add(child.GetComponentInChildren<UIItemData>().GetInventoryItemConfig());
        //            }
        //        }

        //        secondaryWeapon = equippedItems[0];
        //        primaryWeapon = equippedItems[1];
        //        armor= equippedItems[2];
        //        accessory = equippedItems[3];


        //        //this should equal to 3.
        //        //Debug.Log(equippedItems.Count);

        //        // //Debug what is in the equippedItems array. If no item is equipped then it should simply be blank or null. 
        //        // foreach (ItemConfig item in equippedItems)
        //        // {
        //        //     Debug.Log("Item in equiped items: " + item);
        //        // }
        //    }
        //    //otherwise, equip what has been set into the slots for you. Items already exist on character // Primarily for starting out and NPS's.
        //    else
        //    {
        //        LoadWeaponInSlots();
        //    }

        //    EquipToIAttack();
        //}

        //private void EquipToIAttack()
        //{
        //    GetComponent<Fighter>().EquipWeapon(LoadWeapon(primaryWeapon));
        //    GetComponent<Fighter>().EquipArmor(LoadArmor(armor));
        //    //GetComponent<IAttack>().EquipArmor(LoadAccesory(equippedItems[index])); // this has not been implimented yet.
        //}

        //private bool LoadWeaponInSlots()
        //{
        //    equippedItems.Add(primaryWeapon);  // 1
        //    equippedItems.Add(armor); //2
        //    equippedItems.Add(accessory); //3
        //    return true;
        //}

        //Load commands can be sequenced better in the future

        public WeaponConfig LoadWeapon(ItemConfig item)
        {
            WeaponConfig newItem;

            if (item != null)
                {
                  newItem = Resources.Load<WeaponConfig>(item.name);
                return newItem;
                }
            return null;

        }

        public ArmorConfig LoadArmor(ItemConfig item)
        {

            if (item != null)
            {
                ArmorConfig newItem = Resources.Load<ArmorConfig>(item.name);

                return newItem;    
            }
        return null;
            
        }

        //public IEnumerable GetAdditiveModifiers(StatName stat)
        //{
        //   foreach (var item in equippedItems)
        //    {
        //        if (item == null) continue;
                
        //        if (item == equippedItems[0]) continue;
        //        if (item.genericModifier == null) continue;

        //        foreach (var modifier in item.genericModifier.GetAdditiveModifiers())
        //        {
        //            if (modifier == null) continue;

        //            if (modifier.statType == stat)
        //            {
        //                yield return modifier.statValue;
        //            }
        //        }
        //    }
        //}

        //public IEnumerable GetPercentageModifiers(StatName stat)
        //{
        //    foreach (var item in equippedItems)
        //    {   
        //        if (item == null) continue;
        //        if (item == equippedItems[0]) continue;
                
        //        foreach (var modifier in item.genericModifier.GetPercentageModifiers())
        //        {
        //            if (modifier == null) continue;
        //            if (modifier.statType == stat)
        //            {
        //                yield return modifier.statValue;
        //            }
        //        }
        //    }
        //}

    }

}