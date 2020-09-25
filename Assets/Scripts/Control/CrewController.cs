using System;
using System.Collections;
using System.Collections.Generic;
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
        
    public class CrewController : MonoBehaviour
    {

        public List<CrewMember> currentTeam = new List<CrewMember>();
        public List<CrewMember> crewOnShip;

        public CrewMember currentCrewMember;
        public CrewMember selectedCrewMember;

        public CameraController cameraControl;
        float timeSinceClick;


        Interactable interactable;
        public delegate void InteractableAction(CrewController callingController);
        public InteractableAction interactableAction;

        public List<Interactable> cachePlayerObjects = new List<Interactable>();

        //template for perks to be added
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hostpot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavMeshProjectionDistance = 1f;
        [SerializeField] float maxNavPathLength = 40;

        [SerializeField] private UIPlayerInventory uIInventory;

        public CrewMember focus;
        

        bool isDraggingUI = false;

        private void Awake()
        {
            cachePlayerObjects = null;
            foreach (Transform child in gameObject.transform)
            {
                currentTeam.Add(child.GetComponent<CrewMember>());
            }
            GameEvents.instance.UpdateSelectedCrew(currentCrewMember);
        }

        private void Update()
        {

            
            //armorClass = GetComponent<BaseStats>().GetStat(Stat.ArmorClass);

            if (InteractWithUI()) return;

            

            if (currentCrewMember.health.IsDead())
            { 
                SetCursor(CursorType.None);
                return;
            }

            InteractWithKeyPress();
            
            if (InteractWithObject()) return;
            if (InteractWithMovement()) return;
            // print ("Nothing to do.");
            SetCursor(CursorType.None);
            
        }

        private void InteractWithKeyPress()
        {
            if (currentCrewMember.turnManager.GetCanMove())
            {
                currentCrewMember.mov.KeyMovement();
            }
            else
            {
                currentCrewMember.mov.GracefullyStopAnimate();
            }
                       
        }

        public void SetCrewMember(CrewMember calledCrew) 
        {
            if (currentCrewMember == calledCrew) return;
            currentCrewMember = calledCrew;
            cameraControl.SetCameraTarget(currentCrewMember);
            //let the world know you're selected character changed
            GameEvents.instance.UpdateSelectedCrew(currentCrewMember);
        }

        public void SelectNextCrewMember(CrewMember selected)
        {
            if (selected == currentCrewMember || selected.GetComponent<StateManager>().isInCombat) return;

            //cache the current player to set follow behavior.
            CrewMember oldCrewMember = currentCrewMember;

            //make sure that both characters STOP following anyone
            selected.StopFollowingTheLeader();
            currentCrewMember.StopFollowingTheLeader();
            
            //add the new crew to currentcrew
            currentCrewMember = selected;

            //set the old crewmember to follow new
            //set camera to focus on new crewmember

            oldCrewMember.FollowTheLeader(currentCrewMember);
            //set camera to focus on new crewmember
            cameraControl.SetCameraTarget(currentCrewMember);
            //let the world know you're selected character changed
            GameEvents.instance.UpdateSelectedCrew(currentCrewMember);
            
        }

        public List<CrewMember> ListCrew()
        {
            return currentTeam;
        }
        public List<CrewMember> ListCrewOnShip()
        {
            return crewOnShip;
        }

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

                            selectedCrewMember = hit.collider.GetComponent<CrewMember>();

                            if (selectedCrewMember != null)
                            {
                                SelectNextCrewMember(selectedCrewMember);
                            }

                            interactable = hit.collider.GetComponent<Interactable>();
                            if (interactable != null)
                            {
                                StartCoroutine(DefaultAct(this));
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

#region CharacterInteraction
        public IEnumerator DefaultAct(CrewController callingController)
        {
           
            //Move into range for action
            yield return new WaitUntil(() => (InRangeToInteract(currentCrewMember)));
            //Act
            interactable.DefaultInteract(currentCrewMember);
            
        }

        #region ActionMenuFunctions
        public IEnumerator Act(CrewController callingController)
        {
            //Move into range for action
            yield return new WaitUntil(() => (InRangeToInteract(currentCrewMember)));
            //Act
            interactableAction(callingController);
        }

        public void Attack(CrewController callingController)
        {
            CharacterInteraction newInteractable = (CharacterInteraction)interactable;
            newInteractable.AttackNPC(currentCrewMember);
        }
        public void Trade(CrewController callingController)
        {
            CharacterInteraction newInteractable = (CharacterInteraction)interactable;
            newInteractable.ShopMenu(currentCrewMember);
        }
        public void Talk(CrewController callingController)
        {
            CharacterInteraction newInteractable = (CharacterInteraction)interactable;
            newInteractable.TalkToNPC(currentCrewMember);
        }
        public void Move(CrewController callingController)
        {
            currentCrewMember.MoveToTarget(interactable.transform.position);
        }
        public void Open(CrewController callingController)
        {
            interactable.DefaultInteract(currentCrewMember);
        }
        public void PickUp(CrewController callingController)
        {
            ItemInWorld newInteractable = (ItemInWorld)interactable;
            newInteractable.PickUp(currentCrewMember);
        }
        public void Inspect(CrewController callingController)
        {
            //add some kind of functionality
            interactable.DefaultInteract(currentCrewMember);
        }
        public void Scan(CrewController callingController)
        {
            Debug.Log("Scanning ... beep...boop");
            CombatTarget newInteractable = (CombatTarget)interactable;
            newInteractable.Scan(callingController);
        }

        #endregion

        bool InRangeToInteract(CrewMember callingController)
        {
            float distanceToItem = Vector3.Distance(callingController.transform.position, interactable.transform.position);
            if (distanceToItem > interactable.interactRadius)
            {
                currentCrewMember.mov.MoveToInteract(interactable);
                return false;
                // callingController.MoveToTarget
            }
            else
            {
                return true;
            }
        }

        private void SetFollow(CrewMember newFocus)
        {
            focus = newFocus;
            Debug.Log (focus);
        }

        private void RemoveFollow()
        {
            focus = null;
            Debug.Log("focus removed");
        }

#endregion
        // reference this for making complex arrays based on others.



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



        private bool InteractWithMovement()
        {

            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (hasHit)
            {
                if (Input.GetMouseButtonDown(0) && currentCrewMember.turnManager.GetCanMove())
                {
                    StopAllCoroutines();
                    ActionMenu.HideMenuOptions_Static();
                    currentCrewMember.mov.StartMoveAction(target, 1f);

                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

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
            bool hasPath = NavMesh.CalculatePath(currentCrewMember.transform.position, target, NavMesh.AllAreas, path);
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

        

        private void CachePlayerClick(Interactable target)
        {
            cachePlayerObjects = new List<Interactable>();
            cachePlayerObjects.Add(target);

        }

        public void RefreshCachePlayerClick()
        {
            cachePlayerObjects = new List<Interactable>();
        }

        public List<Interactable> GetCachePlayerClick()
        {
            return cachePlayerObjects;
        }

        public CrewMember GetSelectedCrewMember()
        {
            return currentCrewMember;
        }

    }

}
