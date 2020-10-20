using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    //need to find a better way to organize these    
    [CreateAssetMenu(fileName = "Stat", menuName = "Character/Create Stat", order = 0)]
    public class StatType : ScriptableObject
    {
        public StatName statName;
        public string statDescription;
    }
}