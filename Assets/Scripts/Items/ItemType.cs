using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Items
{
    public enum ItemType 
    {
        characterArmor,
        characterWeapon,
        utlity,
        special,
        cargo,
    }

    public enum ItemFilter
    {
        none,
        armor,
        weapon,
        utility,
        special,
        cargo,
    }

    public enum WeaponType
    {
        kinetic,
        elctronic,
        special
    }
    public enum ArmorType
    {
        light,
        medium,
        heavy
    }
}
