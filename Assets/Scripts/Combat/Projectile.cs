using UnityEngine;
using RPG.Attributes;

namespace RPG.Combat
{

    public class Projectile : MonoBehaviour
    {

        public float projectileSpeed = 1f;
        public bool isHomingProjectile = false;
        public GameObject hitEffect = null;
        public float maxLifeTime = 10f;
        public GameObject[] destroyOnHit = null;
        public float lifeAfterImpact = 2;
        public Health target = null;
        public float damage = 0;
        public GameObject instigator = null;


        public virtual void Start()
        {
            transform.LookAt(GetAimLocation());

        }


        // Update is called once per frame
        public virtual void Update()
        {
            if (target == null) return;

            if (isHomingProjectile && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;

            Destroy(gameObject, maxLifeTime);
        }

        public Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;

            // (Vector3.right * targetCapsule.radius) +
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;

            if (target.IsDead()) return;
            
            target.TakeDamage(instigator, damage);

            projectileSpeed = 0;

            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }
            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }
            Destroy(gameObject, lifeAfterImpact);


        }
    }

}