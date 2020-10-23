using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public enum CharacterPointTypes
    {
        major,
        minor,
        perk,
        action
    }
    //used to keep name's uniform. Can be replaced with string.
    public enum StatName
    {
        Health,
        ExperienceReward,
        ExperienceToLevelUp,
        Damage,
        Armor,
        ActionPoints,
        Consitution,
        Intelligence,
        Personality,
        Strength,
        Speed,
        Dexterity,
        Awareness,
        Memory,
        Resolve,
        Charm,
        Insight,
        Luck 
    }

    [System.Serializable]
    public class Modifier
    {
        public StatName statType;
        public float statValue;
    }

    [System.Serializable]
    public class GenericModifier
    {

        public List<Modifier> additiveModifiers;
        private List<string> additiveModStatsAsList;
        public List<Modifier> percentageModifiers;
        private List<string> percentageModStatsAsList;


        public List<Modifier> GetAdditiveModifiers()
        {
            return additiveModifiers;
        }
        public List<string> GetAdditiveModifiersList()
        {
            additiveModStatsAsList = new List<string>();

            foreach (Modifier modifier in additiveModifiers)
            {
                if (modifier == null) continue;

                additiveModStatsAsList.Add(modifier.statValue.ToString() + " " + modifier.statValue.ToString());
            }
            return null;
        }
        public List<Modifier> GetPercentageModifiers()
        {
            return percentageModifiers;
        }

        public List<string> GetPercentageModifiersList()
        {
            percentageModStatsAsList = new List<string>();

            foreach (Modifier modifier in percentageModifiers)
            {
                if (modifier == null) continue;

                percentageModStatsAsList.Add(modifier.statValue.ToString() + " " + modifier.statValue.ToString());
            }
            return null;
        }
    }

    [System.Serializable]
    public class Stat {

        public StatType statType;
        public float baseValue;

    }    
        
}