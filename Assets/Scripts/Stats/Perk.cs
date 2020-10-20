using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{

//Mirror progression dictionary build but for Perks
    [CreateAssetMenu(fileName = "Perk", menuName = "Character/Make Perk", order = 0)]
    public class Perk : ScriptableObject
    {
        public int perkLevel = 0;
        public PerkType perkType;
        public EffectType perkEffectType; 
        public Sprite perkIcon;
        const string perkName = "Perk";
        public List<PerkDetails> perkDetails;
        
       

        public PerkDetails GetPerkData ()
        {
            return perkDetails[perkLevel];
        }
        public List<Modifier> GetPerkPercentMod ()
        {
            return GetPerkData().genericModifier.GetPercentageModifiers();
        }
        public List<Modifier> GetPerkAddMod()
        {
            return GetPerkData().genericModifier.GetAdditiveModifiers();
        }
        public List<string> GetPerkPercentModList()
        {
            return GetPerkData().genericModifier.GetAdditiveModifiersList();
        }
        public List<string> GetPerkAddModList()
        {
            return GetPerkData().genericModifier.GetAdditiveModifiersList();
        }
    }

    [System.Serializable]
    public class PerkDetails
    {

        public GenericModifier genericModifier;
        //perks will rely primaryily percentage modifiers

        public List<Abilities> characterAbilities;
        
                [TextArea(5, 10)]
        public string perkDescription;

    }

    public enum PerkType
    {
        Stealth, Detection, SneakAttack, Evasion, Hacking, Medical, Engineering, Traps, Aggressor, Defender, HeavyRanged, HeavyMelee, Pilot, Synthetic, Juggernaut
    }
    public enum EffectType
    {
        passive, option
    }

}