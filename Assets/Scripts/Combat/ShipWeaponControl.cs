using System.Collections.Generic;
using UnityEngine;
using static RPG.Combat.ShipWeaponSystem;

namespace RPG.Combat
{
    public class ShipWeaponControl : MonoBehaviour, IAttack
    {
        public GameObject shipWeapons;
        public List<ShipWeaponSystem> allWeapons;


        private void Start() {
            GetWeaponSystems();
        }

        public void FireAtWill()
        {

        }
        public void HoldFire()
        {

        }
        public void FireTypeOnly(HardPointType pointType)
        {

        }

        private void GetWeaponSystems()
        {
            foreach (Transform child in shipWeapons.transform)
            {
                if (child.GetComponent<ShipWeaponSystem>() == null) continue;
                allWeapons.Add(child.GetComponent<ShipWeaponSystem>());
            }
        }
        
        public void Attack(GameObject target)
        {
            foreach (ShipWeaponSystem weaponSystem in allWeapons)
            {
                weaponSystem.FireAtWill(target);
            }
        }

        public bool CanAttack(GameObject target)
        {
            return true;
        }

        public void Cancel()
        {
            foreach (ShipWeaponSystem weaponSystem in allWeapons)
            {
                weaponSystem.CeaseFire();
            }
        }

        public string DamageAsString()
        {
            throw new System.NotImplementedException();
        }

        public void RestoreActionPoints()
        {
            throw new System.NotImplementedException();
        }
    }
}