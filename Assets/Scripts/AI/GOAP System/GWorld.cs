using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.AI
{
    [System.Serializable]
    public class ResourceQueue
    {
        public Queue<GameObject> que = new Queue<GameObject>();
        public string tag;
        public string modState;

        public ResourceQueue(string t, string ms, GOAPStates w)
        {
            tag = t;
            modState = ms;
            if (tag != "")
            {
                GameObject[] resources = GameObject.FindGameObjectsWithTag(tag);
                foreach (GameObject r in resources)
                    que.Enqueue(r);
            }

            if (modState != "")
            {
                w.ModifyState(modState, que.Count);
            }
        }

        public ResourceQueue(List<GameObject> listOfResources, string ms, GOAPStates w)
        {
            modState = ms;
            foreach (GameObject r in listOfResources)
            {
                que.Enqueue(r);
                //Debug.Log("Added " + r.name);
            }

            if (modState != "")
            {
                //Debug.Log(modState + " is being added to " + w);
                w.ModifyState(modState, que.Count);
            }
        }

        public void AddResource(GameObject r)
        {
            que.Enqueue(r);
        }

        public void RemoveResource(GameObject r)
        {
            que = new Queue<GameObject>(que.Where(p => p != r));
        }

        public GameObject RemoveResource()
        {
            if (que.Count == 0) return null;
            return que.Dequeue();
        }

    }
    [System.Serializable]
    public class ResourceList
    {
        public List<GameObject> list = new List<GameObject>();
        public string tag;
        public string modState;

        public ResourceList(string t, string ms, GOAPStates w)
        {
            tag = t;
            modState = ms;
            if (tag != "")
            {
                GameObject[] resources = GameObject.FindGameObjectsWithTag(tag);
                foreach (GameObject r in resources)
                    list.Add(r);
            }

            if (modState != "")
            {
                w.ModifyState(modState, list.Count);
            }
        }

        //override for normal behaviour
        public ResourceList(List<GameObject> listOfResources, string ms, GOAPStates w)
        {
            modState = ms;
            foreach (GameObject r in listOfResources)
            {
                list.Add(r);
                //Debug.Log("Added " + r.name);
            }

            if (modState != "")
            {
                //Debug.Log(modState + " is being added to " + w);
                w.ModifyState(modState, list.Count);
            }
        }

        public void AddResource(GameObject r)
        {
            list.Add(r);

        }
        public void ReportResource(string ms, GOAPStates w)
        {
            modState = ms;
            w.ModifyState(modState, list.Count);

        }

        public GameObject RemoveResource(GameObject r)
        {
            GameObject temp = r;
            list.Remove(r);
            return temp;
        }

        public List<GameObject> GetResourceList()
        {
            return list;
        }

        public GameObject GetResource(int i)
        {
            if (list.Count == 0 || i > list.Count-1)
            {
                return null;
            }
            else
            {
                return list[i];
            }
        }
        public int GetListRange()
        {
            return list.Count;
        }

    }

    public sealed class GWorld
    {
        private static readonly GWorld instance = new GWorld();
        private static GOAPStates states;
        private static ResourceList locations;
        //private static ResourceQueue cubicles;
        //private static ResourceQueue offices;
        //private static ResourceQueue toilets;
        //private static ResourceQueue puddles;
        private static Dictionary<string, ResourceQueue> resourceQue = new Dictionary<string, ResourceQueue>();
        private static Dictionary<string, ResourceList> resourceList = new Dictionary<string, ResourceList>();

        static GWorld()
        {
            states = new GOAPStates();
            locations = new ResourceList("Location", "LocationAvailable", states);
            resourceList.Add("locations", locations);
            locations = new ResourceList("Hostile", "HostilePresent", states);
            resourceList.Add("hostiles", locations);
           
        }

        public ResourceQueue GetQueue(string type)
        {
            return resourceQue[type];
        }

        public ResourceList GetList(string type)
        {
            return resourceList[type];
        }

        public static GWorld Instance
        {
            get { return instance; }
        }

        public GOAPStates GetGOAPStates()
        {
            return states;
        }
    }
}
