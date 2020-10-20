using System.Collections.Generic;
using RPG.Attributes;
using RPG.Base;
using RPG.Control;
using UnityEngine;
using static RPG.Combat.ShipWeaponSystem;

namespace RPG.Combat
{
    public class ShipWeaponControl : MonoBehaviour, IAttack
    {
        public GameObject shipWeapons;
        public List<ShipWeaponSystem> allWeapons;
        public List<ShipWeaponSystem> missileWeapons;
        public List<ShipWeaponSystem> turretWeapons;
        public List<ShipWeaponSystem> defenseWeapons;
        float weaponRange;
        public bool fireAtWill;



        private void Start() {
            GetWeaponSystems();
        }

        public void FireAtWill(IDamagable target)
        {
            foreach (ShipWeaponSystem weaponSystem in allWeapons)
            {
                weaponSystem.SetTarget(target);
            }

        }
        public void HoldFire()
        {
            foreach (ShipWeaponSystem weaponSystem in allWeapons)
            {
                weaponSystem.CeaseFire();
            }

        }

        private void OnTriggerEnter(Collider other) {
            if (other.GetComponent<Missile>() != null)
            {  
                Debug.Log(other.gameObject.name);
                DefensiveFire(other.gameObject);
                
            }
        }

        public void DefensiveFire(GameObject missile)
        {
            foreach (ShipWeaponSystem weapon in defenseWeapons)
            {
                if (weapon.target == null)
                {
                    weapon.Attack(missile.gameObject);
                }
            }
        }


        public void FireTypeOnly(HardPointType pointType)
        {
            
        }

        private void GetWeaponSystems()
        {
            foreach (Transform child in shipWeapons.transform)
            {
                if (child.GetComponent<ShipWeaponSystem>() == null) continue;

                    ShipWeaponSystem weaponSystem = child.GetComponent<ShipWeaponSystem>();


                    allWeapons.Add(weaponSystem);

                    if (weaponSystem.hardPointType == HardPointType.Missile)
                    {
                        missileWeapons.Add(weaponSystem);
                    }
                    else if (weaponSystem.hardPointType == HardPointType.Turret)
                    {
                        turretWeapons.Add(weaponSystem);
                    }
                    else if (weaponSystem.hardPointType == HardPointType.Defense)
                    {
                        defenseWeapons.Add(weaponSystem);
                    }
            }

            weaponRange = allWeapons[0].currentWeaponConfig.GetRange();

            foreach (ShipWeaponSystem weapon in allWeapons)
            {
                if (weapon.currentWeaponConfig.GetRange() < weaponRange)
                {
                    weaponRange = weapon.currentWeaponConfig.GetRange();
                }
            }

        }

        public float OptimalWeaponRange()
        {
            return weaponRange;
        }
        
        public void Attack(IDamagable target)
        {
            FireAtWill(target);
        }

        public bool CanAttack(IDamagable target)
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

        public float GetWeaponDamage()
        {
            throw new System.NotImplementedException();
        }

        public float GetWeaponRange()
        {
            throw new System.NotImplementedException();
        }
    }
}