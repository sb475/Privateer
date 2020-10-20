﻿using RPG.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.AI
{
    public class Wander: GAction
    {
        int nextPoint;
        public Wander()
        {
            actionName = "Wander";
        }
       
        public override bool PrePerform()
        {
            engine.SetSpeed(1.558f);

            if (actionState != ActionState.interrupted || timesInterrupted > 2)
            {
                nextPoint = Random.Range(0, GWorld.Instance.GetList("locations").GetListRange());
                //Debug.Log(nextPoint);
                target = GWorld.Instance.GetList("locations").GetResource(nextPoint);

                if (target == null)
                {
                    return false;
                }

                if (!localMemory.items.Contains(target))
                    localMemory.AddItem(target);
            }
            else
            {
                Debug.Log("Resuming action " + this);
            }

            GWorld.Instance.GetGOAPStates().ModifyState("LocationAvailable", -1);
            return true;
        }

        public override bool PerformAction()
        {
            //Debug.Log(gameObject.name + " has performed the action " + this);
            return true;
        }
    
        public override bool PostPerform()
        {
            engine.SetSpeed(5.66f);
            //GWorld.Instance.GetList("locations").AddResource(target);
            localMemory.RemoveItem(target);
            GWorld.Instance.GetGOAPStates().ModifyState("LocationAvailable", 1);
            timesInterrupted = 0;
            actionPerformed = false;
            actionComplete = false;
            target = null;
            return true;
        }

        public override bool OnInterrupt()
        {
            engine.SetSpeed(5.66f);
            GWorld.Instance.GetGOAPStates().ModifyState("LocationAvailable", 1);
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
