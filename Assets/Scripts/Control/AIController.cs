    using UnityEngine;
    using RPG.Movement;
    using RPG.Combat;
    using RPG.Core;
    using RPG.Attributes;
    using GameDevTV.Utils;
    using System.Collections.Generic;
    using RPG.Global;

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

            [SerializeField] GameObject battleController;
            [SerializeField] AttitudeType attitude;

            Fighter fighter;
            public List<CrewMember> playerTeam;
            Health health;
            Mover mover;
            CombatTarget combatTarget;
            StateManager turnManager;
            CharacterFaction characterFaction;
            GameObject target;
        

        //AI memory
            LazyValue<Vector3> guardPosition;
            float timeSinceLastSawPlayer = Mathf.Infinity;
            float timeSinceLastWaypoint = Mathf.Infinity;
            float timeSinceAggrevated = Mathf.Infinity;
            int currentWayointIndex = 0;

            private void Awake() {
                fighter = GetComponent<Fighter>();
                health = GetComponent<Health>();
                
                mover = GetComponent<Mover>();
                guardPosition = new LazyValue<Vector3>(GetGuardPosition);
                turnManager = GetComponent<StateManager>();
                characterFaction = GetComponent<CharacterFaction>();
                
            }
            private Vector3 GetGuardPosition()
            {
                return transform.position;
            }
   
            private void Start() {      

                guardPosition.ForceInit();
                playerTeam = GameEvents.instance.GetCrewRoster();
       
            }

            private void Update()
                {
                    if (health.IsDead()) 
                    {
                        turnManager.SetToNonCombat();
                        return;
                    }

                    if (IsAggrevated() && fighter.CanAttack(target))
                    {

                            if (GameEvents.instance.battleEventCalled == false)
                            {

                                GameEvents.instance.FightBreakOut(gameObject.transform.position);
                            }
            
                            //If enemy can act will attempt to crry out CombatBehvior (run and hit at you)
                            if (turnManager.GetCanAct() == true)
                            {

                                CombatBehavior();
                            }
                            else return;
                        }

                    else if (turnManager.isInCombat == false)
                    {
                        if (timeSinceLastSawPlayer < suspicionTime)
                        {
                            SuspicionBehavior();
                        }
                        else
                        {
                            PatrolBehavior();
                        }
                    }
                    else
                    {
                return;
            }

                    UpdateTimers();
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

            private void CombatBehavior()
            {
                            
                fighter.Attack(target);
                timeSinceLastSawPlayer = 0;

            }

            public bool IsAggrevated()
            {
                if (attitude == AttitudeType.Hostile)
                {    
                        foreach (CrewMember crew in playerTeam)
                        {
                            //get real angry if anyone from player party is in range and hostile
                            float distanceToPlayer = Vector3.Distance(crew.transform.position, transform.position);
                            
                            if (distanceToPlayer < chaseDistance || turnManager.isInCombat) 
                            {
                                target = crew.gameObject;
                                return true;
                            }                 
                        }

                    return false;
                }
            return false;
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
            

        }

    }



