using System.Collections.Generic;
using UnityEngine.UI;

namespace RPG.Stats
{
    [System.Serializable]
    public class Abilities
    {
        public List<Ability> abilities;
        
    }
    [System.Serializable]
    public class Ability{
        public int abilitiyLevel;
        //public Animation
        public string abilityDescription;
        public string abilityName;
        public Image abilityIcon;

    }
}