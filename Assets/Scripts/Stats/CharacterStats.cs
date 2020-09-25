using System;
using System.Collections.Generic;
using RPG.Control;
using RPG.Global;
using UnityEngine;

namespace RPG.Stats
{
    public class CharacterStats : MonoBehaviour, IModifierProvider
    {
        [SerializeField] private List<Perk> characterPerks;
        [SerializeField] private List<Modifier> attributeModifiers;

        [SerializeField] int perkPoints;
        [SerializeField] int majorAttributePoints;
        [SerializeField] int minorAttributePoints;

        public GenericModifier[] genericModifierItems;


        public void AddPerk (Perk perkToAdd, int newPerkLevel)
        {
           
                foreach (Perk perks in characterPerks)
                {
                    //if the level of perk that is trying to be added is a different level then return this level.
                    if (perkToAdd == perks)
                    {
                        perks.perkLevel = newPerkLevel;
                        Debug.Log (perks.name + " is now at level " + perks.perkLevel + ". And the description says: " + perks.GetPerkData().perkDescription);
                        return;
                    }
                }
                characterPerks.Add(perkToAdd);
        }

        internal void AddAttribute(Stat statToDisplay)
        {
            foreach (Modifier mod in attributeModifiers)
            {
                //if the level of perk that is trying to be added is a different level then return this level.
                if (statToDisplay == mod.statType)
                {
                    mod.statValue ++;
                    return;
                }
            }
            attributeModifiers.Add(new Modifier {statType = statToDisplay, statValue = 1});
        }

        public bool SpendFromMajor(Stat stat)
        {
            if (majorAttributePoints > 0)
            {
                AddAttribute(stat);
                majorAttributePoints -= 1;
                GameEvents.instance.OnMajorAttribPoolChange(majorAttributePoints);
                return true;
            }
            else
            {
                GameEvents.instance.SendMessage("You do not have enough points to spend");
                return false;
            }
           
        }
        public bool SpendFromMinor(Stat stat)
        {
            if (minorAttributePoints > 0)
            {
                AddAttribute(stat);
                minorAttributePoints -= 1;
                GameEvents.instance.OnMajorAttribPoolChange(minorAttributePoints);
                return true;
            }
            else
            {
                GameEvents.instance.SendMessage("You do not have enough points to spend");
                return false;
            }
        }
        public bool SpendFromPerk(Perk perkToAdd, int newPerkLevel)
        {
            if (perkPoints > 0)
            {
                AddPerk(perkToAdd, newPerkLevel);
                perkPoints -= 1;
                return true;
            }
            else 
            {
                GameEvents.instance.SendMessage("You do not have enough points to spend");
                return false;
            }

        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            float totalStatValue = 0;

            foreach (var perk in characterPerks)
            {
                if (perk == null) continue;

                foreach (var modifier in perk.GetPerkAddMod())
                {

                    if (modifier.statType == stat)
                    {
                        totalStatValue += modifier.statValue;
                    }
                }
            }
            foreach (Modifier am in attributeModifiers)
            {
                if (stat == am.statType) totalStatValue +=  am.statValue;
            }

            yield return totalStatValue;
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {

            foreach (var perk in characterPerks)
            {
                if (perk == null) continue;

                foreach (var modifier in perk.GetPerkPercentMod())
                {

                    if (modifier.statType == stat)
                    {
                        yield return modifier.statValue;
                    }
                }
            }
        }

    }
}