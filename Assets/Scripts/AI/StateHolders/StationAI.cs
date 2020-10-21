using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.AI
{
    public class StationAI : MonoBehaviour
    {
        public string roomName;
        public List<GameObject> guardObjects;
        public List<GameObject> wanderLocations;
        public List<GameObject> hostilesOnStation;
        public List<SmartRoom> rooms;
        public PatrolPath patrolPath;
        public GOAPStates states;
        public ResourceQueue guards;
        private static ResourceQueue patrol;
        private static ResourceList hostiles;
        private static Dictionary<string, ResourceQueue> resourceQue;
        private static Dictionary<string, ResourceList> resourceList;

        private void Awake()
        {
            states = new GOAPStates();
            resourceList = new Dictionary<string, ResourceList>();
            resourceQue = new Dictionary<string, ResourceQueue>();
            guards = new ResourceQueue(guardObjects, "GuardsAvailable", this.states);
            resourceQue.Add("guards", guards);

            patrol = new ResourceQueue(patrolPath.GetPatrolPath(), "PatrolPointAvailable", this.states);
            resourceQue.Add("patrol", patrol);

            hostiles = new ResourceList(hostilesOnStation, "HostilesOnStation", this.states);
            resourceList.Add("hostiles", hostiles);

        }

        private void Start()
        {
            guardObjects = FindGuards();

        }


        //dead code, may be helpful reference in future
        //IEnumerator LookForHostiles(float checkTime)
        //{
        //    foreach (GAgent ag in GetAgentsOnStation())
        //        if (ag.isHostile && !hostilesPresentInRoom.Contains(ag.room))
        //            hostilesPresentInRoom.Add(ag.room);
                
        //    yield return new WaitForSeconds(checkTime);

        //    StartCoroutine(LookForHostiles(checkTime));
        //}

        public void DeRegisterHostile(GAgent agent)
        {
            int indexToRemove = -1;
            foreach (GameObject ag in GetList("hostiles").GetResourceList())
            {
                indexToRemove++;
                if (ag == agent.gameObject)
                    break;
            }
            if (indexToRemove > -1)
                GetList("hostiles").GetResourceList().RemoveAt(indexToRemove);
        }
        public void RegisterHostile(GAgent agent)
        {
            if (!GetList("hostiles").GetResourceList().Contains(agent.gameObject))
                GetList("hostiles").AddResource(agent.gameObject);
        }


        public void RegisterRoom(SmartRoom room)
        {
            rooms.Add(room);
        }

        public List<SmartRoom> GetRooms()
        {
            return rooms;
        }

        public List<GameObject> GetWander()
        {
            return wanderLocations;
        }

        public List<GAgent> GetAgentsOnStation()
        {
            List<GAgent> agentList = new List<GAgent>();

            foreach (SmartRoom r in rooms)
            {
                foreach (GAgent ag in r.agentsInRoom)
                {

                    agentList.Add(ag);
                }
            }

            return agentList;
        }

        public List<GameObject> FindGuards()
        {
            List<GameObject> agentList = new List<GameObject>();

            foreach (SmartRoom r in rooms)
            {
                foreach (GAgent ag in r.agentsInRoom)
                {
                    if (ag.GetType() == typeof(Guard))
                        agentList.Add(ag.gameObject);

                }
            }

            if (agentList.Count == 0) Debug.Log("Something went wrong");

            return agentList;
        }
            public List<SmartRoom> GetHostileLocation()
        {
            List<SmartRoom> roomsWithHostile = new List<SmartRoom>();
            foreach (SmartRoom r in rooms)
            {
                foreach (GAgent ag in r.agentsInRoom)
                {
                    //if hostile are present, return room.
                }

            }
            return roomsWithHostile;

        }

        public ResourceQueue GetQueue(string type)
        {
            return resourceQue[type];
        }

        public ResourceList GetList(string type)
        {
            return resourceList[type];
        }

        public GOAPStates GetGOAPStates()
        {
            return states;
        }

    }


}