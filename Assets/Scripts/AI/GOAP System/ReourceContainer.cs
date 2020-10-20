using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.AI
{ 
    public class ResourceContainer : ScriptableObject
    {
        public static GOAPStates states;
        //private static ResourceList locations;
        //private static ResourceQueue resource;

        public ResourceContainer()
        {
            states = new GOAPStates();
        }

        public static Dictionary<string, ResourceQueue> resourceQue = new Dictionary<string, ResourceQueue>();
        public static Dictionary<string, ResourceList> resourceList = new Dictionary<string, ResourceList>();

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