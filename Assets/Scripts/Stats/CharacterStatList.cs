using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Stat List", menuName = "Stats/Make New Character Stat List", order = 0)]
    public class CharacterStatList : ScriptableObject
    {
        const string statListName = "Stats";
        public List<StatType> generateStats = new List<StatType>();
        public List<Stat> statList = new List<Stat>();

        private void OnInspectorUpdate()
        {
            if (generateStats != null)
            {
                for (int i = 0; i < generateStats.Count; i++)
                {
                    int statValue = 1;
                    if (generateStats[i].statName == StatName.ActionPoints)
                    {
                        statValue = 2;
                    }
                    else if (generateStats[i].statName == StatName.Health || generateStats[i].statName == StatName.ExperienceReward)
                    {
                        statValue = 100;
                    }
                    statList.Add(new Stat{statType = generateStats[i], baseValue = statValue});
                    generateStats.Remove(generateStats[i]);
                }

            }
        }
   
    }
}