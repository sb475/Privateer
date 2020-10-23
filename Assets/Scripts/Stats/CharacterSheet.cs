using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Stats
{
    public enum StatNameT
    {
        Health,
        ExperienceReward,
        ExperienceToLevelUp,
        Damage,
        Armor,
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
    public class StatToAdd
    {
        public StatNameT s;
        public float v;
    }

    [CreateAssetMenu(fileName = "Stat List", menuName = "Stats/Make New ChracterSheet", order = 0)]
    public class CharacterSheet: ScriptableObject
    {
        public float defaultHealthValue;
        public float defaultStatValue;
        public List<StatToAdd> statsToAddToDict;
        public Dictionary<StatNameT, float> statValues;
        public Dictionary<CharacterPointTypes, int> pointValues;

        private void Awake()
        {
            pointValues = new Dictionary<CharacterPointTypes, int>();
            statValues = new Dictionary<StatNameT, float>();
            InitializePoints();
            InitializeStats();
        }

        public void InitializePoints()
        {
            pointValues.Add(CharacterPointTypes.action, 2);
            pointValues.Add(CharacterPointTypes.major, 0);
            pointValues.Add(CharacterPointTypes.minor, 0);
            pointValues.Add(CharacterPointTypes.perk, 0);
        }

        public void InitializeStats()
        {
            statValues.Add(StatNameT.Health, defaultHealthValue);
            statValues.Add(StatNameT.Consitution, defaultStatValue);
            statValues.Add(StatNameT.Intelligence, defaultStatValue);
            statValues.Add(StatNameT.Personality, defaultStatValue);
            statValues.Add(StatNameT.Strength, defaultStatValue);
            statValues.Add(StatNameT.Speed, defaultStatValue);
            statValues.Add(StatNameT.Dexterity, defaultStatValue);
            statValues.Add(StatNameT.Awareness, defaultStatValue);
            statValues.Add(StatNameT.Memory, defaultStatValue);
            statValues.Add(StatNameT.Resolve, defaultStatValue);
            statValues.Add(StatNameT.Charm, defaultStatValue);
            statValues.Add(StatNameT.Insight, defaultStatValue);
            statValues.Add(StatNameT.Luck, defaultStatValue);
            
            if (statsToAddToDict != null)
            {
                foreach(StatToAdd b in statsToAddToDict)
                {
                    statValues.Add(b.s, b.v);
                }
            }
        }
        
        public void PrintOutDictionary()
        {
            
        }
    }
}