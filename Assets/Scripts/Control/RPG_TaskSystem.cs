using System;
using System.Collections.Generic;
using RPG.Base;
using RPG.Items;
using UnityEngine;

namespace RPG.Control
{
    [System.Serializable]
    public class RPG_TaskSystem
    {
        public abstract class Task {
            public Interactable interactable;
            public ControllableObject controllable;

            public class MoveToPosition : Task {
                public Vector3 targetPoistion;
            }

            public class Talk : Task {
            
            }
            public class Attack : Task
            {
                
            }
            public class Trade : Task
            {

            }
            public class Open : Task
            {

            }
            public class Pickup : Task
            {
                public ItemInInventory item;
  
            }
            public class Inspect : Task
            {

            }
            public class Scan : Task
            {

            }
            public class Default : Task
            {
                public Task task;



            }
           

            
        }

        
        public List<Task> taskList;

        public RPG_TaskSystem ()
        {
            taskList = new List<Task>();

        }

        public Task RequestNextTask() {
            //Player or NPC reqeuseting next task
            if (taskList.Count > 0)
            {
            //assign first task
            Task action = taskList[0];
                taskList.RemoveAt(0);
                return action;
            }
            else {
                //no actions are available
                return null;
            }
        }

        public void AddTask (Task task)
        {
            taskList.Add(task);
            Debug.Log("task added");
        }
    }
}