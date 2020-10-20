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
        public List<Room> rooms;
        public PatrolPath patrolPath;
        public GOAPStates states;
        public ResourceQueue guards;
        private static ResourceQueue patrol;
        private static Dictionary<string, ResourceQueue> resourceQue;
        private static Dictionary<string, ResourceList> resourceList;

        private void Awake()
        {
            states = new GOAPStates();
            resourceList = new Dictionary<string, ResourceList>();
            resourceQue = new Dictionary<string, ResourceQueue>();
            
        }

        private void Start()
        {
            guardObjects = FindGuards();
            guards = new ResourceQueue(guardObjects, "GuardsAvailable", this.states);
            resourceQue.Add("guards", guards);

            patrol = new ResourceQueue(patrolPath.GetPatrolPath(), "PatrolPointAvailable", this.states);
            resourceQue.Add("patrol", patrol);
        }

        public void RegisterRoom(Room room)
        {
            rooms.Add(room);
        }

        public List<Room> GetRooms()
        {
            return rooms;
        }

        public List<GAgent> FindAgentType(Type agentType)
        {
            List<GAgent> agentList = new List<GAgent>();

            foreach (Room r in rooms)
            {
                foreach (GAgent ag in r.agents)
                {

                    agentList.Add(ag);
                }
            }

            return agentList;
        }

        public List<GameObject> FindGuards()
        {
            List<GameObject> agentList = new List<GameObject>();

            foreach (Room r in rooms)
            {
                foreach (GAgent ag in r.agents)
                {
                    if (ag.GetType() == typeof(Guard))
                        agentList.Add(ag.gameObject);

                }
            }

            if (agentList.Count == 0) Debug.Log("Something went wrong");

            return agentList;
        }
            public List<Room> GetHostileLocation()
        {
            List<Room> roomsWithHostile = new List<Room>();
            foreach (Room r in rooms)
            {
                foreach (GAgent ag in r.agents)
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