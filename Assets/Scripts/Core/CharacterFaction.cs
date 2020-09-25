using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class CharacterFaction : MonoBehaviour
    {
        [SerializeField] List<AppplyCharacterFaction> factions;

        public FactionReaction CompareFaction(GameObject player)
        {
            List<AppplyCharacterFaction> playerFaction = player.GetComponent<CharacterFaction>().GetFactions();
            foreach (AppplyCharacterFaction x in playerFaction)
            {
                foreach (AppplyCharacterFaction faction in factions)
                {
                    if (faction == x)
                    {
                        int currentStanding = 0; //faction.GetFactionStanding();

                        if (currentStanding <= -50)
                        {
                            return FactionReaction.despised;
                        }
                        else if (currentStanding <= -20 && currentStanding > -50)
                        {
                            return FactionReaction.hostile;
                        }
                        else if (currentStanding <= 20 && currentStanding > -20)
                        {
                            return FactionReaction.neutral;
                        }
                        else if (currentStanding < 50 && currentStanding > 20)
                        {
                            return FactionReaction.friendly;
                        }
                        else if (currentStanding >= 50)
                        {
                            return FactionReaction.worshipped;
                        }

                    }
                }
            }
            // if nothing else then return neutral.
            return FactionReaction.neutral;

        }

        public List<AppplyCharacterFaction> GetFactions()
        {
            return factions;
        }
    }
}
