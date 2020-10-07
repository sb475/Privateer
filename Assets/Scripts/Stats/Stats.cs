using System.Collections.Generic;

namespace RPG.Stats
{
//need to find a better way to organize these 
    public enum StatType
    {
        Health,
        ExperienceReward,
        ExperienceToLevelUp,
        Damage,
        Armor,
        ActionPoints,
        Consitution,
        Intillect,
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
    public class Stat {

        public StatType statType;
        public float baseValue;
        public string statDescription;

    }

    [System.Serializable]
    public class Stat_Test
    {
        public StatType statType;
        public float baseValue;
        public string statDescription;

        /* void statType(StatType statType, float value, string statDescription)
        {

        } */
    }

    
        
}