using RPG.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.AI
{

    public class Flee : GAction
    {

        public float fleeDistance = 20f;
        public Flee()
        {
            actionName = "Flee";
            customDestination = true;
        }

        public override bool PrePerform()
        {
            target = GWorld.Instance.GetList("hostiles").GetResource(0);
            if (target == null)
            {
                return false;
            }

            if (!localMemory.items.Contains(target)) localMemory.AddItem(target);
            return true;
        }


        public override bool PerformAction()
        {
            if (Vector3.Distance(target.transform.position, agent.transform.position) < fleeDistance)
            {
                Vector3 fleeVector = (target.transform.position - agent.transform.position).normalized;

                agent.engine.MoveToLocation(agent.transform.position - new Vector3(fleeVector.x, 0, fleeVector.y));
                return false;
            }
            else
            {
                return true;
            }




        }



        public override bool PostPerform()
        {
            Debug.Log("Starting Flee PostPerform");

            ResetAction();
            beliefs.RemoveState("threatened");

            return true;
        }

        public override bool OnInterrupt()
        {
            ResetAction();
            return true;
        }
        public override void ActionComplete()
        {
            if (!actionComplete)
            {
                actionComplete = true;
            }

        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(agent.transform.position, fleeDistance);

        }
    }
}

