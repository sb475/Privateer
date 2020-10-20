using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
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
    public class Stat {

        public StatType statType;
        public float baseValue;

    }    
        
}