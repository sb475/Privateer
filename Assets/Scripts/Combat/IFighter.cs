using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    public interface IFighter
    {
         void Cancel();
         void RestoreActionPoints();
         void Attack(IDamagable target);
         bool CanAttack(IDamagable target);
         float GetWeaponDamage();
         float GetWeaponRange();
         string DamageAsString();
    }
}