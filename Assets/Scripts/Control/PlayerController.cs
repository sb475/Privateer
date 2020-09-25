// using UnityEngine;
// using RPG.Movement;
// using RPG.Combat;
// using RPG.Attributes;
// using System;
// using UnityEngine.EventSystems;
// using UnityEngine.AI;
// using RPG.Stats;
// using RPG.Items;
// using RPG.UI;
// using System.Collections.Generic;
// using RPG.Base;
// using System.Collections;
// using RPG.Events;

// namespace RPG.Control
// {
//     public class CrewController : MonoBehaviour
//     {
//         Health health;
//         StateManager turnManager;
//         Interactable interactable;
//         public delegate void InteractableAction(CrewController callingController);
//         public InteractableAction interactableAction;

//         public List<Interactable> cachePlayerObjects = new List<Interactable>();

//         //template for perks to be added
//         [System.Serializable]
//         struct CursorMapping
//         {
//             public CursorType type;
//             public Texture2D texture;
//             public Vector2 hostpot;
//         }

//         [SerializeField] CursorMapping[] cursorMappings = null;
//         [SerializeField] float maxNavMeshProjectionDistance = 1f;
//         [SerializeField] float maxNavPathLength = 40;

//         private Inventory inventory;

//         [SerializeField] private UIPlayerInventory uIInventory; 

//         public Interactable focus;
//         public Mover mov;

//         bool isDraggingUI = false;

//         private void Awake()
//         {
//             health = GetComponent<Health>();
//             turnManager = GetComponent<StateManager>();
//             mov = GetComponent<Mover>();
//             inventory = GetComponent<Inventory>();
//             cachePlayerObjects = null;
            
            
//         }

//         private void Start() {
            

//         }
//         private void Update()
//         {
//             //armorClass = GetComponent<BaseStats>().GetStat(Stat.ArmorClass);

//             if (InteractWithUI()) return;

//             if (health.IsDead())
//             {
//                 SetCursor(CursorType.None);
//                 return;
//             }
//             if (InteractWithObject()) return;
//             if (InteractWithMovement()) return;
//             // print ("Nothing to do.");
//             SetCursor(CursorType.None);
//         }

//         private bool InteractWithObject()
//         {
//             //iterates through physical object in raycas
//             RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
//             foreach (RaycastHit hit in hits)
//             {
//                 //looks for components to apply to IRaycastable
//                 IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
//                 foreach (IRaycastable raycastable in raycastables)
//                 {
//                     if (raycastable.HandleRaycast(this))
//                     {
//                         SetCursor(raycastable.GetCursorType(this));

//                         if (Input.GetMouseButtonDown(0))
//                         {
//                             StopAllCoroutines();
//                             ActionMenu.HideMenuOptions_Static();

//                             interactable = hit.collider.GetComponent<Interactable>();
//                             if (interactable != null)
//                             {
//                                 StartCoroutine(DefaultAct(this));
//                             }
//                         }

//                         if (Input.GetMouseButtonDown(1))
//                         {
//                             ActionMenu.HideMenuOptions_Static();

//                             interactable = hit.collider.GetComponent<Interactable>();
//                             if (interactable != null)
//                             {
//                                 ActionMenu.HideMenuOptions_Static();
//                                 ActionMenu.ShowMenuOptions_Static(interactable);
                                
//                             }
//                         }

                        
//                         return true;
//                     }
//                 }
//             }
//             return false;
//         }

//         public IEnumerator DefaultAct(CrewController callingController)
//         {
//             //Move into range for action
//             yield return new WaitUntil(() => (InRangeToInteract(callingController)));
//             //Act
//             interactable.DefaultInteract(callingController);
//         }

// #region ActionMenuFunctions
//         public IEnumerator Act(CrewController callingController)
//         {
//             //Move into range for action
//             yield return new WaitUntil(() => (InRangeToInteract(callingController)));
//             //Act
//             interactableAction(callingController);
//         }

//         public void Attack(CrewController callingController)
//         {
//             CharacterInteraction newInteractable = (CharacterInteraction)interactable;
//             newInteractable.ShopMenu(callingController);
//         }
//         public void Trade(CrewController callingController)
//         {
//             CharacterInteraction newInteractable = (CharacterInteraction)interactable;
//             newInteractable.ShopMenu(callingController);
//         }
//         public void Talk(CrewController callingController)
//         {
//             CharacterInteraction newInteractable = (CharacterInteraction)interactable;
//             newInteractable.TalkToNPC(callingController);
//         }
//         public void Move(CrewController callingController)
//         {
//             MoveToTarget(interactable.transform.position);
//         }
//         public void Open(CrewController callingController)
//         {
//             interactable.DefaultInteract(callingController);
//         }
//         public void PickUp(CrewController callingController)
//         {
//             ItemInWorld newInteractable = (ItemInWorld)interactable;
//             newInteractable.PickUp(callingController);
//         }
//         public void Inspect(CrewController callingController)
//         {
//             //add some kind of functionality
//             interactable.DefaultInteract(callingController);
//         }
//         public void Scan(CrewController callingController)
//         {
//             Debug.Log ("Scanning ... beep...boop");
//             CombatTarget newInteractable = (CombatTarget)interactable;
//             newInteractable.Scan(callingController);
//         }

// #endregion

//         bool InRangeToInteract(CrewController callingController)
//         {
//             float distanceToItem = Vector3.Distance(callingController.transform.position, interactable.transform.position);
//             if (distanceToItem > interactable.interactRadius)
//             {
//                 callingController.mov.MoveToInteract(interactable);
//                 return false;
//                 // callingController.MoveToTarget
//             }
//             else {
//                 return true;
//             }        
//         }

//         private void SetFocus(Interactable newFocus)
//         {
//             focus = newFocus;
//             mov.FollowTarget(newFocus);
//         }

//         private void RemoveFocus()
//         {
//             focus = null;
//             mov.StopFollowTarget();
//         }


//         // reference this for making complex arrays based on others.
       


//         private bool InteractWithUI()
//         {
//             if (Input.GetMouseButtonUp(0))
//             {
//                 isDraggingUI = false;
//             }
//             if (EventSystem.current.IsPointerOverGameObject())
//             {
//                 if (Input.GetMouseButtonDown(0))
//                 {
//                     isDraggingUI = true;
//                 }
//                 SetCursor(CursorType.UI);
//                 return true;
//             }
//             if (isDraggingUI)
//             {
//                 return true;
//             }
//             return false;
//         }



//         private bool InteractWithMovement()
//         {

//             Vector3 target;
//             bool hasHit = RaycastNavMesh(out target);
//             if (hasHit)
//             {
//                 if (Input.GetMouseButtonDown(0) && turnManager.GetCanMove() == true)
//                 {
//                     StopAllCoroutines();
//                     ActionMenu.HideMenuOptions_Static();
//                     RemoveFocus();
//                     GetComponent<Mover>().StartMoveAction(target, 1f);
                    
//                 }
//                 SetCursor(CursorType.Movement);
//                 return true;
//             }
//             return false;
//         }

//         RaycastHit[] RaycastAllSorted()
//         {

//             //Get all hits
//             RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

//             //build array distnaance
//             //makes the size of array based on #of hits
//             float[] distances = new float[hits.Length];

//             //fills the distance array with element "distance" from hits array.
//             for (int i = 0; i < hits.Length; i++)
//             {
//                 distances[i] = hits[i].distance;

//             }
//             Array.Sort(distances, hits);
//             //Return
//             return hits;
//         }

//         private bool RaycastNavMesh(out Vector3 target)
//         {
//             target = new Vector3();
//             RaycastHit hit;
//             bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
//             if (!hasHit) return false;

//             NavMeshHit navMeshHit;
//             bool hasCastToNavMesh = NavMesh.SamplePosition(
//                 hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
//             if (!hasCastToNavMesh) return false;

//             target = navMeshHit.position;
//             //give caluclate an object that it can modify.
//             NavMeshPath path = new NavMeshPath();
//             bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
//             if (!hasPath) return false;
//             //if you can't get to targetted navmesh it will not show that you can go there.
//             if (path.status != NavMeshPathStatus.PathComplete) return false;
//             if (GetPathLength(path) > maxNavPathLength) return false;
//             return true;
//         }


//         //Limits character running crazy distances to get to navmesh point
//         private float GetPathLength(NavMeshPath path)
//         {
//             float total = 0;
//             if (path.corners.Length < 2) return total;
//             for (int i = 0; i < path.corners.Length - 1; i++)
//             {
//                 total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
//             }
//             //add print to tweak total distance
//             return total;
//         }

//         private void SetCursor(CursorType type)
//         {
//             CursorMapping mapping = GetCursorMapping(type);
//             Cursor.SetCursor(mapping.texture, mapping.hostpot, CursorMode.Auto);
//         }

//         private CursorMapping GetCursorMapping(CursorType type)
//         {
//             foreach (CursorMapping mapping in cursorMappings)
//             {
//                 if (mapping.type == type)
//                 {
//                     return mapping;
//                 }

//             }
//             return cursorMappings[0];
//         }


//         private static Ray GetMouseRay()
//         {
//             return Camera.main.ScreenPointToRay(Input.mousePosition);
//         }

//         public void MoveToTarget (Vector3 target)
//         {
//             GetComponent<Mover>().MoveTo(target, 1f);
//         }

//         public void AddItemToInventory(ItemConfig item, int quantity)
//         {
//             inventory.AddItem(new ItemInInventory { itemObject = item, itemQuantity = quantity });
//         }

//         private void CachePlayerClick(Interactable target)
//         {
//             cachePlayerObjects = new List<Interactable>();
//             cachePlayerObjects.Add(target);
            
//         }

//         public void RefreshCachePlayerClick()
//         {
//             cachePlayerObjects = new List<Interactable>();
//         }

//         public List<Interactable> GetCachePlayerClick()
//         {
//             return cachePlayerObjects;
//         }

//     }
// }
