using RPG.Attributes;
using RPG.Base;
using RPG.Combat;
using UnityEngine;
using static RPG.Combat.ShipWeaponSystem;

namespace RPG.Items
{
    [CreateAssetMenu(fileName = "ShipSystem", menuName = "Item/Make New ShipWeapon", order = 0)]
    public class ShipWeaponConfig : ItemConfig
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] ShipWeapon equippedPrefab = null;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float damageBonus = 0;
        [SerializeField] float weaponRange = 100f;
        [SerializeField] float actionPointCost = 0f;
        [SerializeField] Projectile projectile = null;
        public HardPointType hardPointType;
        const string weaponName = "ShipSystem";

        private void Awake()
        {
            itemType = ItemType.characterWeapon;
            isEquippable = true;
        }

        public ShipWeapon Spawn(Transform hardPoint, Animator animator)
        {
            DestroyOldWeapon(hardPoint);

            ShipWeapon weapon = null;

            if (equippedPrefab != null)
            {

                weapon = Instantiate(equippedPrefab, hardPoint.transform);
                weapon.gameObject.name = weaponName;
            }


            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }

            return weapon;
        }

        private void DestroyOldWeapon(Transform hardPoint)
        {
            Transform oldWeapon = hardPoint.Find(weaponName);
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }
        
        //this where we will be able to add specific details for targetting. Also need to update location for tip of barrel.
        public void LaunchProjectile(Transform hardPoint, Health target, GameObject instigator, float calculatedDamage)
        {
            
            Projectile projectileInstance =  projectileInstance = Instantiate(projectile, hardPoint.position, Quaternion.LookRotation(hardPoint.forward));                       
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public float GetDamage()
        {
            return weaponDamage;
        }
        public float GetRange()
        {
            return weaponRange;
        }
        public float GetDamageBonus()
        {
            return damageBonus;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(equippedPrefab.transform.position, weaponRange);
        }
    }
    
}