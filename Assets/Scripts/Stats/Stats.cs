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

    [System.Serializable]
    public class Attribute {
        
        public int baseValue;
        public int modifiedValue;
        public string atttributeName;
        public string attribteDescription;

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

    [System.Serializable]
    public class CharacterAttributes
    {
       public Attribute strength = new Attribute("Stength", 10);
       private void Awake() {
           
       }
    }

}