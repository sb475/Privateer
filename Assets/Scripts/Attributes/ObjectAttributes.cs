using RPG.Combat;
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
        public float maxDurability;

        public float currentDurability;

        private void Awake() {
            currentDurability = maxDurability;
        }


        public void Die()
        {
            onDie?.Invoke();
            
            //Add special behaviours here. I.e. if it's a missile do this
            if (GetComponent<Missile>() != null)
            {
                GetComponent<Missile>().DestroyProjectile(gameObject, 0f);
            }
            else
            {
                Destroy(gameObject);
            }
            
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            currentDurability -= damage;
            takeDamage?.Invoke(damage);

            //some kind of roll mechanic involved here

            Debug.Log(this.gameObject.name + " durability is: " + currentDurability);
            if (currentDurability <= 0)
            {
                isDead = true;
                Die();
            }
        }

        //if object being targetted has a ShipWeaponControl system, it will be added to sytem for tracking.
        public void TargetLocked(Missile missile)
        {
            if (GetComponent<ShipWeaponControl>() != null)
            {
                GetComponent<ShipWeaponControl>().DetectIncomingMissiles(missile);
            }
            
        }
    }
}