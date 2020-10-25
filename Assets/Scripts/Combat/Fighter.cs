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
using RPG.Base;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IFighter
    {
        Character character;

        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon;

        public IDamagable target;
        
        // mathf.infitity allows to attack right away
        float timeSinceLastAttack = Mathf.Infinity;

        [Header("Current Weapon and Armor")]
        public WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;

        private void Awake() {
            character = GetComponent<Character>();
            defaultWeapon = Resources.Load<WeaponConfig>("Unarmed");
            currentWeaponConfig = defaultWeapon;
            //change this to pull from equipment.
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private void Start() 
        {
            currentWeapon.ForceInit();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            // guard conditions
            if (target == null) return;
            if (target.IsDead()) return;
            if (GetComponent<IDamagable>().IsDead()) return;
        }

        #region AttackFunctions
        public void Attack(IDamagable combatTarget)
        {
            target = combatTarget;
            GetComponent<ActionScheduler>().StartAction(this);
            AttackBehavior(combatTarget);
        }

        public bool CanAttack(IDamagable combatTarget)
        {
            if (combatTarget == null) { return false; }
            IDamagable targetToTest = combatTarget;
            return targetToTest != null && !targetToTest.IsDead();
        }

        private void AttackBehavior(IDamagable target)
        {
            transform.LookAt(target.gameObject.transform);
            //This will trigger the Hit() event.
            if ((timeSinceLastAttack > currentWeaponConfig.rateOfAttack) && (GetIsInRange(target)))
            {
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }
        private bool GetIsInRange(IDamagable combatTarget)
        {
            return Vector3.Distance(transform.position, combatTarget.gameObject.transform.position) < currentWeaponConfig.GetRange();
        }

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
        #endregion


        #region WeaponConfigurations

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }
        
        public void EquipWeapon(WeaponConfig weapon)
        {
            if (weapon == null) weapon = defaultWeapon;

            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        public void EquipPrimary()
        {
            ItemConfig primary;
            character.equipment.equipped.TryGetValue(EquipmentSlots.weapon, out primary);
            EquipWeapon(primary as WeaponConfig);
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }


        #endregion


        #region Animations

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
            float damage = GetComponent<CharacterStats>().CalculateDamage(StatName.Damage, currentWeaponConfig);
            
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
            target = null;

        }

        void Shoot() //ties animatin into Hit event
        {
            Hit();
        }

        #endregion






        #region Getters
        public string DamageAsString()
        {
            return (GetComponent<CharacterStats>().GetDamage(StatName.Damage, currentWeaponConfig) + " " + currentWeaponConfig.GetDamageBonus()).ToString();
        }

        private void OnDrawGizmos() {

            if ( currentWeaponConfig != null)
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

        public void RestoreActionPoints()
        {
            throw new NotImplementedException();
        }

        public IDamagable GetTarget()
        {
            return target;
        }
        #endregion
    
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
    }
        


}
