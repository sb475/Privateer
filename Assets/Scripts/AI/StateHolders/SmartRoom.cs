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
        public List<GAgent> agents;
        public GOAPStates states;
        public bool hostilePresent;
        private static Dictionary<string, ResourceQueue> resourceQue = new Dictionary<string, ResourceQueue>();
        private static Dictionary<string, ResourceList> resourceList;

        private void Awake()
        {
            agents = new List<GAgent>();
            states = new GOAPStates();
            resourceList = new Dictionary<string, ResourceList>();
            cover = new ResourceList(coverObjects, "CoverAvailable", this.states);
            resourceList.Add("cover", cover);
            station = GetComponentInParent<StationAI>();
            station.RegisterRoom(this);

        }

        public void RegisterAgent(GAgent agent, SmartRoom room)
        {
            if (room != null) room.DeRegisterAgent(agent);

            agents.Add(agent);
            if (agent.isHostile) station.hostilesPresentInRoom.Add(this);
        }

        public void DeRegisterAgent(GAgent agent)
        {
            int indexToRemove = -1;
            foreach (GAgent ag in agents)
            {
                indexToRemove++;
                if (ag == agent)
                    break;
            }
            if (indexToRemove > -1)
                agents.RemoveAt(indexToRemove);
        }

        public List<GAgent> GetAgentsInRoom()
        {
            return agents;
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