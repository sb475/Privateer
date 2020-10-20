using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;
using RPG.Control;

namespace RPG.AI
{

    public class SubGoal
    {
        public Dictionary<string, int> sgoals;
        public bool remove;

        public SubGoal(string s, int i, bool r)
        {
            sgoals = new Dictionary<string, int>();
            sgoals.Add(s, i);
            remove = r;
        }
    }

    public class GAgent : MonoBehaviour
    {
        public List<GAction> actions = new List<GAction>();

        public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();
        public LocalMemory localMemory = new LocalMemory();
        public GOAPStates beliefs = new GOAPStates();

        public Room room;
        //public Station station;
        //public Ship ship;
        //public World world;

        public IEnumerator startAction;

        GPlanner planner;
        Queue<GAction> actionQueue;
        public GAction currentAction;
        public GAction previousAction;
        SubGoal currentGoal;
        bool actionStarted = false;

        Vector3 destination = Vector3.zero;

        // Start is called before the first frame update
        public void Start()
        {
            GAction[] acts = this.GetComponents<GAction>();
            foreach (GAction a in acts)
                actions.Add(a);

            room.agents.Add(this);
        }


        public bool ShouldCancel()
        {
            if (previousAction == null) return true;

            //Debug.Log(previousAction + " " + currentAction);
            //Debug.Log("previousAction == currentAction: " + (previousAction == currentAction));
            //Debug.Log("previousAction.actionState == GAction.ActionState.interrupted: " + (previousAction.actionState == GAction.ActionState.interrupted));
            //Debug.Log("currentAction.actionState == GAction.ActionState.running: " + (currentAction.actionState == GAction.ActionState.running));
            //Debug.Log("previousAction == currentAction && previousAction.actionState == GAction.ActionState.interrupted: " + (previousAction == currentAction && previousAction.actionState == GAction.ActionState.interrupted));

            //conditions not to cancel
            if (previousAction.actionState == GAction.ActionState.interrupted) return false;
            if (previousAction == currentAction && previousAction.actionState == GAction.ActionState.interrupted) return false;


            return true;
        }
        public void CancelCurrentGoal()
        {
            
            if (!ShouldCancel()) return;
            CancelAction();
            currentAction = null;

            if (actionQueue.Count > 0)
                actionQueue.Clear();
        }
        void CancelAction()
        {
            Debug.Log("Cancel " + currentAction);
            StopCoroutine(startAction);
            currentAction.actionState = GAction.ActionState.interrupted;
            currentAction.OnInterrupt();
            previousAction = currentAction;
            actionStarted = false;
        }

        public void CompleteAction()
        {
            currentAction.actionState = GAction.ActionState.complete;
            currentAction.PostPerform();
            actionStarted = false;
        }

        public Room GetCurrentRoom()
        {
            return room;
        }

        public void SetCurretRoom(Room room)
        {
            room.RegisterAgent(this, this.room);
            this.room = room;

        }



        void LateUpdate()
        {

            if (currentAction != null && currentAction.actionState == GAction.ActionState.running)
            {
                if (currentAction.actionComplete)
                {
                    CompleteAction();
                }
                return;
            }

            if (planner == null || actionQueue == null)
            {
                planner = new GPlanner();

                var sortedGoals = from entry in goals orderby entry.Value descending select entry;

                foreach (KeyValuePair<SubGoal, int> sg in sortedGoals)
                {
                    actionQueue = planner.plan(actions, sg.Key.sgoals, beliefs, room.GetGOAPStates(), room.station.GetGOAPStates());
                    if (actionQueue != null)
                    {
                        currentGoal = sg.Key;
                        break;
                    }
                }
            }

            if (actionQueue != null && actionQueue.Count == 0)
            {
                if (currentGoal.remove)
                {
                    goals.Remove(currentGoal);
                }
                planner = null;
            }

            if (actionQueue != null && actionQueue.Count > 0)
            {
                currentAction = actionQueue.Dequeue();

                if (!actionStarted)
                {
                    if (currentAction.PrePerform())
                    {
                        startAction = TakeAction();
                        StartCoroutine(startAction);
                        actionStarted = true;
                    }
                }
                return;
            }
            else
            {
                actionStarted = false;
                actionQueue = null;  
            
            }
        }

        public IEnumerator TakeAction()
        {
            currentAction.actionState = GAction.ActionState.running;
            //if action requires a destination, get it
            if (!currentAction.customDestination)
            {
                //if there's no target, try and find by tag
                if (currentAction.target == null && currentAction.targetTag != "")
                    currentAction.target = GameObject.FindWithTag(currentAction.targetTag);

                    //Debug.Log("Action Destinatio zeroed");
                    if (currentAction.target != null)
                    {
                        //look for a Destination at target and use that
                        Transform dest = currentAction.target.transform.Find("Destination");
                        if (dest != null)
                            destination = dest.position;
                        else
                            destination = currentAction.target.transform.position;

                        //move to destination and when in range, move to next state.
                            currentAction.actionDestination = destination;
                    }

                }

                yield return new WaitUntil(() => (InRangeForAction()));

            //Debug.Log(gameObject.name + " " + this + " permorming action" + currentAction.actionName);
            if (!currentAction.actionPerformed)
            {
                yield return new WaitUntil(() => (currentAction.PerformAction()));
                currentAction.actionPerformed = true;
            }

            //Debug.Log(currentAction + " waiting " + currentAction.duration + " start: " + Time.time);
            yield return new WaitForSeconds(currentAction.duration);
            //Debug.Log("stop: " + Time.time);

            //Debug.Log(this + " action is complete");
            currentAction.ActionComplete();

            }

        public virtual bool InRangeForAction()
        {
            if (currentAction == null) return false;
            float distanceToTarget = Vector3.Distance(currentAction.actionDestination, this.transform.position);
            //this will be updated to get from the target or character as required.
            if (distanceToTarget < 2f)
            {
                return true;
            }
            else
            {
                currentAction.engine.MoveToLocation(currentAction.actionDestination);
                return false;
            }

        }
    }
}
   
