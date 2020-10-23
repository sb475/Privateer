using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using RPG.Base;
using RPG.Cinematic;
using RPG.Combat;
using RPG.Core;
using RPG.Global;
using RPG.Items;
using RPG.UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hostpot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavMeshProjectionDistance;
        [SerializeField] float maxNavPathLength = 40;

        [Header("Controllable Assets")]
        public List<CrewMember> currentTeam;
        public List<CrewMember> crewOnShip;
        public Ship ship;
        public bool canControlShip;
        public bool controllingShip;
        public float zoomContolTransition;
        public float maxZoomOnChar;
        public float maxZoomOnShip;

        public IControllable currentControllable;
        public IControllable lastControlledCrew;

        public CameraController cameraControl;
        float timeSinceClick;


        Interactable interactable;
        public Act command;

        //template for perks to be added

        [SerializeField] private UIPlayerInventory uIInventory;
        
        bool isDraggingUI = false;
        private bool isPaused;

        private void Awake()
        {
            command = new Act();
            SelectControllable(currentTeam[0]);
        }

        private void Start() {

            SetCamera();
        }

        private void Update()
        {
            maxNavMeshProjectionDistance = cameraControl.GetCameraZoom();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isPaused)
                {
                    Time.timeScale = 1f;
                    isPaused = false;
                }
                else
                 {
                     Time.timeScale = 0;
                    isPaused = true;

                }
            }

            KeyMapping();

            if (InteractWithUI()) return;

            //if (currentControllable.health.IsDead())
            //{ 
            //    SetCursor(CursorType.None);
            //    return;
            //}
            DetermineShipControl();
            MoveWithKeyPress();
            
            if (InteractWithObject()) return;
            if (InteractWithMovement()) return;
            // print ("Nothing to do.");
            SetCursor(CursorType.None);
            
        }

        private void KeyMapping()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SelectControllable(currentTeam[0]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SelectControllable(currentTeam[0]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SelectControllable(currentTeam[0]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SelectControllable(currentTeam[0]);
            }
        }
        #region DeterminePlaterControl

        private void DetermineShipControl ()
        { if (canControlShip)
            {
                
                if (cameraControl.GetCameraZoom() < zoomContolTransition)
                {  //add pause when zooming in
                    if (controllingShip)
                    {
                        SetCamera();
                        SetControllable(lastControlledCrew);
                        controllingShip = false;
                        maxNavPathLength = 40f;
                    }
                    controllingShip = false;
                    
                }
                else
                {
                    //need to add a performace modifier to control scale when zooming in and out.
                    controllingShip = true;
                    //if (currentControllable != ship)
                    //{
                    //    SetCamera();
                    //    lastControlledCrew = (CrewMember)currentControllable;
                    //    //SetControllable(ship);
                    //    maxNavPathLength = 2000f;
                    //}
                    
                }
            }
        }

        private void SetCamera ()
        {
            if (canControlShip)
            {
                cameraControl.SetZoomSpeed(100f);
                cameraControl.SetCameraMaxZoom(maxZoomOnShip);
            }
            else
            {   cameraControl.SetZoomSpeed(5f);
                cameraControl.SetCameraMaxZoom(maxZoomOnChar);
            }
        }
        private void MoveWithKeyPress()
        {

                currentControllable.KeyMovement();                       
        }

        public void SetControllable(IControllable calledObject) 
        {
            //additional guard statement
            if (currentControllable == calledObject) return;

            if (calledObject is CrewMember)
            {
                
                
            }
            else if (calledObject is Ship)
            {

            }
            currentControllable = calledObject;
            cameraControl.SetCameraTarget(currentControllable);
            //let the world know you're selected character changed
            //GameEvents.instance.UpdateSelectedCrew(currentControllable);
        }

        

        public void SelectControllable(IControllable selected)
        {
            if (selected == null) return;
            if (selected == currentControllable) return;

            currentControllable = selected;
            Debug.Log("Current controllable: " + currentControllable);
            cameraControl.SetCameraTarget(currentControllable);
        }

        public List<CrewMember> ListCrew()
        {
            return currentTeam;
        }
        public List<CrewMember> ListCrewOnShip()
        {
            return crewOnShip;
        }
#endregion

#region Interactable Behaviours

        private bool InteractWithObject()
        {
            //iterates through physical object in raycas
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                //looks for components to apply to IRaycastable
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType(this));

                        if (Input.GetMouseButtonDown(0))
                        {
                            StopAllCoroutines();
                            ActionMenu.HideMenuOptions_Static();

                            if (hit.collider.GetComponent<ControllableObject>())
                            {
                                if (hit.collider.GetComponent<CrewMember>())
                                {
                                    
                                    CrewMember selectedCrewMember = hit.collider.GetComponent<CrewMember>();

                                    if (selectedCrewMember != null)
                                    {
                                        SelectControllable(selectedCrewMember);
                                    }

                                }
                                else if (hit.collider.GetComponent<Ship>() && canControlShip)
                                {
                                    ship = hit.collider.GetComponent<Ship>();
                                    if (interactable != null)
                                    {

                                      Debug.Log("Click on ship behaviour");
                                    }
                                }
                            }
                            
                            else 
                            {
                                interactable = hit.collider.GetComponent<Interactable>();
                                if (interactable != null)
                                {
                                    Debug.Log(interactable.GetDefaultAction());
                                    currentControllable.IssueCommand(interactable.GetDefaultAction(), interactable.gameObject);
                                }
                            }                  

                        }

                        if (Input.GetMouseButtonDown(1))
                            {
                                ActionMenu.HideMenuOptions_Static();

                                interactable = hit.collider.GetComponent<Interactable>();
                                if (interactable != null)
                                {
                                    ActionMenu.HideMenuOptions_Static();
                                    ActionMenu.ShowMenuOptions_Static(interactable);

                            }
                            
                        }


                        return true;
                    }
                }
            }
            return false;
        }
#endregion


        // reference this for making complex arrays based on others.

        #region ActionMenu
        //public void SelectActionMenu(ActionMenuOptions selectedOptions, Interactable interactable)
        //{

        //    //actions based on clicking from mouse. Most rely on Coroutine Act, which functionality 
        //    //is changed by editing the delagate "interactableAction". See in PlayerController.cs. Some 
        //    //actions require to be manually altered at this time.

        //    switch (selectedOptions)
        //    {

        //        case ActionMenuOptions.Attack:
        //            RPG_TaskSystem.Task atkTask = new RPG_TaskSystem.Task.Attack { interactable =interactable, controllable = currentControllable };
        //            currentControllable.taskSystem.AddTask(atkTask);
        //            break;
        //        case ActionMenuOptions.Trade:
        //            RPG_TaskSystem.Task tradeTask = new RPG_TaskSystem.Task.Trade { interactable = interactable };
        //            currentControllable.taskSystem.AddTask(tradeTask);
        //            break;
        //        case ActionMenuOptions.Talk:
        //            RPG_TaskSystem.Task talkTask = new RPG_TaskSystem.Task.Talk { interactable = interactable };
        //            currentControllable.taskSystem.AddTask(talkTask);
        //            break;
        //        case ActionMenuOptions.Move:
        //            RPG_TaskSystem.Task moveTask = new RPG_TaskSystem.Task.MoveToPosition { targetPoistion = interactable.transform.position };
        //            currentControllable.taskSystem.AddTask(moveTask);
        //            break;
        //        case ActionMenuOptions.Open:
        //            RPG_TaskSystem.Task openTask = new RPG_TaskSystem.Task.Open { interactable = interactable };
        //            currentControllable.taskSystem.AddTask(openTask);
        //            break;
        //        case ActionMenuOptions.PickUp:
        //            RPG_TaskSystem.Task pickUpTask = new RPG_TaskSystem.Task.Pickup { interactable = interactable, controllable = currentControllable };
        //            currentControllable.taskSystem.AddTask(pickUpTask);
        //            break;
        //        case ActionMenuOptions.Inspect:
        //            RPG_TaskSystem.Task inspectTask = new RPG_TaskSystem.Task.Inspect { interactable = interactable };
        //            currentControllable.taskSystem.AddTask(inspectTask);
        //            break;
        //        case ActionMenuOptions.Scan:
        //            RPG_TaskSystem.Task scanTask = new RPG_TaskSystem.Task.Scan { interactable = interactable };
        //            currentControllable.taskSystem.AddTask(scanTask);
        //            break;
        //    }

        //    ActionMenu.HideMenuOptions_Static();
        //}

        #endregion

        #region InteractWithUI
        private bool InteractWithUI()
        {
            if (Input.GetMouseButtonUp(0))
            {
                isDraggingUI = false;
            }
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isDraggingUI = true;
                }
                SetCursor(CursorType.UI);
                return true;
            }
            if (isDraggingUI)
            {
                return true;
            }
            return false;
        }
#endregion


#region InteractWithMovement
        private bool InteractWithMovement()
        {
            Vector3 target;
                bool hasHit = RaycastNavMesh(out target);

                if (hasHit)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("Mouse clicked");

                    currentControllable.IssueCommand(ManualActions.Move, target);
                    }
                    SetCursor(CursorType.Movement);
                    return true;
                }
                return false;
            //}
        }

    #endregion


#region RayCast sorting and functions
        RaycastHit[] RaycastAllSorted()
        {

            //Get all hits
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            //build array distnaance
            //makes the size of array based on #of hits
            float[] distances = new float[hits.Length];

            //fills the distance array with element "distance" from hits array.
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;

            }
            Array.Sort(distances, hits);
            //Return
            return hits;
        }

        // private bool RayCastPlane(out Vector3 target)
        // {
        //     target = new Vector3();
        //     RaycastHit hit;
        //     bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
        //     if (!hasHit) return false;

            
        // }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(
                hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;
            
            //give caluclate an object that it can modify.
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(currentControllable.gameObject.transform.position, target, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            //if you can't get to targetted navmesh it will not show that you can go there.
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxNavPathLength) return false;
            return true;
        }


        //Limits character running crazy distances to get to navmesh point
        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            //add print to tweak total distance
            return total;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hostpot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }

            }
            return cursorMappings[0];
        }


        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
#endregion

        public IControllable GetCurrentControllable()
        {
            return currentControllable;
        }

        public CrewMember GetCurrentCrewMember()
        {
            if (GetCurrentControllable() as CrewMember != null)
            {
                return GetCurrentControllable() as CrewMember;
            }
            else {
                return lastControlledCrew as CrewMember;
            }
            
        }

    }

}
