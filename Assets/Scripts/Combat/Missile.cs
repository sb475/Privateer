using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    public class Missile : Projectile
    {
        public float intializeTime= 3f;
        Rigidbody missileBody;
        public float turn;
        float timer;

        private void Awake() {
            missileBody = GetComponent<Rigidbody>();
        }

        public override void Start()
        {
            
        }

        public override void Update() {
            timer += Time.deltaTime;
            
        }

        

        float friction = 0.985f; // applied each frame 
        float accel = 40.0f; // meters/second. This might be way off

        private void FixedUpdate()
        {

            missileBody.velocity = transform.forward * projectileSpeed;
            var rocketTargetRotation = Quaternion.LookRotation(GetAimLocation() - transform.position);

            if (timer > intializeTime)
            {
            missileBody.MoveRotation(Quaternion.RotateTowards(transform.rotation, rocketTargetRotation, turn));
            }
            
        }
    }

}