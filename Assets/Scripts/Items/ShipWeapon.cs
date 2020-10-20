using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Items
{
   
    public class ShipWeapon : MonoBehaviour
    {
        public bool IsTracking;
        GameObject target;

        [SerializeField] UnityEvent onHit;
        
        public void OnHit()
        {
            onHit.Invoke();
        }

        private void Update() {
            if (target != null)
            {
                TrackTarget(target);
            }
        }

        public void TrackTarget (GameObject target)
        {   this.target = target;
            //need to play with this to figure out how it will behave.
            Vector3 direction = (target.transform.position - transform.position).normalized;
            Quaternion loookRoation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, loookRoation, Time.deltaTime * 5f);
        }

        public void StopTrackingTarget()
        {
            this.target = null;
        }

        


    }
}
