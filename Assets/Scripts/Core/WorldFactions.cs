using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core{

    [CreateAssetMenu(fileName = "WorldFactions", menuName = "World/Make Faction Container", order = 0)]
    public class WorldFactions : ScriptableObject
    {
        [SerializeField] public List<Faction> worldFactions;
    }

    [System.Serializable]
    public class Faction
    {
        public string worldFactionsNames;

        [TextArea(15, 20)]
        [SerializeField] string factionDescription;
    }

    [System.Serializable]
    public class AppplyCharacterFaction
    {
        public Faction characterFaction;
        public int characterFactionStanding;
    }

    public enum WorldFactionsNames
    {
        general,
        aiCollective
    }

    public enum FactionReaction {
        despised,
        hostile,
        neutral,
        friendly,
        worshipped
    }

    

    // public class ApplyFaction
    // {
    //     public WorldFactions

    //       public int GetFactionStanding()
    //     {
    //         return factionStanding;
    //     }

    //     public string GetFactionDescription()
    //     {
    //         return worldFactions.factionDescription;
    //     }
    // }


}


//     Dictionary<Faction, 

//     private void BuildLookup()
//     {
//         if (lookupTable != null) return;

//         lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

//         foreach (ProgressionCharacterClass progressionClass in characterClasses)
//         {
//             var statLookupTable = new Dictionary<Stat, float[]>();

// }
