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
        public LazyValue<float> maxAP;
        public float currentAP;
        bool outOfRangeAnnounced = false;
        public bool attacking;

        public GameObject currentTarget;
        public IDamagable target;
        StateManager turnManager;
        CharacterEquipment equipment;
        
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
            maxAP = new LazyValue<float>(GetInitalActionPoints);
            turnManager = GetComponent<StateManager>();
            attacking = false;
            equipment = GetComponent<CharacterEquipment>();

        }

        private float GetInitalActionPoints()
        {
            return GetComponent<CharacterStats>().GetStat(StatType.ActionPoints);
        }

        private void Start() 
        {
            currentWeapon.ForceInit();
            maxAP.ForceInit();
            currentAP = maxAP.value;
        }

       
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            // guard conditions
            if (target == null) return;
            if (target.IsDead()) return;
            if (GetComponent<IDamagable>().IsDead()) return;
        }

        private void AttackBehavior(IDamagable target)
        {
            if (!GetIsInRange(target))
            {
                if (turnManager.GetCanMove())
                {
                    GetComponent<IEngine>().StartMoveAction(target.gameObject.transform.position, 1f);
                }
                else
                {
                    print(target.gameObject.name + " is out of range");

                    if ((GetComponent<AIController>() != null))
                    {
                        turnManager.SetCanAct(false);
                    }

                }
                //leave in place for moving to cover
            }
            else
            {
                GetComponent<IEngine>().Cancel();
                    //reset flag so that next time in range resets.
                outOfRangeAnnounced = false;

                transform.LookAt(target.gameObject.transform);

                //This will trigger the Hit() event.


                if (timeSinceLastAttack > timeBetweenAttacks)
                {
                    TriggerAttack();
                    timeSinceLastAttack = 0;
                }
            }
        }

#region WeaponConfigurations

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

        public void SwitchToSecondary()
        {
            EquipWeapon(equipment.LoadWeapon(equipment.GetEquippedItems()[0]));
        }
        public void SwitchToPrimary()
        {
            EquipWeapon(equipment.LoadWeapon(equipment.GetEquippedItems()[1]));
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

    #endregion


#region Animations
        public IDamagable GetTarget()
        {
            return target;
        }

        public void PresentRifle()
        {
            GetComponent<Animator>().SetTrigger("alertReady");
        }

        public void PutWeaponAway()
        {
            GetComponent<Animator>().SetTrigger("weaponAway");
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }
#endregion


#region AnimationEvent
        void Hit()
        {

            if (target == null) return; // guard statement
                  
    //this rolls based on the Damage of the weapon + Base damage of the weapon. Weapon damage = 6f, it's a D6 roll.
            float damage = GetComponent<CharacterStats>().CalculateDamage(StatType.Damage, currentWeaponConfig);
            
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
            target = null;

        }

        void Shoot() //ties animatin into Hit event
        {
            Hit();
        }

#endregion


        private bool GetIsInRange(IDamagable combatTarget)
        {
            return Vector3.Distance(transform.position, combatTarget.gameObject.transform.position) < currentWeaponConfig.GetRange();
        }

        public bool CanAttack(IDamagable combatTarget)
        {
            if (combatTarget == null) { return false; }
            if (currentAP <= 0) { return false; }
            IDamagable targetToTest = combatTarget;
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(IDamagable combatTarget)
        {
            if (!turnManager.GetCanAct()) 
            {
                Debug.Log("You cannot attack");
            }
            else
            {
            //assigns target for dealing damage
            target = combatTarget;
            currentTarget = combatTarget.gameObject;
            GetComponent<ActionScheduler>().StartAction(this);
            AttackBehavior(combatTarget);
            }

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
            currentAP = maxAP.value;
            turnManager.SetCanAct(true);
        }
        public float GetActionPoints()
        {
            return currentAP;
        }

        private float UpdateActionPoints(float costActionPoints)
        {
            currentAP -= costActionPoints;
            if (currentAP <= currentWeaponConfig.GetActionPointCost())
            {
                turnManager.SetCanAct(false);
            }
            else
            {
                turnManager.SetCanAct(true);
            }
            return currentAP;

        }

        public string DamageAsString()
        {
            return (GetComponent<CharacterStats>().GetDamage(StatType.Damage, currentWeaponConfig) + " " + currentWeaponConfig.GetDamageBonus()).ToString();
        }

        private void OnDrawGizmos() {
            if (currentWeaponConfig != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, currentWeaponConfig.GetRange());
            }

        }

        public float GetWeaponDamage()
        {
            return currentWeaponConfig.GetDamage() + currentWeaponConfig.GetDamageBonus();
        }

        public float GetWeaponRange()
        {
            return currentWeaponConfig.GetRange();
        }
    }
        


}
