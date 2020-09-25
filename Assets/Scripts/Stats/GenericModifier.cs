using System.Collections.Generic;

namespace RPG.Stats
{
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
}