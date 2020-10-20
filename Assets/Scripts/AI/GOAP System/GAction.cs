using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.AI
{ 

    public abstract class GAction : MonoBehaviour
    {

        public enum ActionState { running, complete, interrupted, pending}
        public string actionName = "Action";
        public float cost = 1.0f;
        public GameObject target;
        public string targetTag;
        public float duration = 0f;
        public GOAPState[] preConditions;
        public GOAPState[] afterEffects;
        public IEngine engine;
        public GAgent agent;

        public Dictionary<string, int> preconditions;
        public Dictionary<string, int> effects;

        public GOAPStates agentBeliefs;
        public LocalMemory localMemory;
        public GOAPStates beliefs;

        public ActionState actionState;
        public bool actionPerformed = false;
        public bool actionComplete = false;
        public bool customDestination = false;
        public Vector3 actionDestination = Vector3.zero;
        //internal bool inRange;
        public int timesInterrupted = 0;

        public GAction()
        {
            preconditions = new Dictionary<string, int>();
            effects = new Dictionary<string, int>();
        }

        public void Awake()
        {
            engine = this.gameObject.GetComponent<IEngine>();
            agent = this.gameObject.GetComponent<GAgent>();
            localMemory = agent.localMemory;
            beliefs = agent.beliefs;

            actionState = ActionState.pending;

            if (preConditions != null)
                foreach (GOAPState w in preConditions)
                {
                    preconditions.Add(w.key, w.value);
                }

            if (afterEffects != null)
                foreach (GOAPState w in afterEffects)
                {
                    effects.Add(w.key, w.value);
                }

        }

        public bool IsAchievable()
        {
            return true;
        }

        public bool IsAchievableGiven(Dictionary<string, int> conditions)
        {
            foreach (KeyValuePair<string, int> p in preconditions)
            {
                if (!conditions.ContainsKey(p.Key))
                    return false;
            }
            return true;
        }


        //public virtual bool InRangeForAction()
        //{
        //    float distanceToTarget = Vector3.Distance(actionDestination, this.transform.position);
        //    if (distanceToTarget < 2f)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        engine.MoveToLocation(actionDestination);
        //        return false;
        //    }
           
        //}

        public void ResetAction()
        {
            actionPerformed = false;
            actionComplete = false;
        }


        public abstract bool PrePerform();
        public abstract bool PostPerform();
        public abstract bool OnInterrupt();
        public abstract bool PerformAction();
        public abstract void ActionComplete();

    }
}