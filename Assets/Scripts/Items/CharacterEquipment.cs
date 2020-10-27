using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;
using RPG.Base;
using BrainFailProductions.PolyFew;

namespace RPG.Items
{
    public enum EquipmentSlots
    {
        weapon,
        tool,
        armor,
        special,

    }

    [System.Serializable]
    public class CharacterEquipment
    {
        Character character;
        DefaultEquip defaultEquip;
        public Dictionary<EquipmentSlots, ItemConfig> equipped;
        public Item[] weaponsAvailable;
        public ArmorConfig currentArmor;
        public Item[] utilityAvailable;
        public Item[] specialAvailable;

        private List<Modifier> modifiersToAdd = new List<Modifier>();

        public CharacterEquipment(Character character)
        {
            this.character = character;
            equipped = new Dictionary<EquipmentSlots, ItemConfig>();
            if (character.defaultEquip != null)
                this.defaultEquip = character.defaultEquip;
                //Debug.Log("Default armor is: " + defaultEquip.defaultArmor);

            if (defaultEquip.defaultArmor != null)
            {
                Equip(EquipmentSlots.armor, defaultEquip.defaultArmor);
                currentArmor = equipped[EquipmentSlots.armor] as ArmorConfig;
                weaponsAvailable = new Item[currentArmor.weaponSlots];
                utilityAvailable = new Item[currentArmor.utlitySlots];
                specialAvailable = new Item[currentArmor.specialSlots];
            }
        }

        public void InitializeEquipment() 
        {
            equipped = new Dictionary<EquipmentSlots, ItemConfig>();

            //Debug.Log(defaultEquip.defaultPrimary.GetName());
            Equip(EquipmentSlots.weapon, defaultEquip.defaultPrimary);
            Equip(EquipmentSlots.armor, defaultEquip.defaultArmor);
            Equip(EquipmentSlots.tool, defaultEquip.defaultTool);
            Equip(EquipmentSlots.special, defaultEquip.defaultSpecial);
                    
        }

        public bool SwitchToRangedWeapon(bool ranged)
        {
            WeaponConfig weaponToTest = new WeaponConfig();

            foreach (Item weapon in weaponsAvailable)
            {
                WeaponConfig weaponAvail = weapon.itemObject as WeaponConfig;
                //add evaluation to determine which is best.
                if (weaponAvail.HasProjectile() == ranged) weaponToTest = weaponAvail;
            }

            if (weaponToTest != null)
            {
                Equip(EquipmentSlots.weapon, weaponToTest);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Equip (EquipmentSlots equipmentSlot, ItemConfig itemToEquip)
        {
            if (itemToEquip == null) return;

            if (equipmentSlot == EquipmentSlots.weapon)
            {
                TryEquip(equipmentSlot, itemToEquip as WeaponConfig);
            }
            else if (equipmentSlot == EquipmentSlots.armor)
            {
                currentArmor = itemToEquip as ArmorConfig;
                TryEquip(equipmentSlot, currentArmor);
            }
            else if (equipmentSlot == EquipmentSlots.armor)
            {
                TryEquip(equipmentSlot, itemToEquip as SpecialConfig);
            }
            else if (equipmentSlot == EquipmentSlots.armor)
            {
                TryEquip(equipmentSlot, itemToEquip as UtilityConfig);
            }
        }

        private void TryEquip(EquipmentSlots equipmentSlot, ItemConfig itemToEquip)
        {
            ItemConfig itemInSlot;

            if (equipped.TryGetValue(equipmentSlot, out itemInSlot))
            {
                SwapEquip(equipmentSlot, itemToEquip);
            }
            else
            {
                equipped.Add(equipmentSlot, itemToEquip);
                AddModifiersFromEquipment(equipped[equipmentSlot]);
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



    }

}