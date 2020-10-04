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


        public virtual void Start()
        {
           transform.LookAt(GetAimLocation());
        }


        // Update is called once per frame
        public virtual void FixedUpdate()
        {
            if (target == null) return;

            transform.Translate(0, 0, projectileSpeed * Time.deltaTime);
        }

        public void SetTarget(IDamagable target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;

            Destroy(gameObject, maxLifeTime);
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

        public void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<IDamagable>() != target) return;

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