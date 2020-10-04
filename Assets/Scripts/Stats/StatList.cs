using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Stat List", menuName = "Stats/Make New Stat List", order = 0)]
    public class StatList: ScriptableObject
    {
        const string statListName = "Stats";
        public List<Stat> statList;
    }
}