using UnityEngine;
using RPG.Attributes;
using RPG.Combat;
using RPG.Base;

namespace RPG.Items
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Item/Make New Weapon", order = 0)]
    public class WeaponConfig : ItemConfig
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Weapon equippedPrefab = null;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float damageBonus = 0;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float actionPointCost = 2f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;
        const string weaponName = "Weapon";

        private void Awake() {
            itemType = ItemType.characterWeapon;
            isEquippable = true;
        }

        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            Weapon weapon = null;

            if (equippedPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                weapon = Instantiate(equippedPrefab, handTransform);
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

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
                if (oldWeapon == null)
                {
                    oldWeapon = leftHand.Find(weaponName);
                    }
                    if (oldWeapon == null) return;

                    oldWeapon.name = "DESTROYING";
                    Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded) handTransform = rightHand;
            else handTransform = leftHand;
            return handTransform;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
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
        public float GetActionPointCost()
        {
            return actionPointCost;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(equippedPrefab.transform.position, weaponRange);
        }
    }
}