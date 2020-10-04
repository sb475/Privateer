using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    public class Missile : Projectile
    {
        public float intializeTime= 3f;
        Rigidbody missileBody;
        [SerializeField] Vector3 direction;
        public Vector3 launchDirection;
        public float turnSpeed;
        float timer;
        IDamagable missileHealth;

        private void Awake() {
            missileHealth = GetComponent<IDamagable>();
        }
        private void Update() {
            timer += Time.deltaTime;
        }

        public override void Start()
        {
            

            if (missileHealth.IsDead())
            {
                Destroy(this);
            }

            transform.LookAt(launchDirection);
        }



        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (timer > intializeTime)
            {
            direction = GetAimLocation() - this.transform.position;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * turnSpeed);
            }

            
            
            
        }
    }

}