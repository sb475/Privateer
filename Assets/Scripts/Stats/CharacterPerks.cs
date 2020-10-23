using System;
using System.Collections.Generic;
using RPG.Control;
using RPG.Global;
using UnityEngine;

namespace RPG.Stats
{
    public class CharacterPerks
    {
        [SerializeField] private List<Perk> characterPerks;
        Character character;

        public CharacterPerks(Character character)
        {
            this.character = character;
        }

        public void RegisterPerk(Perk perk)
        {
            //maybe unpdate to dictionary.
            characterPerks.Add(perk);

            //add additive modifiers
            foreach (Modifier stat in perk.GetPerkAddMod())
            {
                if (!character.characterModAdd.ContainsKey(stat.statType))
                {
                    character.characterModAdd.Add(stat.statType, stat.statValue);
                }
                else
                {
                    character.characterModAdd[stat.statType] = character.characterModAdd[stat.statType] += stat.statValue;
                }
            }

            //add percentage modifiers
            foreach (Modifier stat in perk.GetPerkPercentMod())
            {
                if (!character.characterModPercent.ContainsKey(stat.statType))
                {
                    character.characterModPercent.Add(stat.statType, stat.statValue);
                }
                else
                {
                    character.characterModPercent[stat.statType] = character.characterModPercent[stat.statType] += stat.statValue;
                }
            }
        }
       public void AddPerkLevel (Perk perkName, int increase = 1)
        {
            int perkIndex = 0;
            foreach (Perk perk in characterPerks)
            {
                if (perkName == perk)
                {
                    break;
                }
                perkIndex++;
            }

            characterPerks[perkIndex].perkLevel += increase;
        }

    }
}