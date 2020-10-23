using RPG.AI;
using RPG.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.AI {

    public class MoveToPoint: GAction
    {
        public MoveToPoint()
        {
            actionName = "MoveToPoint";
            customDestination = true;
        }

        public override bool PrePerform()
        {
            actionDestination = agent.actionDestination;
            ActionComplete();
            return true;
        }

        public override bool PerformAction()
        {
            return true;
        }
    
        public override bool PostPerform()
        {
            target = null;
            agent.actionDestination = new Vector3();
            return true;
        }

        public override bool OnInterrupt()
        {
            target = null;
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
