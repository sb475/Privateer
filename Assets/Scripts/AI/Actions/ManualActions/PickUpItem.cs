using RPG.AI;
using RPG.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.AI {

    public class PickUpItem: GAction
    {
        ItemInWorld itemToPickup = null;
        public PickUpItem()
        {
            actionName = "PickUpItem";
            customDestination = true;
        }

        public override bool PrePerform()
        {
            target = agent.actionTarget;
            itemToPickup = target.GetComponent<ItemInWorld>();
            if (itemToPickup == null) return false;

            if (Vector3.Distance(target.transform.position, agent.transform.position) < itemToPickup.interactRadius)
            {
                actionDestination = agent.transform.position;
                return true;
            }
            else 
            { 
                Vector3 targetDir = agent.transform.position - target.transform.position;
                Vector3 inRange = target.transform.position + targetDir.normalized * itemToPickup.interactRadius;
                actionDestination = inRange;
                return true;
            }
            
        }

        public override bool PerformAction()
        {
            agent.character.inventory.AddItem(itemToPickup.PickUpItem());
            ActionComplete();
            return true;
        }
    
        public override bool PostPerform()
        {
            agent.actionTarget = null;
            target = null;
            ResetAction();
            return true;
        }

        public override bool OnInterrupt()
        {
            target = null;
            ResetAction();
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
