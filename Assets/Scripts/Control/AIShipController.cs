using System.Collections.Generic;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIShipController : MonoBehaviour
    {

        //this script needs to be optimized so that it only happens every 60 frames or so versus every frame

        public Transform target;
        public Vector3 storeTarget;
        public bool savePos;
        public bool overrideTarget;
        public Vector3 newTargetPos;
        public float evadeOffset;

        public float shipWidth;

        ShipEngine engine;
        Transform obstacle;
        public List<Vector3> EscapeDirections = new List<Vector3>();

        private void Start() {

            engine = GetComponent<ShipEngine>();
            shipWidth = GetComponentInChildren<Collider>().bounds.extents.x * 2;
            
        }

        private void Update() {
            FollowTarget(target);
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

       void ObstacleAvoidance(Vector3 direction, float offsetX)
        {

            RaycastHit [] hit = Rays(direction, offsetX);

            for(int i =0; i < hit.Length -1; i++)
            {
                if (hit[i].transform.root.gameObject != this.gameObject)
                {
                    if (!savePos)
                    {
                        storeTarget = target.position;
                        Debug.Log("Storing target. Found obstacles " + storeTarget);
                        obstacle = hit[i].transform;
                        savePos = true;
                    }

                    FindEscapeDirections(hit[i].collider);
                }
            }

            if (EscapeDirections.Count > 0)
            {
                if (!overrideTarget)
                {
                    newTargetPos = getClosests();
                    Debug.Log("New target position is: " + newTargetPos);
                    overrideTarget = true;
                }
            }

            float distance = Vector3.Distance(transform.position, target.position);

            if (distance < 20 + shipWidth)
            {
                if (savePos)
                {
                    target.position = storeTarget;
                    Debug.Log ("Saving target position :" + storeTarget);
                    savePos = false;
                }

                overrideTarget = false;
                EscapeDirections.Clear();
            }
        }


    Vector3 getClosests()
    {
        Vector3 closestEscapeDir = EscapeDirections[0];
        float distance = Vector3.Distance(transform.position, EscapeDirections[0]);

        foreach(Vector3 dir in EscapeDirections)
        {
            
            float tempDistance = Vector3.Distance(transform.position, dir);

            if (tempDistance < distance)
            {
                Debug.Log (dir + " is the closetst");
                distance = tempDistance;
                closestEscapeDir = dir;
            }
        }
        return closestEscapeDir;
    }

    void FindEscapeDirections(Collider col)
    {
        
        RaycastHit hitRight;

        if(Physics.Raycast(col.transform.position, col.transform.right, out hitRight, (col.bounds.extents.x * 2) + shipWidth))
        {}
        else
        {
            Vector3 dir = col.transform.position + new Vector3((col.bounds.extents.x) + shipWidth + evadeOffset, 0, 0);

            if (!EscapeDirections.Contains(dir))
            {
                EscapeDirections.Add(dir);
            }
        }

        RaycastHit hitLeft;

        if (Physics.Raycast(col.transform.position, -col.transform.right, out hitLeft, (col.bounds.extents.x * 2) + shipWidth + evadeOffset))
        { }
        else
        {
            Vector3 dir = col.transform.position + new Vector3((-col.bounds.extents.x * 2) - shipWidth - evadeOffset, 0, 0);

            if (!EscapeDirections.Contains(dir))
            {
                EscapeDirections.Add(dir);
            }
        }

    }

    RaycastHit[] Rays(Vector3 direction, float offsetX)
    {
        Ray ray = new Ray(transform.position, direction);
        Debug.DrawRay(transform.position, direction * 10 * engine.maxSpeed, Color.red);


        float distanceToLookAhead = engine.maxSpeed * 10;

        RaycastHit[] hits = Physics.SphereCastAll(ray, 5, distanceToLookAhead);

        return hits;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }




    }
}