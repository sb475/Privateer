using System;
using System.Collections;
using PixelCrushers.DialogueSystem;
using RPG.Attributes;
using RPG.Base;
using RPG.Combat;
using RPG.Events;
using RPG.Items;
using RPG.Movement;
using RPG.Stats;
using RPG.UI;
using UnityEngine;

namespace RPG.Control
{

    [RequireComponent(typeof(IEngine))]
    [RequireComponent(typeof(StateManager))]
    [RequireComponent(typeof(IDamagable))]
    [RequireComponent(typeof(IAttack))]
    public class ControllableObject: MonoBehaviour
    {

        private enum TaskState { waiting, acting, next }

        public IEngine mov;
        public StateManager turnManager;
        public IDamagable health;
        public IAttack IAttack;
        public delegate void DefaultBehaviour();
        public DefaultBehaviour defaultBehaviour;
        [SerializeField] private TaskState taskState;
        public RPG_TaskSystem taskSystem;
        float waitingTimer;
        public delegate void InteractableAction(RPG_TaskSystem.Task task);
        public InteractableAction interactableAction;

        public virtual void Awake()
        {
            mov = GetComponent<IEngine>();
            turnManager = GetComponent<StateManager>();
            health = GetComponent<IDamagable>();
            IAttack = GetComponent<IAttack>();
            taskSystem = new RPG_TaskSystem();
        }

        public virtual void Update() {

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
                //all though messy, this is the switch type that handles different types of actions.
                if (task is RPG_TaskSystem.Task.MoveToPosition)
                {
                    ExecuteTask_Move(task as RPG_TaskSystem.Task.MoveToPosition);
                    return;
                }
                else if (task is RPG_TaskSystem.Task.Attack)
                {
                    ExecuteTask_Attack(task as RPG_TaskSystem.Task.Attack);
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
                else if (task is RPG_TaskSystem.Task.Default)
                {
                    ExecuteTask_Default(task as RPG_TaskSystem.Task.Default);
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

        bool InRangeToInteract(Interactable interactable)
        {
            float distanceToInteractable = Vector3.Distance(transform.position, interactable.transform.position);
            if (distanceToInteractable > interactable.interactRadius)
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

        //This function immediately requests next task to minimize halty movement appearence.
        public void ExecuteTask_Move(RPG_TaskSystem.Task.MoveToPosition task)
        {
            MoveToTarget(task.targetPoistion,() =>{
                 RequestNextTask();
             });
        }

            //special function for just moving to a location
         public void MoveToTarget(Vector3 target, Action OnArrival)
         {
             Debug.Log("MoveToTarget invoked");
             if (turnManager.canMove) 
             {
                 mov.MoveToLocation(target);
                 OnArrival?.Invoke();
             }
         }

        //special function that invokes IAttack's attack sequence versus a "MoveToInteract => Interact" type pattern.
        public void ExecuteTask_Attack(RPG_TaskSystem.Task.Attack task)
        {
            task.controllable.GetComponent<IAttack>().Attack(task.interactable.GetComponent<IDamagable>());
            taskState = TaskState.waiting;

        }

        //standards interaction pattern functions.
        public void ExecuteTask_Trade(RPG_TaskSystem.Task.Trade task)
        {
            if (task.interactable.GetComponent<Shop>() == null)
            {
                Debug.Log ("I don't have anything to trade");
            }
            else
            {
                task.interactable.GetComponent<Shop>().OpenShopMenu();
            }
            
            
            taskState = TaskState.waiting;
        }
        public void ExecuteTask_Talk(RPG_TaskSystem.Task.Talk task)
        {

            if (task.interactable.GetComponent<DialogueSystemTrigger>() == null)
            {
                Debug.Log("I don't have anything to talk about");
                return;
            }
            task.interactable.GetComponent<DialogueSystemTrigger>().OnUse();
            //task.interactable.TalkToNPC(this);
            taskState = TaskState.waiting;
        }
        public void ExecuteTask_Open(RPG_TaskSystem.Task.Open task)
        {
 
            task.interactable.DefaultInteract(this);
            taskState = TaskState.waiting;
        }
        public void ExecuteTask_PickUp(RPG_TaskSystem.Task.Pickup task)
        {   
            ItemInWorld itemInWorld = task.interactable as ItemInWorld;

            GetComponent<Inventory>().AddItem(itemInWorld.PickUpItem());
            taskState = TaskState.waiting;
        }
        public void ExecuteTask_Inspect(RPG_TaskSystem.Task.Inspect task)
        {

            //add some kind of functionality
            task.interactable.DefaultInteract(this);
            taskState = TaskState.waiting;
        }
        public void ExecuteTask_Scan(RPG_TaskSystem.Task.Scan task)
        {
            CharacterStats targetStats = task.interactable.GetComponent<CharacterStats>();
            Debug.Log("Scanning ... beep...boop");

            Debug.Log(gameObject.name);

            // Debug.Log(combatTargetStat.GetStat(Stat.Armor));
            // Debug.Log(combatTargetStat.GetStat(Stat.Health));
            Debug.Log(targetStats.GetLevel());
            taskState = TaskState.waiting;
        }
        public void ExecuteTask_Default(RPG_TaskSystem.Task.Default task)
        {

            task.interactable.DefaultInteract(this);
            taskState = TaskState.waiting;
        }   

    }
}