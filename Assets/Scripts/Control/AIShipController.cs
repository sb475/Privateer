using System.Collections.Generic;
using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIShipController : MonoBehaviour
    {

        //this script needs to be optimized so that it only happens every 60 frames or so versus every frame

        public ShipWeaponControl shipWeaponControl;

        public Transform target;
        public Vector3 storeTarget;
        public bool savePos;
        public bool overrideTarget;
        public Vector3 newTargetPos;
        public float evadeOffset;
        public AttitudeType attitude;

        public float shipWidth;

        ShipEngine engine;
        Transform obstacle;
        public List<Vector3> EscapeDirections = new List<Vector3>();

        private void Start() {

            engine = GetComponent<ShipEngine>();
            shipWidth = GetComponentInChildren<Collider>().bounds.extents.x * 2;
            shipWeaponControl = GetComponent<ShipWeaponControl>();
        }

        private void Update() {
            Behaviour();
        }
           
        public void FollowTarget(Transform target) {
            engine.StartMoveAction(target.position, 1f);
        }

        private void FixedUpdate() {
            // ObstacleAvoidance(transform.forward, 0);

            // if (overrideTarget)
            // {
            //     target.position = newTargetPos;
            // }
        }

    public void Behaviour ()
    {
        if (attitude == AttitudeType.Hostile)
        {
            if (target != null)
            {
                    
                Debug.Log(target.GetComponent<ControllableObject>());
                if (target.GetComponent<ControllableObject>() != null)
                {
                    EngageShip(target);
                }

            }
        }
        else if (attitude == AttitudeType.Neutral)
        {
            Wander();
        }
    }

    void OrbitAroundAtRange(float range, Vector3 position)
    {
        float orbitRange;
        float orbit;


    }

    Vector3 orbitPosition = Vector3.zero;


    Vector3 wanderTarget = Vector3.zero;
    void Wander ()
        {
            float wanderRadius = 10;
            float wanderDistance = 30;
            float wanderJitter = 1;

            wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,
                                                        0,
                                                        Random.Range(-1.0f, 1.0f) * wanderJitter);
            wanderTarget.Normalize();
            wanderTarget *= wanderRadius;

            Vector3 targetLocal = wanderTarget + new Vector3(0,0,wanderDistance);
            Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);
            engine.StartMoveAction(targetWorld, 1f);
        }

    public void Flee(Vector3 direction) {
        Vector3 fleeVector = direction - this.transform.position;
        engine.StartMoveAction((this.transform.position - fleeVector), 1f);
    }

    // public void OrbitAround(Vector3 target, float distance)
    // {
    //     Vector3 orbitDirection = 
    // }

    public void Evade()
    {
        
    }

    public void EngageShip (Transform target)
    {
        Debug.Log("Engaging " + target.name);
        engine.StartMoveAction(target.position, 1f);
        if (Vector3.Distance(target.position, transform.position) <= shipWeaponControl.OptimalWeaponRange())
        {
            engine.Cancel();
        }
        shipWeaponControl.Attack(target.GetComponent<IDamagable>());
    }

    public AttitudeType GetAttitude()
    {
        return attitude;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }




    }
}















// void ObstacleAvoidance(Vector3 direction, float offsetX)
// {

//     RaycastHit[] hit = Rays(direction, offsetX);

//     for (int i = 0; i < hit.Length - 1; i++)
//     {
//         if (hit[i].transform.root.gameObject != this.gameObject)
//         {
//             if (!savePos)
//             {
//                 storeTarget = target.position;
//                 Debug.Log("Storing target. Found obstacles " + storeTarget);
//                 obstacle = hit[i].transform;
//                 savePos = true;
//             }

//             FindEscapeDirections(hit[i].collider);
//         }
//     }

//     if (EscapeDirections.Count > 0)
//     {
//         if (!overrideTarget)
//         {
//             newTargetPos = getClosests();
//             Debug.Log("New target position is: " + newTargetPos);
//             overrideTarget = true;
//         }
//     }

//     float distance = Vector3.Distance(transform.position, target.position);

//     if (distance < 20 + shipWidth)
//     {
//         if (savePos)
//         {
//             target.position = storeTarget;
//             Debug.Log("Saving target position :" + storeTarget);
//             savePos = false;
//         }

//         overrideTarget = false;
//         EscapeDirections.Clear();
//     }
// }


// Vector3 getClosests()
// {
//     Vector3 closestEscapeDir = EscapeDirections[0];
//     float distance = Vector3.Distance(transform.position, EscapeDirections[0]);

//     foreach (Vector3 dir in EscapeDirections)
//     {

//         float tempDistance = Vector3.Distance(transform.position, dir);

//         if (tempDistance < distance)
//         {
//             Debug.Log(dir + " is the closetst");
//             distance = tempDistance;
//             closestEscapeDir = dir;
//         }
//     }
//     return closestEscapeDir;
// }

// void FindEscapeDirections(Collider col)
// {

//     RaycastHit hitRight;

//     if (Physics.Raycast(col.transform.position, col.transform.right, out hitRight, (col.bounds.extents.x * 2) + shipWidth))
//     { }
//     else
//     {
//         Vector3 dir = col.transform.position + new Vector3((col.bounds.extents.x) + shipWidth + evadeOffset, 0, 0);

//         if (!EscapeDirections.Contains(dir))
//         {
//             EscapeDirections.Add(dir);
//         }
//     }

//     RaycastHit hitLeft;

//     if (Physics.Raycast(col.transform.position, -col.transform.right, out hitLeft, (col.bounds.extents.x * 2) + shipWidth + evadeOffset))
//     { }
//     else
//     {
//         Vector3 dir = col.transform.position + new Vector3((-col.bounds.extents.x * 2) - shipWidth - evadeOffset, 0, 0);

//         if (!EscapeDirections.Contains(dir))
//         {
//             EscapeDirections.Add(dir);
//         }
//     }

// }

// RaycastHit[] Rays(Vector3 direction, float offsetX)
// {
//     Ray ray = new Ray(transform.position, direction);
//     Debug.DrawRay(transform.position, direction * 10 * engine.maxSpeed, Color.red);


//     float distanceToLookAhead = engine.maxSpeed * 10;

//     RaycastHit[] hits = Physics.SphereCastAll(ray, 5, distanceToLookAhead);

//     return hits;
// }
