using RPG.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.AI
{

    public class SeekCover : GAction
    {
        GameObject chosenObj = null;
        public List<GameObject> coverPoints;
        public GameObject testPoint;
        public GameObject lastCoverObj;
        public Vector3 lastCoverPos;
        public SeekCover()
        {
            actionName = "SeekCover";
            customDestination = true;
        }

        public override bool PrePerform()
        {


            float dist = Mathf.Infinity;
            Vector3 chosenSpot = Vector3.zero;

            //Debug.Log(this + " PrePerform");
            if (actionState != ActionState.interrupted || timesInterrupted > 2)
            {
                target = GWorld.Instance.GetList("hostiles").GetResource(0);
                if (target == null) return false;

                //some kind of validation to minimize processing every round.

                //Debug.Log("Cover Points: " + coverPoints.Count);
                if (coverPoints.Count == 0) coverPoints = agent.GetCurrentRoom().GetList("cover").GetResourceList();
                actionDestination = this.transform.position;

                for (int i = 0; i < coverPoints.Count; i++)
                {
                    //Debug.Log(coverPoints[i].name);

                    Vector3 hideDir = coverPoints[i].transform.position - target.transform.position;
                    Vector3 hidePos = coverPoints[i].transform.position + hideDir.normalized * 2;

                    if (Vector3.Distance(this.transform.position, hidePos) < dist)
                    {
                        chosenSpot = hidePos;
                        chosenObj = coverPoints[i];
                        dist = Vector3.Distance(this.transform.position, hidePos);
                    }
                }

                chosenObj = agent.GetCurrentRoom().GetList("cover").RemoveResource(chosenObj);

                actionDestination = chosenSpot;
                //actionDestination = testPoint.transform.position;
                //target = chosenObj
            }
            else
            {
                Debug.Log("Resuming action " + this);
            }


            agent.room.GetGOAPStates().ModifyState("CoverAvailable", -1);

            if (target == null)
            {
                return false;
            }
            return true;
        }

        public override bool PerformAction()
        {
            //Debug.Log(gameObject.name + " has performed the action " + this);
            return true;
        }

        public override bool PostPerform()
        {
            Debug.Log(this + " PostPerform");

            agent.GetCurrentRoom().GetList("cover").AddResource(chosenObj);

            //coverPoints = new List<GameObject>();
            agent.room.GetGOAPStates().ModifyState("CoverAvailable", 1);
            timesInterrupted = 0;
            actionPerformed = false;
            actionComplete = false;
            target = null;
            return true;
        }

        public override bool OnInterrupt()
        {
            agent.GetCurrentRoom().GetList("cover").AddResource(chosenObj);
            //coverPoints = new List<GameObject>();
            agent.room.GetGOAPStates().ModifyState("CoverAvailable", 1);
            timesInterrupted++;
            Debug.Log(this + " was interrupted");
            actionPerformed = false;
            actionComplete = false;
            return true;
        }

        public override void ActionComplete()
        {
            if (!actionComplete)
            {
                //Debug.Log("Action completed successfully");
                //agent.CompleteAction();
                actionComplete = true;
            }


        }
    }


}
