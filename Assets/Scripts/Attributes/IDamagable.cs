using RPG.Combat;
using UnityEngine;

namespace RPG.Attributes
{
    public interface IDamagable
    {
        void TakeDamage(GameObject instigator, float damage);
        void Die();
        bool IsDead();
        void TargetLocked(Missile missile);

        GameObject gameObject { get; }
    }
}