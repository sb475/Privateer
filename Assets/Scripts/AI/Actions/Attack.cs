﻿using RPG.AI;
using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.AI
{

    public class Attack: GAction
    {
        public Attack()
        {
            actionName = "Attack";
            customDestination = true;
        }

        public override bool PrePerform()
        {
            target = GWorld.Instance.GetList("hostiles").GetResource(0);
            float weaponRange = agent.fighter.GetWeaponRange();
            if (Vector3.Distance(target.transform.position, this.transform.position) < weaponRange)
            {
                actionDestination = this.transform.position;
                return true;
            }
            else
            {
                Vector3 targetDir = this.transform.position - target.transform.position;
                Vector3 inRangeForAttackPos = target.transform.position + targetDir.normalized * weaponRange;
                actionDestination = inRangeForAttackPos;
                return true;
            }  
        }

        public override bool PerformAction()
        {
            //Debug.Log(gameObject.name + " has performed the action " + this)

            Debug.Log("pew pew pew");
            return true;
        }

        public override bool PostPerform()
        {
            //Debug.Log(this + " PostPerform");

            beliefs.RemoveState("threatened");
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