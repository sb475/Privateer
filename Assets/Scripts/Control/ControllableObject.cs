using System;
using System.Collections;
using RPG.Attributes;
using RPG.Base;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{

    
    public class ControllableObject: MonoBehaviour
    {

        private enum TaskState { waiting, acting, next }

        public Mover mov;
        public StateManager turnManager;
        public Health health;
        public Fighter fighter;
        public delegate void DefaultBehviour();
        public DefaultBehviour defaultBehviour;
        private TaskState taskState;
        public RPG_TaskSystem taskSystem;
        float waitingTimer;
        public delegate void InteractableAction(RPG_TaskSystem.Task task);
        public InteractableAction interactableAction;

        public virtual void Awake()
        {
            mov = GetComponent<Mover>();
            turnManager = GetComponent<StateManager>();
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
        }

        public virtual void Start() {
            taskSystem = new RPG_TaskSystem();
        }

        public virtual void Update() {

            Debug.Log (taskState);
            switch (taskState)
            {
                case TaskState.waiting:
                    waitingTimer -= Time.deltaTime;
                    if (waitingTimer <= 0) {
                        float waitingTimermax = .2f;
                        waitingTimer = waitingTimermax;
                        RequestNextTask();
                    }
                    break;
                case TaskState.acting:
                    break;

            
            }
        }

        public virtual void RequestNextTask()
        {
            RPG_TaskSystem.Task task = taskSystem.RequestNextTask();
            if (task == null) 
            {
                taskState = TaskState.waiting;
            } else {
                taskState = TaskState.acting;
                if (task is RPG_TaskSystem.Task.MoveToPosition)
                {
                    ExecuteTask_Move(task as RPG_TaskSystem.Task.MoveToPosition);
                    return;
                }
                else if (task is RPG_TaskSystem.Task.Attack)
                {
                    StartCoroutine(MoveToAct(task, ()=> ExecuteTask_Attack(task as RPG_TaskSystem.Task.Attack)));
                    return;
                }
                else if (task is RPG_TaskSystem.Task.Talk)
                {
                    StartCoroutine(MoveToAct(task, () => ExecuteTask_Talk(task as RPG_TaskSystem.Task.Talk)));
                    return;
                }
                else if (task is RPG_TaskSystem.Task.Trade)
                {
                    StartCoroutine(MoveToAct(task, () => ExecuteTask_Trade(task as RPG_TaskSystem.Task.Trade)));
                    return;
                }
                else if (task is RPG_TaskSystem.Task.Open)
                {
                    StartCoroutine(MoveToAct(task, () => ExecuteTask_Open(task as RPG_TaskSystem.Task.Open)));
                    return;
                }
                else if (task is RPG_TaskSystem.Task.Pickup)
                {
                    StartCoroutine(MoveToAct(task, () => ExecuteTask_PickUp(task as RPG_TaskSystem.Task.Pickup))); 
                    return;
                }
                else if (task is RPG_TaskSystem.Task.Inspect)
                {
                    StartCoroutine(MoveToAct(task, () => ExecuteTask_Inspect(task as RPG_TaskSystem.Task.Inspect)));
                    return;
                }
                else if (task is RPG_TaskSystem.Task.Scan)
                {
                    StartCoroutine(MoveToAct(task, () => ExecuteTask_Scan(task as RPG_TaskSystem.Task.Scan)));
                    return;
                }
               
               
            }
        }

        public IEnumerator MoveToAct(RPG_TaskSystem.Task task, Action interactableAction)
        {
            //Move into range for action
            yield return new WaitUntil(() => (InRangeToInteract(task.interactable)));
            interactableAction?.Invoke();
        }

        public void ExecuteTask_Move(RPG_TaskSystem.Task.MoveToPosition task)
        {
            MoveToTarget(task.targetPoistion,() =>{
                 RequestNextTask();
             });
        }

         public void MoveToTarget(Vector3 target, Action OnArrival)
         {
             if (turnManager.canMove) 
             {

                 mov.MoveTo(target, 1f);
                 OnArrival?.Invoke();
             }
         }

        public void ExecuteTask_Attack(RPG_TaskSystem.Task.Attack task)
        {
            task.controllable.GetComponent<Fighter>().Attack(task.interactable.gameObject);
            taskState = TaskState.waiting;


            
        }
        public void ExecuteTask_Trade(RPG_TaskSystem.Task.Trade task)
        {
            task.interactable.ShopMenu();
            taskState = TaskState.waiting;
        }
        public void ExecuteTask_Talk(RPG_TaskSystem.Task.Talk task)
        {


            task.interactable.TalkToNPC();
            taskState = TaskState.waiting;
        }
        public void ExecuteTask_Open(RPG_TaskSystem.Task.Open task)
        {
 
            task.interactable.DefaultInteract();
            taskState = TaskState.waiting;
        }
        public void ExecuteTask_PickUp(RPG_TaskSystem.Task.Pickup task)
        {

            task.interactable.PickUp(task.controllable);
            taskState = TaskState.waiting;
        }
        public void ExecuteTask_Inspect(RPG_TaskSystem.Task.Inspect task)
        {

            //add some kind of functionality
            task.interactable.DefaultInteract();
            taskState = TaskState.waiting;
        }
        public void ExecuteTask_Scan(RPG_TaskSystem.Task.Scan task)
        {

            Debug.Log("Scanning ... beep...boop");
            task.interactable.Scan();
            taskState = TaskState.waiting;
        }


        public IEnumerator MoveToAct(Interactable interactable, RPG_TaskSystem.Task task)
        {
            //Move into range for action
            yield return new WaitUntil(() => (InRangeToInteract(interactable)));
            interactableAction(task);
        }

        public void DefaultAct(Interactable interactable) => interactable.DefaultInteract();

        

        bool InRangeToInteract(Interactable interactable)
        {
            float distanceToItem = Vector3.Distance(transform.position, interactable.transform.position);
            if (distanceToItem > interactable.interactRadius)
            {

                mov.MoveToInteract(interactable);
                return false;
                // callingController.MoveToTarget
            }
            else
            {
                return true;
            }
        }

    }
}