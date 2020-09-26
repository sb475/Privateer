using System.Collections.Generic;

namespace RPG.Stats
{
//need to find a better way to organize these 
    public enum Stat
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

    public class Attribute {
        
        int baseValue;
        string atttributeName;
        string attribteDescription;

        public Attribute (string atttributeName, int baseValue)
        {
            this.atttributeName = atttributeName;
            this.baseValue = baseValue;

        }

        public void OneUpAttribute()
        {
            baseValue ++;
        }
        public void OneDownAttribute()
        {
            baseValue --;
        }
        
    }
    public class CharacterAttributes
    {
        int Level;
        List<Attribute> characterAttributes;
    }

}