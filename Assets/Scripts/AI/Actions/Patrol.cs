using RPG.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.AI {

    public class Patrol: GAction
    {
        public Patrol()
        {
            actionName = "Patrol";
        }

        public override bool PrePerform()
        {
            duration = Random.Range(10, 30);
            agent.engine.SetSpeed(1.558f);
            if (actionState != ActionState.interrupted || timesInterrupted > 2)
            {
                //Debug.Log(nextPoint);
                target = agent.room.station.GetQueue("patrol").RemoveResource();
 
                if (target == null)
                {
                    return false;
                }

            }
            else
            {
                Debug.Log("Resuming action " + this);
            }

            agent.room.station.states.ModifyState("PatrolPointAvailable", -1);

            return true;
        }

        public override bool PerformAction()
        {
            //Debug.Log(gameObject.name + " has performed the action " + this);
            return true;
        }
    
        public override bool PostPerform()
        {
            agent.room.station.GetQueue("patrol").AddResource(target);
            agent.room.station.states.ModifyState("PatrolPointAvailable", 1);
            agent.engine.SetSpeed(5.66f);
            //GWorld.Instance.GetList("locations").AddResource(target);
            timesInterrupted = 0;
            actionPerformed = false;
            actionComplete = false;
            target = null;
            return true;
        }

        public override bool OnInterrupt()
        {
            agent.room.station.GetQueue("patrol").AddResource(target);
            agent.room.station.states.ModifyState("PatrolPointAvailable", 1);
            agent.engine.SetSpeed(5.66f);
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
                actionComplete = true;
            }

        
        }
    }


}
