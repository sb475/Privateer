using UnityEngine;
using RPG.Attributes;

namespace RPG.Combat
{

    public class Projectile : MonoBehaviour
    {

        public float projectileSpeed = 1f;
        public GameObject hitEffect = null;
        public float maxLifeTime = 10f;
        public GameObject[] destroyOnHit = null;
        public float lifeAfterImpact = 2;
        public IDamagable target = null;
        public float damage = 0;
        public GameObject instigator = null;
        public float currentSpeed;


        public virtual void Start()
        {
           //transform.LookAt(GetAimLocation());
        }


        // Update is called once per frame
        public virtual void FixedUpdate()
        {
            if (target == null) return;
            currentSpeed = projectileSpeed * Time.deltaTime;
            transform.Translate(0, 0, projectileSpeed * Time.deltaTime);
        }

        public void SetTarget(IDamagable target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;

            DestroyProjectile(gameObject, maxLifeTime);
        }

        public float GetCurrentSpeed ()
        {
            return currentSpeed;
        }

        public Vector3 GetAimLocation()
        {
            Collider targetCollider = target.gameObject.GetComponent<Collider>();
            if (targetCollider == null)
            {
                return target.gameObject.transform.position;
            }
            return targetCollider.bounds.center;

            // (Vector3.right * targetCapsule.radius) +
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            Debug.Log("Hit " + other.gameObject.name);
            if (other.GetComponent<IDamagable>() != target) return;

            if (target.IsDead()) return;

            print(instigator + "hit " + target.gameObject.name + " for " + damage);
            target.TakeDamage(instigator, damage);

            projectileSpeed = 0;

            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach (GameObject toDestroy in destroyOnHit)
            {
                //the zero in this case just refers to destroy immediately.
                DestroyProjectile(toDestroy, 0f);
            }
            DestroyProjectile(gameObject, 0f);

        }

        public virtual void DestroyProjectile(Object obj, float t)
        {
            Destroy(obj, t);
        }
    }

}