using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.AI
{
    public class Room : MonoBehaviour
    {
        public StationAI station;
        public string roomName;
        public List<GameObject> coverObjects;
        public ResourceList cover;
        public List<GAgent> agents;
        public GOAPStates states;
        private static Dictionary<string, ResourceQueue> resourceQue = new Dictionary<string, ResourceQueue>();
        private static Dictionary<string, ResourceList> resourceList;

        private void Awake()
        {
            states = new GOAPStates();
            resourceList = new Dictionary<string, ResourceList>();
            cover = new ResourceList(coverObjects, "CoverAvailable", this.states);
            station = GetComponentInParent<StationAI>();
            station.RegisterRoom(this);
            //resourceList.Add("cover", cover);
        }

        public void RegisterAgent(GAgent agent, Room room)
        {
            room.DeRegisterAgent(agent);
            agents.Add(agent);
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

        public ResourceList GetLocalResources()
        {
            return cover;
        }

    }


}