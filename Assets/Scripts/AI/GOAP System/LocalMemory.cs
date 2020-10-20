using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.AI
{
    [System.Serializable]
    public class LocalMemory
    {
        public List<GameObject> items = new List<GameObject>();

        public void AddItem(GameObject i)
        {
            items.Add(i);
        }

        public GameObject FindItemWithTag(string tag)
        {
            foreach (GameObject i in items)
            {
                if (i == null) break;
                if (i.tag == tag)
                {
                    return i;
                }
            }
            return null;
        }

        //how to remove from List.
        public void RemoveItem(GameObject i)
        {
            int indexToRemove = -1;
            foreach (GameObject g in items)
            {
                indexToRemove++;
                if (g == i)
                    break;
            }
            if (indexToRemove > -1)
                items.RemoveAt(indexToRemove);
        }
    }
}