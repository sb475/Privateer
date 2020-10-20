using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

namespace RPG.AI {
    public class PatrolPath : MonoBehaviour
    {
        const float waypointGizmoRadius = 0.3f;
        private void OnDrawGizmos() {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(GetWaypoint(i), waypointGizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        

        public List<GameObject> GetPatrolPath()
        {
            List<GameObject> pp = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++)
            {
                pp.Add(transform.GetChild(i).gameObject);
            }
            return pp;
        }

        //not being used functions, may be handy later
        public int GetNextIndex(int i)
        {
            if (i + 1 == transform.childCount)
            {
                return 0;
            }
            return i + 1;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}

