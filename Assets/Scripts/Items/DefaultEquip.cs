using RPG.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Items
{
    [CreateAssetMenu(fileName = "DefaultEquipment", menuName = "Character/Make New DefaultEquipment", order = 0)]
    public class DefaultEquip : ScriptableObject
{
        
        public WeaponConfig defaultPrimary;
        public WeaponConfig defaultSeconday;
        public ArmorConfig defaultArmor;
        public UtilityConfig defaultTool;
        public SpecialConfig defaultSpecial;
 
    }
}
