using Language.Lua;
using RPG.AI;
using RPG.Combat;
using RPG.Items;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.AI
{

    public class RushTarget: GAction
    {
        public RushTarget()
        {
            actionName = "Rush Target";
            customDestination = true;
        }

        public override bool PrePerform()
        {
            target = GWorld.Instance.GetList("hostiles").GetResource(0);
            Debug.Log("Rush target is: " + target.name);

            if (target == null) return false;

            //if character is using a range weapon cannot rush. If current weapon has range then check secondary.
            if (agent.fighter.currentWeaponConfig.HasProjectile())
            {
                if (!agent.character.equipment.SwitchToRangedWeapon(false)) return false;
            }

            float weaponRange = agent.fighter.GetWeaponRange();
            if (Vector3.Distance(target.transform.position, agent.transform.position) < weaponRange)
            {
                actionDestination = agent.transform.position;
                return true;
            }
            else
            {
                Vector3 targetDir = agent.transform.position - target.transform.position;
                Vector3 inRangeForAttackPos = target.transform.position + targetDir.normalized * weaponRange;
                actionDestination = inRangeForAttackPos;
                return true;
            }  
        }

        public override bool PerformAction()
        {
            //Debug.Log(gameObject.name + " has performed the action " + this)

            Debug.Log("stab stab stab");
            return true;
        }

        public override bool PostPerform()
        {
            //Debug.Log(this + " PostPerform");

            //if target goes away no longer threatened
            if (target == null) beliefs.RemoveState("threatened");
            //beliefs.RemoveState("readyToEngage");
            target = null;
            timesInterrupted = 0;
            ResetAction();
            return true;
        }

        public override bool OnInterrupt()
        {
            timesInterrupted++;
            ResetAction();
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
