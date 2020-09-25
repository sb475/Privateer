using UnityEngine;
using RPG.Attributes;
using RPG.Combat;
using RPG.Base;

namespace RPG.Items
{
    [CreateAssetMenu(fileName = "Armor", menuName = "Item/Make New Armor", order = 0)]
    public class ArmorConfig : ItemConfig
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Armor equippedPrefab = null;
        [SerializeField] ArmorType armorType;
        const string weaponName = "Armor";

        private void Awake()
        {
            itemType = ItemType.characterArmor;
            isEquippable = true;
            isStackable = false;
        }

        public Armor Spawn(Transform rightHand, Transform leftHand)
        {
            //DestroyOldWeapon(rightHand, leftHand);

            Armor armor = null;

            if (equippedPrefab != null)
            {
                //This should be the armor location or slot. Potential better to use a mesh rendered change. Not sure.
                Transform handTransform = GetTransform(rightHand, leftHand);
                armor = Instantiate(equippedPrefab, handTransform);
                armor.gameObject.name = weaponName;

            }

            return armor;
        }

        private void DestroyOld(Transform rightHand, Transform leftHand)
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
            Transform chestTransform;
            chestTransform = rightHand;
            return chestTransform;
        }
    }
}