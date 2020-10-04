    using UnityEngine;
    using RPG.Movement;
    using RPG.Combat;
    using RPG.Core;
    using RPG.Attributes;
    using GameDevTV.Utils;
    using System.Collections.Generic;
    using RPG.Global;
using System;
using RPG.Items;

namespace RPG.Control
    {

        public class AIController : MonoBehaviour
        {
            public float chaseDistance = 10f;
            [SerializeField] float suspicionTime = 3f;
            [SerializeField] float aggroCoolDownTime = 5f;
            [SerializeField] float waypointDwellTime = 3f;
            [SerializeField] PatrolPath patrolPath;
            [SerializeField] float waypointTolerance = 1f ;
            [Range(0,1)]
            [SerializeField] float patrolSpeedFraction = 0.2f;
            [SerializeField] AttitudeType attitude;

            public IAttack fighter;
            public List<CrewMember> playerTeam;
            IDamagable health;
            CharacterEngine mover;
            StateManager turnManager;
            
            public Transform boardingLocation;
            public CoverObject chosenCover;

            public float closeCombatRange = 10f;

            public CharacterFaction characterFaction;

            public IDamagable target;
        

        //AI memory
            LazyValue<Vector3> guardPosition;
            float timeSinceLastSawPlayer = Mathf.Infinity;
            float timeSinceLastWaypoint = Mathf.Infinity;
            float timeSinceAggrevated = Mathf.Infinity;
            int currentWayointIndex = 0;
        
            [SerializeField] private bool atBoardingLocation = false;

        private void Awake() {
                fighter = GetComponent<IAttack>();
                health = GetComponent<IDamagable>();
                mover = GetComponent<CharacterEngine>();
                guardPosition = new LazyValue<Vector3>(GetGuardPosition);
                turnManager = GetComponent<StateManager>();
                characterFaction = GetComponent<CharacterFaction>();
            }

            private Vector3 GetGuardPosition()
            {
                return transform.position;
            }
   
            private void Start() {      

                //guardPosition.ForceInit();
                playerTeam = GameEvents.instance.GetCrewRoster();
               
       
            }

            private void Update()
            {
            // if (health.IsDead()) 
            // {
            //     turnManager.SetToNonCombat();
            //     return;
            // }

            mover.MoveTo(target.gameObject.transform.position, 1f);


            // if (InCloseCombatRange()) return;


            // if (isInRange()) return;

            // if (HostileBehavior()) return;

            // //if (OnGuardDuty()) return;

            UpdateTimers();
        }

        private bool InCloseCombatRange()
        {
            if (target == null) return false;
            if (Vector3.Distance(transform.position, target.gameObject.transform.position) < 10)
            {
                mover.MoveTo(target.gameObject.transform.position, 1f);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool isInRange()
        {
            if (Vector3.Distance(transform.position, boardingLocation.position) > fighter.GetWeaponRange())
            {
                mover.MoveTo(boardingLocation.position, 1f);
                return false;
            }
            else
            {

                UseCover();
                return true;

            }
        }

        private bool OnGuardDuty()
        {
            if (turnManager.isInCombat == false)
            {


                if (timeSinceLastSawPlayer < suspicionTime)
                {
                    SuspicionBehavior();
                }
                else
                {
                    PatrolBehavior();
                    
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        public void Aggrevate()
        {
            turnManager.isInCombat = true;
        }

        private void UpdateTimers()
            {
                timeSinceLastSawPlayer += Time.deltaTime;
                timeSinceLastWaypoint += Time.deltaTime;
                timeSinceAggrevated += Time.deltaTime;
            }
#region Patrol Behaviour

            private void PatrolBehavior()
            {
                Vector3 nextPosition = guardPosition.value;

            if (patrolPath != null)
            {
            
                if (AtWayPoint())
                {   
                    timeSinceLastWaypoint = 0;
                    CycleWaypoint();
                }
            
                    nextPosition = GetCurrentWaypoint();
            }

                if (timeSinceLastWaypoint > waypointDwellTime)
                    {
                        mover.StartMoveAction(nextPosition, patrolSpeedFraction);
                }
           
            }

            private void CycleWaypoint()
            {
                currentWayointIndex = patrolPath.GetNextIndex(currentWayointIndex);
            }

            private Vector3 GetCurrentWaypoint()
            {
                return patrolPath.GetWaypoint(currentWayointIndex);
            }

            private bool AtWayPoint()
            {
                float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
                return distanceToWaypoint < waypointTolerance;
            }

            private void SuspicionBehavior()
            {
                GetComponent<ActionScheduler>().CancelCurrectAction();
                //take object out of combat
            }
        #endregion

#region TriggerAggresiveAction
        public bool HostileBehavior()
        {
            if (attitude == AttitudeType.Hostile)
            {
                float closetTarget = Mathf.Infinity;
               foreach (CrewMember crew in playerTeam)
               {
                   
                float distanceToPlayer = Vector3.Distance(crew.transform.position, transform.position);

                    if (distanceToPlayer < chaseDistance || turnManager.isInCombat)
                    {
                        //determine decision on what to target
                        if (distanceToPlayer < closetTarget)
                        {
                            target = crew.GetComponent<IDamagable>();
                            closetTarget = distanceToPlayer;
                        }
                        
                        CombatBehavior();
                        
                        return true;
                    }

                return false;
               }
            }
            else if (attitude == AttitudeType.Friendly)
            {
                //friends join in the fight
                return true;
            }


            return false;
        }

#endregion

#region CombatBehvaiour

        private void CombatBehavior()
        {

            if (fighter.CanAttack(target))
                {

                if (GameEvents.instance.battleEventCalled == false)
                {

                    GameEvents.instance.FightBreakOut(gameObject.transform.position);
                }

                //If enemy can act will attempt to crry out CombatBehvior (run and hit at you)
                if (turnManager.GetCanAct() == true)
                {

                    fighter.Attack(target);


                    // if (Vector3.Distance(target.transform.position, transform.position) < closeCombatRange)
                    // {
                    //     CloseIn(target.GetComponent<IDamagable>());
                    // }
                    // else
                    // {
                    //     fighter.Attack(target.GetComponent<IDamagable>());
                    // }
                    
                    timeSinceLastSawPlayer = 0;
                }
            }
            else
            {
                Debug.Log ("Cannot attack target");
            }
            
        }

        private void UseCover()
        {
            float dist = Mathf.Infinity;
            Vector3 chosenSpot = Vector3.zero;

            for (int i = 0; i < WorldController.instance.GetCoverSpots().Length; i++)
            {   
                //if someone is already using cover don't use it.
                if (WorldController.instance.GetCoverSpots()[i].inUse) continue;

                Vector3 hideDir = WorldController.instance.GetCoverSpots()[i].transform.position;
                Vector3 hidePos = WorldController.instance.GetCoverSpots()[i].transform.position + hideDir.normalized * 5;

                if (Vector3.Distance(this.transform.position, hidePos) < dist)
                {
                    chosenSpot = hidePos;
                    dist = Vector3.Distance(this.transform.position, hidePos);
                }
            }

            mover.StartMoveAction(chosenSpot, 1f);

        }
        
            private void OnDrawGizmos()
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, chaseDistance);
            }

            public void Stop()
            {
                GetComponent<ActionScheduler>().CancelCurrectAction();
            }
            public AttitudeType GetAttitude()
            { 
                return attitude;
            }
#endregion
            

        }

    }

