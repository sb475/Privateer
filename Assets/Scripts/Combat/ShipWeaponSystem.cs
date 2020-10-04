    using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Base;
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


        [Header("Weapon Attributes")]
        public bool tracking;
        public float trackingSpeed = 0.5f;
        public float rateOfFire = 3;
        public ShipWeaponConfig currentWeaponConfig;


        public HardPointType hardPointType;
        
        [SerializeField] Transform hardPoint = null;
        [SerializeField] ShipWeaponConfig defaultWeapon = null;
        bool outOfRangeAnnounced = false;
        public Transform launchPosition;
        

        public IDamagable target;
        StateManager turnManager;

        // mathf.infitity allows to attack right away
        float timeSinceLastAttack = Mathf.Infinity;
        
        LazyValue<ShipWeapon> currentWeapon;

        private void Awake()
        {
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<ShipWeapon>(SetupDefaultWeapon);
            turnManager = GetComponent<StateManager>();
            rateOfFire = currentWeaponConfig.GetRateOfFire();
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
            if (gameObject.GetComponent<IDamagable>().IsDead()) return;

            if (target != null) AttackBehavior();

        }

        private void LateUpdate() {
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

        public IDamagable GetTarget()
        {
            return target;
        }

        public void FireOnce( GameObject target)
        {
            this.target = target.GetComponent<IDamagable>(); 
            AttackBehavior();
            this.target = null;
        }

        public void SetTarget(IDamagable target)
        {
            this.target = target;
        }

        public void CeaseFire()
        {
            target = null;
        }

        private void AttackBehavior()
        {
            //reset flag so that next time in range resets.
            if (Vector3.Distance(target.gameObject.transform.position, transform.position) > currentWeaponConfig.GetRange()) return;
       
            outOfRangeAnnounced = false;

            Vector3 direction = target.gameObject.transform.position -this.transform.position;
            if (tracking)
            {
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime*trackingSpeed);
            }
            
            if (timeSinceLastAttack > rateOfFire)
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

        private void OnDrawGizmos()
        {
            if (currentWeaponConfig != null)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(transform.position, currentWeaponConfig.GetRange());
            }
            
        }

    }
        
}