using System.Collections.Generic;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIShipController : MonoBehaviour
    {

        public Transform target;
        Vector3 storeTarget;
        bool savePos;
        bool overrideTarget;

        ShipEngine engine;

        public List<Vector3> EscapeDirection = new List<Vector3>();


        private void Start() {
            // storeMaxSpeed = maxSpeed;
            // targetSpeed = storeMaxSpeed;

            engine = GetComponent<ShipEngine>();
            engine.StartMoveAction(target.position, 1f);
            
        }

        private void Update() {
            FollowTarget(target);
        }
           
        public void FollowTarget(Transform target) {
            engine.MoveTo(transform.position, 1f);
        }
    }
}