    using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Core;
using RPG.Items;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class ShipWeaponSystem : MonoBehaviour
    {
        //Ship has several weapon systems depending on ship
        //Turret
        //Rail
        //Machinegun
        //Missile Launcher
        //DTC

        

        public enum HardPointType {
            Turret, Fixed, Missile, Defense
        }

        public class ShipHardPoint {
            public Transform hardPointPosition;
            public HardPointType hardPointType;

        }

        public enum WeaponSystemState { waiting, firing, acquire};

        public bool tracking;
        public HardPointType hardPointType;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform hardPoint = null;
        [SerializeField] ShipWeaponConfig defaultWeapon = null;
        bool outOfRangeAnnounced = false;
        public Transform launchPosition;

        Health target;
        StateManager turnManager;

        // mathf.infitity allows to attack right away
        float timeSinceLastAttack = Mathf.Infinity;
        public ShipWeaponConfig currentWeaponConfig;
        LazyValue<ShipWeapon> currentWeapon;

        private void Awake()
        {
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<ShipWeapon>(SetupDefaultWeapon);
            turnManager = GetComponent<StateManager>();
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

        public virtual void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            // guard conditions
            if (target == null) return;

            //if target is dead, stop firing
            if (target.IsDead()) return;

            // if system is dead, do nothing
            if (gameObject.GetComponent<Health>().IsDead()) return;

            if (target != null) AttackBehavior();

        }

        private ShipWeapon SetupDefaultWeapon()
        {

            return AttachWeapon(defaultWeapon);
        }

        public void EquipWeapon(ShipWeaponConfig weapon)
        {
            if (weapon == null) weapon = defaultWeapon;

            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        private ShipWeapon AttachWeapon(ShipWeaponConfig turret)
        {
            Animator animator = GetComponent<Animator>();
            return turret.Spawn(hardPoint.transform, animator);
        }

        public Health GetTarget()
        {
            return target;
        }

        public void FireOnce( GameObject target)
        {
            this.target = target.GetComponent<Health>(); 
            AttackBehavior();
            this.target = null;
        }

        public void FireAtWill(GameObject target)
        {
            this.target = target.GetComponent<Health>();
        }

        public void CeaseFire()
        {
            target = null;
        }

        private void AttackBehavior()
        {
            //reset flag so that next time in range resets.
            outOfRangeAnnounced = false;
            if (tracking)
            {
                transform.LookAt(target.transform);
            }
            
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                //This will trigger the Hit() event.
                TriggerAttack();
                timeSinceLastAttack = 0;
            }


        }

        private void TriggerAttack()
        {
            //GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("fire");
        }

        // Animation Event
        void Hit()
        {
            Debug.Log("SYSTEM FIRED");

            if (target == null) return; // guard statement

            //this rolls based on the Damage of the weapon + Base damage of the weapon. Weapon damage = 6f, it's a D6 roll.
            float damage = currentWeaponConfig.GetDamage();

            if (currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }
            print(gameObject + "'s damage is: " + damage);
            if (currentWeaponConfig.HasProjectile()) // check to see if it's prjectile weapon
            {
                currentWeaponConfig.LaunchProjectile(launchPosition, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }
            //resets target to avoid auto attacking same target next round.

        }

        public void Shoot() //ties animatin into Hit event
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

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            combatTarget.GetComponent<Health>();

            if (!GetIsInRange(combatTarget))
            {
                print(combatTarget.gameObject.name + " is out of range");
                return;
            }
            //assigns target for dealing damage
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
            ShipWeaponConfig weapon = Resources.Load<ShipWeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }

        public void ClearTarget()
        {
            target = null;
        }

        public string DamageAsString()
        {
            return currentWeaponConfig.GetDamageBonus().ToString();
        }

    }
        
}