using System;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Base;
using RPG.Control;
using UnityEngine;
using static RPG.Combat.ShipWeaponSystem;

namespace RPG.Combat
{
    public class ShipWeaponControl : MonoBehaviour, IFighter
    {
        public GameObject shipWeapons;
        public List<ShipWeaponSystem> allWeapons;
        public List<ShipWeaponSystem> missileWeapons;
        public List<ShipWeaponSystem> turretWeapons;
        public List<ShipWeaponSystem> defenseWeapons;
        public List<Missile> incomingMissiles;
        float weaponRange;
        public bool fireAtWill;



        private void Start() {
            GetWeaponSystems();
        }

        private void LateUpdate() {
            DefensiveFire();
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

        public float GetAverageWeaponRange (List<ShipWeaponSystem> weaponSystemList)
        {
            float averageRange = 0;
            foreach(ShipWeaponSystem weaponSystem in weaponSystemList)
            {
                averageRange += weaponSystem.GetWeaponRange();
            }
            return averageRange / weaponSystemList.Count;
        }

        public void IncomingProjectingDestroyed(Missile missile)
        {
            incomingMissiles.Remove(missile);
            foreach (ShipWeaponSystem weapon in defenseWeapons)
            {
                if (weapon.target == missile.GetComponent<IDamagable>())
                {
                //work in a dps calculation to determine targets. If dps of weapon will not defeat durability of missile before time of impact, add secondary target.
                    Debug.Log("Found null missile, removing");
                    weapon.CeaseFire();
                }
            }
            
        }

        public void DefensiveFire()
        {
            foreach (Missile missile in incomingMissiles)
            {
                if (missile == null)
                {
                    incomingMissiles.Remove(missile);
                    continue;
                }

                else if (Vector3.Distance(missile.transform.position, transform.position) <= GetAverageWeaponRange(defenseWeapons))
                {
                    Debug.Log("Missile is in range");
                    foreach (ShipWeaponSystem weapon in defenseWeapons)
                    {
                        //work in a dps calculation to determine targets. If dps of weapon will not defeat durability of missile before time of impact, add secondary target.
                        if (weapon.target == null && weapon.target != missile)
                        {
                            weapon.SetTarget(missile.GetComponent<IDamagable>());
                        }
                        else
                        {
                            continue;
                        }
                    }
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

        public void DetectIncomingMissiles(Missile missile)
        {
            Debug.Log("WARNING WARNING INCOMING MISSILES");
            incomingMissiles.Add(missile);
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