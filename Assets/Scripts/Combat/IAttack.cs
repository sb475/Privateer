using UnityEngine;

namespace RPG.Combat
{
    public interface IAttack
    {
         void Cancel();
         void RestoreActionPoints();
         void Attack(GameObject target);
         bool CanAttack(GameObject target);
         string DamageAsString();
    }
}