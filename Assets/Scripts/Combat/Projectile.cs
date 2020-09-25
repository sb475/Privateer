﻿using UnityEngine;
using RPG.Attributes;

namespace RPG.Combat
{

    public class Projectile : MonoBehaviour
    {

        [SerializeField] float projectileSpeed = 1f;
        [SerializeField] bool isHomingProjectile = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 10f;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 2;
        Health target = null;
        float damage = 0;
        GameObject instigator = null;


        private void Start()
        {
            transform.LookAt(GetAimLocation());

        }


        // Update is called once per frame
        void Update()
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

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;

            // (Vector3.right * targetCapsule.radius) +
        }

        private void OnTriggerEnter(Collider other)
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