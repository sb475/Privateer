using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using RPG.Attributes;
using RPG.Movement;
using RPG.Control;
using System.Collections.Generic;
using GameDevTV.Utils;
using System;
using RPG.Items;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IAttack
    {

        [SerializeField] float timeBetweenAttacks = 1f;

        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Transform weaponBackTransform = null;
        // [SerializeField] Transform pistolHolsterTransform = null;
        [SerializeField] Transform knifeHolsterTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
        [SerializeField] ArmorConfig defaultArmor = null;
        LazyValue<float> totalActionPoints;
        [SerializeField] private float actionPoints;
        bool outOfRangeAnnounced = false;
        
        Health target;
        StateManager turnManager;
        
        // mathf.infitity allows to attack right away
        float timeSinceLastAttack = Mathf.Infinity;
        public WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;

        //Armor variables
        [SerializeField] ArmorConfig currentArmorConfig;
        LazyValue<Armor> currentArmor;

        private void Awake() {
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
            currentArmor = new LazyValue<Armor>(SetupDefaultArmor);
            totalActionPoints = new LazyValue<float>(GetInitalActionPoints);
            turnManager = GetComponent<StateManager>();
        }

        private float GetInitalActionPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.ActionPoints);
        }

        private void Start() 
        {
            currentWeapon.ForceInit();
            totalActionPoints.ForceInit();
            actionPoints = totalActionPoints.value;
        }

       
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            // guard conditions
            if (target == null) return;
            if (target.IsDead()) return;
            if (gameObject.GetComponent<Health>().IsDead()) return;
        
            if(turnManager.GetCanMove())
            {
                //leave in place for moving to cover
                MoveToTarget(target.gameObject);
            }
            else
            {
                GetComponent<IEngine>().Cancel();
                if (turnManager.GetCanAct() == true)
                {
                    AttackBehavior();
                }
                
            }
        }

        public void MoveToTarget(GameObject target)
        {
            GetComponent<IEngine>().MoveToLocation(target.transform.position);
        }

        private Weapon SetupDefaultWeapon()
        {
            
            return AttachWeapon(defaultWeapon);
        }

        private Armor SetupDefaultArmor()
        {

            return AttachArmor(defaultArmor);
        }
        
        public void EquipWeapon(WeaponConfig weapon)
        {
            if (weapon == null) weapon = defaultWeapon;

            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
            
        }

        public void EquipArmor(ArmorConfig armor)
        {
            if (armor == null) armor = defaultArmor;

            currentArmorConfig = armor;
            currentArmor.value = AttachArmor(armor);
        }

        private Armor AttachArmor(ArmorConfig armor)
        {
            return armor.Spawn(rightHandTransform, leftHandTransform);
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget()
        {
            return target;
        }

        private void AttackBehavior()
        {
                //reset flag so that next time in range resets.
                outOfRangeAnnounced = false;

                            transform.LookAt(target.transform);
                if (timeSinceLastAttack > timeBetweenAttacks)
                {
                    //This will trigger the Hit() event.
                    TriggerAttack();
                    timeSinceLastAttack = 0;
                }
               

        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        // Animation Event
        void Hit()
        {

            if (target == null) return; // guard statement
                  
    //this rolls based on the Damage of the weapon + Base damage of the weapon. Weapon damage = 6f, it's a D6 roll.
            float damage = GetComponent<BaseStats>().CalculateDamage(Stat.Damage, currentWeaponConfig);
            
            if (currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }
            print (gameObject + "'s damage is: " + damage);
            if (currentWeaponConfig.HasProjectile()) // check to see if it's prjectile weapon
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }
            //resets target to avoid auto attacking same target next round.
            UpdateActionPoints(2);

        }

        void Shoot() //ties animatin into Hit event
        {
            Hit();
        }


        private bool GetIsInRange(GameObject combatTarget)
        {
            return Vector3.Distance(transform.position, combatTarget.transform.position) < currentWeaponConfig.GetRange();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            if (actionPoints <= 0) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            combatTarget.GetComponent<Health>();

            if (actionPoints <= 0)
            {
                turnManager.SetCanAct(false);
            } 
            if (!GetIsInRange(combatTarget) && turnManager.GetCanMove() == false)
            {
                    print(combatTarget.gameObject.name + " is out of range");
                if ((GetComponent<AIController>() != null))
                {
                    turnManager.SetCanAct(false);
                    }   
                    return;
            }
            //assigns target for dealing damage
            GetComponent<ActionScheduler>().StartAction(this);
        }
          

            //  print ("take that you weasel");
        
        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<IEngine>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }


//save functions
        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }

        //Action Points
        public void RestoreActionPoints()
        {
            actionPoints = totalActionPoints.value;
            turnManager.SetCanAct(true);
        }
        public float GetActionPoints()
        {
            return actionPoints;
        }

        private float UpdateActionPoints(float costActionPoints)
        {
            actionPoints -= costActionPoints;
            if (actionPoints <= currentWeaponConfig.GetActionPointCost())
            {
                turnManager.SetCanAct(false);
            }
            else
            {
                turnManager.SetCanAct(true);
            }
            return actionPoints;

        }
        public void ClearTarget()
        {
            target = null;
        }

        public string DamageAsString()
        {
            return (GetComponent<BaseStats>().GetDamage(Stat.Damage, currentWeaponConfig) + " " + currentWeaponConfig.GetDamageBonus()).ToString();
        }

    }
        


}
