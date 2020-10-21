using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.AI
{
    public class SmartRoom : MonoBehaviour
    {
        public StationAI station;
        public string roomName;
        public List<GameObject> coverObjects;
        public ResourceList cover;
        public List<GAgent> agentsInRoom;
        public GOAPStates states;
        private static Dictionary<string, ResourceQueue> resourceQue = new Dictionary<string, ResourceQueue>();
        private static Dictionary<string, ResourceList> resourceList;

        private void Awake()
        {
            agentsInRoom = new List<GAgent>();
            states = new GOAPStates();
            resourceList = new Dictionary<string, ResourceList>();
            cover = new ResourceList(coverObjects, "CoverAvailable", this.states);
            resourceList.Add("cover", cover);
            station = GetComponentInParent<StationAI>();


            Time.timeScale = 5f;
        }

        private void Start()
        {
            station.RegisterRoom(this);
        }

        public void RegisterAgent(GAgent agent)
        {
            if (!agentsInRoom.Contains(agent)) 
                agentsInRoom.Add(agent);

            if (agent.isHostile)
                station.RegisterHostile(agent);
        }

        public void DeRegisterAgent(GAgent agent)
        {
            int indexToRemove = -1;
            foreach (GAgent ag in agentsInRoom)
            {
                indexToRemove++;
                if (ag == agent)
                    break;
            }
            if (indexToRemove > -1)
                agentsInRoom.RemoveAt(indexToRemove);
        }

        public List<GAgent> GetAgentsInRoom()
        {
            return agentsInRoom;
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