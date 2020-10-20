using UnityEngine;
using UnityEngine.Events;
using static RPG.Attributes.Health;

namespace RPG.Attributes
{
    public class ObjectAttributes : MonoBehaviour, IDamagable
    {
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent onDie;
        public bool isDead = false;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        public float durability;

        public void Die()
        {
            onDie?.Invoke();
            Destroy(this);
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            durability -= damage;
            takeDamage?.Invoke(damage);
            //some kind of roll mechanic involved here
            if (durability == 0)
            {
                isDead = true;
                Die();
            }
        }
    }
}